using Npgsql;
using PostgreSQL_TelegramBot.Context;
using Renci.SshNet;

namespace PostgreSQL_TelegramBot.Actions
{
    public static class ActionsPool
    {
        delegate Task<string> ExecuteAsyncCallback(DatabaseInfo db);

        private static Dictionary<ActionType, ExecuteAsyncCallback> _actions = new Dictionary<ActionType, ExecuteAsyncCallback>()
        {
            { ActionType.RELOAD,                ReloadAsync             },
            { ActionType.LOGS_CLEANING,         LogsCleaningAsync       },
            { ActionType.CLOSE_TRANSACTION ,    CloseTransactionAsync   },
            { ActionType.REBUILDING_INDEXES,    RebuildingIndexesAsync  },
            { ActionType.MAKE_BACKUP,           MakeBackupAsync         },
            { ActionType.CLOSE_ALL_REQUESTS,    CloseAllRequestsAsync   }
        };

        private const string SUCCESS = "OK";

        public static async Task<string> ExecuteAsync(ActionType type, DatabaseInfo db)
        {
            string res;

            try
            {
                res = await _actions[type](db);
            }
            catch (Exception e)
            {
                res = e.Message;
            }

            return res;
        }

        private static async Task<string> ReloadAsync(DatabaseInfo db) 
        {
            using (var connection = new NpgsqlConnection(db.ConnectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("SELECT pg_reload_conf();", connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    await reader.ReadAsync();

                    return reader["pg_reload_conf"].ToString();
                }
            }
        }

		private static async Task<string> LogsCleaningAsync(DatabaseInfo db)
        {
			using (var connection = new NpgsqlConnection(db.ConnectionString))
			using (var command = new NpgsqlCommand("VACUUM FULL;", connection))
            {
                await command.ExecuteNonQueryAsync();
                return SUCCESS;
            }
        }

		private static async Task<string> CloseTransactionAsync(DatabaseInfo db)
        {
            using (var connection = new NpgsqlConnection(db.ConnectionString))
            using (var command = new NpgsqlCommand("SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE pg_stat_activity.datname = 'postgres' AND pg_stat_activity.pid <> pg_backend_pid();", connection))
            {
                await command.ExecuteNonQueryAsync();
                return SUCCESS;
			}
        }

		private static async Task<string> RebuildingIndexesAsync(DatabaseInfo db)
        {
			using (var connection = new NpgsqlConnection(db.ConnectionString))
			using (var command = new NpgsqlCommand("REINDEX DATABASE @dbname;", connection))
            {
                command.Parameters.Add(new NpgsqlParameter("@dbname", db.DatabaseName));

                await command.PrepareAsync();
                await command.ExecuteNonQueryAsync();

                return SUCCESS;
            }
        }

        private static async Task<string> MakeBackupAsync(DatabaseInfo db)
        {
            if (!db.SshEnabled)
            {
                return "Отказано. Нет доступа к SSH";
            }

            var script = $"{db.PGBinaryDirectory}/pg_dump -U {db.UserName} -d {db.DatabaseName} -f {db.BackupDirectory}";

            using(var sshClient = new SshClient(db.SshHost, db.SshUser, db.SshPassword))
            {
                sshClient.Connect();
                sshClient.RunCommand(script);
            }

			await Storage.SetAsync(db.DatabaseName, new DatabasePersistentInfo
            {
                DatabaseName = db.DatabaseName,
                LastBackupDate = DateTime.UtcNow,
            });

            return SUCCESS;
        }

		public static async Task<string> CloseAllRequestsAsync(DatabaseInfo db)
		{
			using (var connection = new NpgsqlConnection(db.ConnectionString))
			using (var command = new NpgsqlCommand("SELECT pg_terminate_backend(pg_stat_activity.pg_backend_pid) FROM pg_stat_activity WHERE pg_stat_activity.datname = @dbname AND state = 'active'", connection))
			{
				command.Parameters.Add(new NpgsqlParameter("@dbname", db.DatabaseName));

				await command.ExecuteNonQueryAsync();

				return SUCCESS;
			}
		}
	}
}