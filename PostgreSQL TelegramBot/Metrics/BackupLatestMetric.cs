using PostgreSQL_TelegramBot.Context;

namespace PostgreSQL_TelegramBot.Metrics
{
	public sealed class BackupLatestMetric : MetricBase
	{
		public override MetricType Type { get; } = MetricType.BACKUP_LATEST_METRIC;

		public override BackupLatestMetric Clone()
		{
			return new BackupLatestMetric();
		}

		public override async Task Update()
		{
			var info = await Storage.GetAsync(Database.DatabaseName);

			CurrentValue = info?.LastBackupDate;
		}
	}
}
