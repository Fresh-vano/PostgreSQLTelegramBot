using MongoDB.Bson.Serialization.Attributes;

namespace PostgreSQL_TelegramBot.Context
{
	public class DatabasePersistentInfo
	{
		[BsonId]
		public string? DatabaseName { get; set; }

		public DateTime? LastBackupDate { get; set; }
	}
}
