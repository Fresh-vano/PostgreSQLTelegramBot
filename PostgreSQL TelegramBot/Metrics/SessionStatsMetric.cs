using Npgsql;
using System.Collections.Generic;

namespace PostgreSQL_TelegramBot.Metrics
{
	public class SessionStats
	{
		public int? Total { get; set; } = null;
		public int? TotalWithLWLocks { get; set; } = null;
		public int? TotalWithLocks { get; set; } = null;
	}

	public sealed class SessionStatsMetric : MetricBase
	{
		public override MetricType Type { get; } = MetricType.SESSION_STATS_METRIC;

		public override SessionStatsMetric Clone()
		{
			return new SessionStatsMetric();
		}

		protected override async Task<object?> UpdateInternal(NpgsqlConnection connection)
		{
			SessionStats stats = new SessionStats();

			using (var command = new NpgsqlCommand(
			@"	SELECT COUNT(*) c FROM pg_stat_activity
				WHERE datname = @dbname AND usename <> @usename AND state = @state
				UNION ALL
				SELECT COUNT(*) c FROM pg_stat_activity
				WHERE datname = @dbname AND usename <> @usename AND wait_event_type = @wetlw
				UNION ALL
				SELECT COUNT(*) c FROM pg_stat_activity
				WHERE datname = @dbname AND usename <> @usename AND wait_event_type = @wetl
			", connection
			))
			{
				command.Parameters.Add(new NpgsqlParameter("@dbname", Database.DatabaseName));
				command.Parameters.Add(new NpgsqlParameter("@usename", Database.UserName));
				command.Parameters.Add(new NpgsqlParameter("@state", "active"));
				command.Parameters.Add(new NpgsqlParameter("@wetlw", "LWLock"));
				command.Parameters.Add(new NpgsqlParameter("@wetl", "Lock"));

				if (command is null)
					Console.WriteLine("command null\n");

				await command.PrepareAsync();

				var reader = await command.ExecuteReaderAsync();

				if (await reader.ReadAsync())
					stats.Total = Convert.ToInt32(reader["c"]);

				if (await reader.ReadAsync())
					stats.TotalWithLWLocks = Convert.ToInt32(reader["c"]);

				if (await reader.ReadAsync())
					stats.TotalWithLocks = Convert.ToInt32(reader["c"]);

				await reader.CloseAsync();
			}

			return stats;
		}
	}
}