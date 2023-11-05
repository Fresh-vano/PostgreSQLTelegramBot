using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace PostgreSQL_TelegramBot.Context
{
	public static class Storage
	{
		private const string kDbName = "storage";

		private static string _connectionString = string.Empty;
		private static MongoClient _client = new MongoClient();

		public static void Initialize(string connectionstring)
		{
			_connectionString = connectionstring;
			_client = new MongoClient(connectionstring);

			var db = _client.GetDatabase(kDbName);
			db.CreateCollection(kDbName);
		}

		public static async Task RegisterAsync(string name)
		{
			var db = FetchTargetCollection();
			var filter = Builders<BsonDocument>.Filter.Eq("DatabaseName", name);
			var data = await db.Find(filter).ToListAsync();
			
			if (data is null) 
			{
				var dpi = new DatabasePersistentInfo 
				{ 
					DatabaseName = name, 
					LastBackupDate = null 
				};

				db?.InsertOne(dpi.ToBsonDocument());
			}
		}

		public static async Task<DatabasePersistentInfo?> GetAsync(string name)
		{
			var db = FetchTargetCollection();
			var filter = Builders<BsonDocument>.Filter.Eq("DatabaseName", name);
			var data = await db.Find(filter).ToListAsync();

			if (data is null || data.Count == 0)
			{
				return null;
			}

			return BsonSerializer.Deserialize<DatabasePersistentInfo>(data.First());
		}

		public static async Task SetAsync(string name, DatabasePersistentInfo value)
		{
			var db = FetchTargetCollection();
			var filter = Builders<BsonDocument>.Filter.Eq("DatabaseName", name);
			await db?.ReplaceOneAsync(filter, value.ToBsonDocument());
		}

		private static IMongoCollection<BsonDocument>? FetchTargetCollection()
		{
			return _client
				.GetDatabase(kDbName)
				.GetCollection<BsonDocument>(kDbName);
		}
	}
}
