using Npgsql;

namespace PostgreSQL_TelegramBot.Metrics
{
	public sealed class DiskSpaceMetric : MetricBase
	{
		public override MetricType Type { get; } = MetricType.DISK_SPACE_METRIC;

		public override DiskSpaceMetric Clone()
		{
			return new DiskSpaceMetric();
		}

		protected override async Task<object?> UpdateInternal(NpgsqlConnection connection)
		{
			string? res = null;

			using (var command = new NpgsqlCommand(
				@"SELECT pg_database.datname, pg_size_pretty(pg_database_size(pg_database.datname)) AS size
                FROM pg_database WHERE pg_database.datname = @dbname 
                ORDER BY pg_database_size(pg_database.datname) DESC", connection
			))
			{
				command.Parameters.Add(new NpgsqlParameter("@dbname", Database.DatabaseName));

				await command.PrepareAsync();

				var reader = await command.ExecuteReaderAsync();

				if (await reader.ReadAsync() && reader["size"] != null)
					res = reader["size"].ToString();

				await reader.CloseAsync();
			}

			return res;
		}
	}
}
