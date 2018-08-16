using System;
using System.Linq;
using System.Web.Script.Serialization;

#pragma warning disable 0649
namespace TestAddin
{
	// Needed in multiple places so I pulled this instance out into this class
	static class Json
	{
		public static JavaScriptSerializer Serializer = new JavaScriptSerializer();
	}

	// These could also be classes but I chose structs since UserCreds is required to be
	// a struct for the magic in Authentication.cs
	
	struct UserCreds
	{
		public string domain;
		public string username;
		public string password;
		public string commserver;
	}

	struct LoginInformation
	{
		public OrganizationInformation ownerOrganization;
		public string aliasName;
		public Guid userGUID;
		public long loginAttempts;
		public long remainingLockTime;
		public string smtpAddress;
		public ProviderInformation providerOrganization;
		public LoginError[] errList;
		public string username;
		public long providerType;
		public long ccn;
		public string token;
		public long capability;
		public bool forcePasswordChange;
		public bool isAccountLocked;

		public struct OrganizationInformation
		{
			public int providerId;
			public string providerDomainName;
		}
		public struct ProviderInformation
		{
			public int providerId;
			public string providerDomainName;
		}
		public struct LoginError
		{
			public int errorCode;
			public string errLogMessage;
		}
	}

	struct IndexServerRecord
	{
		public string hostName;
		public int clientId;
		public string clientName;
		public string cIServerURL;
		public int type;
		public string version;
		public int basePort;
		public int cloudID;
		public string engineName;
		public int indexServerClientId;

		public override string ToString() => hostName;

		public override int GetHashCode() => clientId;
		public override bool Equals(object obj) => obj is IndexServerRecord isr ? this == isr : false;
		public static bool operator !=(IndexServerRecord a, IndexServerRecord b) => !(a == b);
		public static bool operator ==(IndexServerRecord a, IndexServerRecord b)
			=> a.hostName == b.hostName
			&& a.clientId == b.clientId
			&& a.clientName == b.clientName
			&& a.cIServerURL == b.cIServerURL
			&& a.type == b.type
			&& a.version == b.version
			&& a.basePort == b.basePort
			&& a.cloudID == b.cloudID
			&& a.engineName == b.engineName
			&& a.indexServerClientId == b.indexServerClientId;
	}

	struct IndexServers
	{
		public IndexServerRecord[] listOfCIServer;

		public static IndexServers Deserialize(string s) => Json.Serializer.Deserialize<IndexServers>(s);
	}

	struct DataSourceInfo
	{
		public int ownerType;
		public Guid datasourceGuid;
		public string description;
		public int userId;
		public int datasourceType;
		public string zkQuorum;
		public int datasourceId;
		public long createUTCTime;
		public long attribute;
		public string datasourceName;
		public int modifiedByUserId;
		public long modifiedUTCTime;
		public DataSourceProperty[] properties;

		public override int GetHashCode() => datasourceId;
		public override bool Equals(object obj) => obj is DataSourceInfo dsi ? this == dsi : false;
		public static bool operator !=(DataSourceInfo a, DataSourceInfo b) => !(a == b);
		public static bool operator ==(DataSourceInfo a, DataSourceInfo b)
			=> a.ownerType == b.ownerType
			&& a.datasourceGuid == b.datasourceGuid
			&& a.description == b.description
			&& a.userId == b.userId
			&& a.datasourceType == b.datasourceType
			&& a.zkQuorum == b.zkQuorum
			&& a.datasourceId == b.datasourceId
			&& a.createUTCTime == b.createUTCTime
			&& a.attribute == b.attribute
			&& a.datasourceName == b.datasourceName
			&& a.modifiedByUserId == b.modifiedByUserId
			&& a.modifiedUTCTime == b.modifiedUTCTime
			&& ((a.properties is null)? b.properties is null : a.properties.SequenceEqual(b.properties));

		public override string ToString() => datasourceName;

		public struct DataSourceProperty
		{
			public string propertyName;
			public string propertyNameSEA;
			public string propertyValue;
			public int propertyId;

			public override int GetHashCode() => propertyId;
			public override bool Equals(object obj) => obj is DataSourceProperty dsp ? this == dsp : false;
			public static bool operator !=(DataSourceProperty a, DataSourceProperty b) => !(a == b);
			public static bool operator ==(DataSourceProperty a, DataSourceProperty b)
				=> a.propertyName == b.propertyName
				&& a.propertyNameSEA == b.propertyNameSEA
				&& a.propertyValue == b.propertyValue
				&& a.propertyId == b.propertyId;
		}
	}

	struct DataSourceRecord
	{
		public int clientId;
		public string comutedCoreName;
		public string description;
		public string coreName;
		public int cloudId;
		public int ownerUserId;
		public string schemaType;
		public long createUTCTime;
		public int coreId;
		public long attribute;
		public DataSourceInfo[] datasources;
	}

	struct DataSources
	{
		public DataSourceRecord[] collections;

		public static DataSources Deserialize(string s) => Json.Serializer.Deserialize<DataSources>(s);
	}

	struct Handler
	{
		public string handlerName;
		public int dataSourceId;
		public string dataSourceName;
		public int handlerId;
		public HandlerProperties handlerInfo;
		public long attribute;

		public override int GetHashCode() => handlerId;
		public override bool Equals(object obj) => obj is Handler h ? this == h : false;
		public static bool operator !=(Handler a, Handler b) => !(a == b);
		public static bool operator ==(Handler a, Handler b)
			=> a.handlerName == b.handlerName
			&& a.dataSourceId == b.dataSourceId
			&& a.dataSourceName == b.dataSourceName
			&& a.handlerId == b.handlerId
			&& a.handlerInfo == b.handlerInfo
			&& a.attribute == b.attribute;

		public override string ToString() => handlerName;

		public struct HandlerProperties
		{
			public int version;
			public bool alwaysDecode;
			public dynamic appendParams;
			public dynamic invariantParams;
			public dynamic defaultParams;
			public dynamic rawAppendParams;
			public dynamic rawInvariantParams;
			public dynamic rawDefaultParams;

			public override int GetHashCode() => base.GetHashCode();
			public override bool Equals(object obj) => obj is HandlerProperties hp ? this == hp : false;
			public static bool operator !=(HandlerProperties a, HandlerProperties b) => !(a == b);
			public static bool operator ==(HandlerProperties a, HandlerProperties b)
				=> a.version == b.version
				&& a.alwaysDecode == b.alwaysDecode
				&& a.appendParams == b.appendParams
				&& a.invariantParams == b.invariantParams
				&& a.defaultParams == b.defaultParams
				&& a.rawAppendParams == b.rawAppendParams
				&& a.rawInvariantParams == b.rawInvariantParams
				&& a.rawDefaultParams == b.rawDefaultParams;
		}
	}

	struct HandlerInfo
	{
		public Handler[] handlerInfos;

		public static HandlerInfo Deserialize(string s) => Json.Serializer.Deserialize<HandlerInfo>(s);
	}

	struct CreateDataSourceInfo
	{
		public CollectionReq collectionReq;
		public CreateDataSourceDescriptor dataSource;

		//public string Serialized { get => Json.Serializer.Serialize(this); }

		public CreateDataSourceInfo(int cloudId, string name, int type, string description = "", long attribute = 0)
		{
			collectionReq = new CollectionReq(name, cloudId);
			dataSource = new CreateDataSourceDescriptor(name, type, description, attribute);
		}

		public string Serialize() => Json.Serializer.Serialize(this);

		public struct CollectionReq
		{
			public string collectionName;
			public CIServer ciserver;

			public CollectionReq(string name, int cloudId)
			{
				collectionName = name;
				ciserver = new CIServer(cloudId);
			}

			public struct CIServer
			{
				public int cloudID;
				public CIServer(int cloudId) => this.cloudID = cloudId;
			}
		}
		public struct CreateDataSourceDescriptor
		{
			public string description;
			public int datasourceType;
			public long attribute;
			public string datasourceName;

			public CreateDataSourceDescriptor(string name, int type, string description = "", long attribute = 0)
			{
				datasourceName = name;
				datasourceType = type;
				this.attribute = attribute;
				this.description = description;
			}
		}
	}

	struct HandlerResponse
	{
		public HandlerResponseInternal response;

		public static HandlerResponse Deserialize(string s) => Json.Serializer.Deserialize<HandlerResponse>(s);

		public struct HandlerResponseInternal
		{
			public int numFound;
			public int start;
			public dynamic[] docs;
		}
	}

	struct Schema
	{
		public string uniqueKey;
		public Field[] schemaFields;
		public string[] fieldTypes;
		public Field[] dynSchemaFields;

		public struct Field
		{
			public bool omitNorms;
			public bool omitTermFreqAndPositions;
			public string fieldName;
			public bool indexed;
			public bool autocomplete;
			public bool termVectors;
			public bool docValues;
			public string type;
			public bool required;
			public bool searchDefault;
			public bool spellcheck;
			public bool stored;
			public bool dynamicField;
			public bool omitPositions;
			public bool multiValued;
			public bool skipdelete;
			public string[] copyFields;
		}
	}
	
	struct SchemaCollection
	{
		public SchemaWrapper[] collections;

		public static SchemaCollection Deserialize(string s) => Json.Serializer.Deserialize<SchemaCollection>(s);

		public struct SchemaWrapper
		{
			public Schema schema;
		}
	}

	struct DSTimestampStruct
	{
		public ResponseHeader responseHeader;
		public dynamic index;

		public struct ResponseHeader
		{
			public long status;
			public long QTime;
		}

		public static DSTimestampStruct Deserialize(string s) => Json.Serializer.Deserialize<DSTimestampStruct>(s);
	}

	// There are two conflicting definitions for this type:
	// http://documentation.commvault.com/commvault/v11/article?p=44202.htm
	// http://documentation.commvault.com/commvault/v11/article?p=44158.htm
	class DataSourceType
	{
		public const int Database = 1;
		public const int Website = 2;
		public const int NAS = 3; // THIS VALUE MAY BE WRONG -- I COULDN'T VALIDATE THIS SINCE THERE ARE NO NAS DATASOURCES
		public const int CSV = 4;
		public const int FileSystem = 5;
		public const int Eloqua = 6; // SAME AS ABOVE COMMENT
		public const int Salesforce = 8; // SAME AS ABOVE COMMENT
		public const int LDAP = 9;
		public const int FederatedSearch = 10;
		public const int OpenDataSource = 11;
		public const int HTTP = 12;
		public const int Facebook = 14;
		public const int Twitter = 19;
	}
}
