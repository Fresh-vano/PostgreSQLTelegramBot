using Npgsql;

namespace PostgreSQL_TelegramBot.Metrics
{
	/// <summary>
	/// Класс работы со временем последнего изменения состояния.
	/// </summary>
	public sealed class StateMetric : MetricBase
    {
        public override MetricType Type { get; } = MetricType.STATE_METRIC;

        public override StateMetric Clone()
        {
            return new StateMetric();
        }

        protected override async Task<object?> UpdateInternal(NpgsqlConnection connection)
        {
            string? res = null;

            using (var command = new NpgsqlCommand(
                @"SELECT state FROM pg_stat_activity WHERE datname = @dbname GROUP BY state;", connection
            ))
            {
                command.Parameters.Add(new NpgsqlParameter("@dbname", Database.DatabaseName));

                await command.PrepareAsync();

                var reader = await command.ExecuteReaderAsync();

				if (await reader.ReadAsync() && reader["state"] != null)
                {
					res = reader["state"].ToString();
				}

				await reader.CloseAsync();
			}

            return res;
        }
    }
}