using Npgsql;
using PostgreSQL_TelegramBot.Metrics;

namespace PostgreSQL_TelegramBot.Metrics
{
    public sealed class StateLastTimeMetric : MetricBase
    {
        public override MetricType Type { get; } = MetricType.STATE_LAST_TIME_METRIC;

        public override object Clone()
        {
            return new StateLastTimeMetric();
        }

        protected override async Task<object?> UpdateInternal(NpgsqlConnection connection)
        {
            string? res = null;

            using (var command = new NpgsqlCommand(
                    @"SELECT MAX(now() - pg_stat_activity.query_start) AS last_state_change FROM pg_stat_activity WHERE datname = @dbname ORDER BY last_state_change DESC", connection
            ))
            {
                command.Parameters.Add(new NpgsqlParameter("@dbname", Database.DatabaseName));

                await command.PrepareAsync();

                var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync() && reader["last_state_change"] != null)
                {
                    res = reader["last_state_change"].ToString();
                }

                await reader.CloseAsync();
            }

            return res;
        }
    }
}