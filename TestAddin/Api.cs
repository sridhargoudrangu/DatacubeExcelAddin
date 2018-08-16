using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;

using TestAddin.Properties;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace TestAddin
{
	// This class lays out the Apis I knew of when I wrote the project
	class Api
	{
		// This will get the relevant HTTP method ("GET"/"POST"/...) assiociated with the Api
		public string Method
		{
			get
			{
				switch(Path)
				{
					case KnownApis.login:
					case KnownApis.DataCube.createDataSource:
					case KnownApis.DataCube.deleteDataSource:
					case KnownApis.DataCube.post:
					case KnownApis.DataCube.updateschema:
						return HttpMethods.POST;

					case KnownApis.DataCube.getAnalyticsEngine:
					case KnownApis.DataCube.search:
					case KnownApis.DataCube.handler:
					case KnownApis.DataCube.GetDataSources:
					case KnownApis.DataCube.GetStatus:
					case KnownApis.DataCube.GetHistory:
					case KnownApis.DataCube.GetJDBCDrivers:
					case KnownApis.DataCube.getcommcellinfo:
					case KnownApis.DataCube.gethandler:
					case KnownApis.DataCube.getDSSchema:
						return HttpMethods.GET;

					default:
						throw new ArgumentException(string.Format(Resources.UnknownAPIException, Path));
				}
			}
		}

		public string Host { get; private set; }     // http:// <  THIS PART   > / blah/blah/endpoint ?q=query&s=string
		public string Path { get; private set; }     // http:// somehostname.com / <   THIS PART    > ?q=query&s=string
		public string PostPath { get; private set; } // http:// somehostname.com / blah/blah/endpoint <   THIS PART   >
		public string Body { get; private set; } // The body of the request
		public bool Json { get; private set; } // Do we want the server to talk to us in json?

		// Ease of use "almost full url maker"
		public string FullApiString { get => $"{Path}{PostPath}"; }

		// Ease of use constructor
		public Api(string host, string path, string postpath = default, string body = default, bool json = true)
		{
			Host = host;
			Path = path;
			PostPath = postpath;
			Body = body;
			Json = json;
		}

		public void Execute(out Stream result)
		{
			// Start writing the http request
			HttpWebRequest req = WebRequest.CreateHttp($"http://{Host}{Settings.Default.WebconsoleApiPath}{FullApiString}");
			req.Accept = req.ContentType = (Json ? "application/json" : "text/plain") + "; charset=utf-8";
			WebResponse response;
			string token;

			//req.Timeout = Settings.Default.OperationTimeout;

			// if this isn't a get request (or there's a body) then we have a body to write
			if((req.Method = Method) != HttpMethods.GET || !(Body is null))
				using(var sw = new StreamWriter(req.GetRequestStream()))
					sw.Write(Body);

			// if logged in, append authtoken
			if(Authentication.LoggedIn)
			{
				unsafe
				{
					// authtoken should be zeroed after use to avoid in-memory duplication
					fixed (char* ptoken = (token = Authentication.LoginToken.ToString()))
					{
						req.Headers.Add("Authtoken", token); // add token to header

						response = req.GetResponse();

						// zero login token
						for(int i = 0; i < token.Length; ptoken[i++] = default) ;
					}
				}
			}
			else
				response = req.GetResponse();

			result = response.GetResponseStream();
		}

		public bool Execute(out string result)
		{
			try
			{
				Execute(out Stream respstream);

				using(StreamReader sr = new StreamReader(respstream))
					result = sr.ReadToEnd();

				// success
				return true;
			}
			catch(Exception ex)
			{
				// an exception was raised, err message into result and return false
				result = string.Format(Resources.RestApiExceptionText, FullApiString, Host, ex.Message);
				return false;
			}
		}
		public bool ExecuteLoggedIn(out string result)
		{
			// if we cannot successfully execute our rest api...
			if(!Execute(out result))
			{
				// and if we are able to re-establish our login
				if(!Authentication.ReEstablishLogin())
					return false;

				return Execute(out result); // retry with login
			}
			return true; // the api did not initially error
		}
		public void ExecuteLoggedIn(out Stream result)
		{
			try
			{
				Execute(out result);
			}
			catch
			{
				if(!Authentication.ReEstablishLogin())
					throw new OperationCanceledException(Resources.UnableToContinue);
				try
				{
					Execute(out result);
				}
				catch
				{
					throw new OperationCanceledException(Resources.UnableToContinue);
				}
			}
		}

		public class Handler : Api
		{
			public Handler(string host, TestAddin.Handler h, bool json = false, long maxrows = Useful.EXCEL_MAX_ROWS - 1, long skip = 0) : base(host, KnownApis.DataCube.handler, $"/{h.handlerId}/{h.handlerName}?q=%2A:%2A&start={skip}&wt={(json ? "json" : "csv")}&rows={maxrows}") { }
			public Handler(string host, long id, string name, bool json = false, long maxrows = Useful.EXCEL_MAX_ROWS - 1, long skip = 0) : base(host, KnownApis.DataCube.handler, $"/{id}/{name}?q=%2A:%2A&start={skip}&wt={(json ? "json" : "csv")}&rows={maxrows}") { }
		}
		public class GetNumRecords : Handler
		{
			public GetNumRecords(string host, TestAddin.Handler h) : base(host, h, true, 0) { }
			public GetNumRecords(string host, long id, string name) : base(host, id, name, true, 0) { }

			public bool ExecuteLoggedIn(out int numrecords)
			{
				bool c;
				numrecords = (c = ExecuteLoggedIn(out string result)) ? HandlerResponse.Deserialize(result).response.numFound : -1;
				return c;
			}
		}
		public class Login : Api
		{
			public Login(string host, string body) : base(host, KnownApis.login, body: body) { }
		}
		public class Create : Api
		{
			public Create(string host, string body) : base(host, KnownApis.DataCube.createDataSource, body: body) { }
		}
		public class Post : Api
		{
			public Post(string host, string postpath, string body) : base(host, KnownApis.DataCube.post, postpath, body) { }
			public Post(string host, int dsid, string body, bool json = true) : base(host, KnownApis.DataCube.post, $"/{(json ? "json" : "csv")}/{dsid}", body, json) { }
		}
		public class GetAnalyticsEngine : Api
		{
			public GetAnalyticsEngine(string host) : base(host, KnownApis.DataCube.getAnalyticsEngine) { }
		}
		public class GetDataSources : Api
		{
			public GetDataSources(string host) : base(host, KnownApis.DataCube.GetDataSources) { }
		}
		public class GetHandlers : Api
		{
			public GetHandlers(string host, string postpath) : base(host, KnownApis.DataCube.gethandler, postpath) { }
		}
		public class GetSchema : Api
		{
			public GetSchema(string host, int dsid) : base(host, KnownApis.DataCube.getDSSchema, $"/{dsid}") { }

			public bool ExecuteLoggedIn(out IEnumerable<Schema> schemas)
			{
				bool c;
				schemas = (c = ExecuteLoggedIn(out string result)) ? SchemaCollection.Deserialize(result).GetSchemata() : null;
				return c;
			}
		}
		public class GetTimestamp : Api
		{
			public GetTimestamp(string host, int dsid) : base(host, KnownApis.DataCube.search, $"/{dsid}/admin/luke?show=index") { }
			public bool ExecuteLoggedIn(out DateTime timestamp)
			{
				bool c = ExecuteLoggedIn(out string result);
				try
				{
					timestamp = DateTime.Parse(DSTimestampStruct.Deserialize(result).index["lastModified"]);
				}
				catch
				{
					timestamp = DateTime.MinValue;
				}
				return c;
			}
		}
		public class UpdateSchema : Api
		{
			public UpdateSchema(string host, int dsid, SchemaModel model) : base(host, KnownApis.DataCube.updateschema, body: model.ToJson(dsid)) { }
		}

		// const strings containing HTTP methods
		static class HttpMethods
		{
			public const string POST = "POST";
			public const string GET = "GET";
			public const string DELETE = "DELETE";
			public const string PUT = "PUT";
		}

		// const strings containing Paths to rest apis
		static class KnownApis
		{
			public const string login = "login";
			public static class DataCube
			{
				public const string getAnalyticsEngine = "dcube/getAnalyticsEngine";
				public const string createDataSource = "dcube/createDataSource";
				public const string search = "dcube/search";
				public const string handler = "dcube/handler";
				public const string GetDataSources = "dcube/GetDataSources";
				public const string GetStatus = "dcube/GetStatus";
				public const string GetHistory = "dcube/GetHistory";
				public const string GetJDBCDrivers = "dcube/GetJDBCDrivers";
				public const string getcommcellinfo = "dcube/getcommcellinfo";
				public const string deleteDataSource = "dcube/deleteDataSource";
				public const string gethandler = "dcube/gethandler";
				public const string post = "dcube/post";
				public const string getDSSchema = "dcube/getDSSchema";
				public const string updateschema = "dcube/updateschema";
			}
		}
	}
}
