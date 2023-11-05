using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using System.Collections;

namespace PostgreSQL_TelegramBot.Metrics
{
    /// <summary>
    /// Тип метрик.
    /// </summary>
    public enum MetricType
    {
        STATE_METRIC,
        STATE_LAST_TIME_METRIC,
		TRANSACTION_LONGEST_DURATION_METRIC,
		TIME_CURRENT_TRANSACTION_METRIC,
		TIME_CURRENT_REQUEST_METRIC,
        DISK_SPACE_METRIC,
        SESSION_STATS_METRIC,
        FREE_SPACE_METRIC,
        RESOURCES_UTILIZATION_METRIC,
        BACKUP_LATEST_METRIC
    }

    /// <summary>
    /// Тип результатов вычисления метрик.
    /// </summary>
    public enum MetricEvaluationResult
    {
        OK,
        NO_CONNECTION,
        TIMEOUT,
        NO_SSH_CONNECTION,
        INTERNAL_ERROR
    }

    /// <summary>
    /// Абстрактный класс метрик.
    /// </summary>
    public abstract class MetricBase : ICloneable
    {
        /// <summary>
        /// Тип метрики.
        /// </summary>
        public abstract MetricType Type { get; }

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        public DatabaseInfo Database { get; set; }

        /// <summary>
        /// Текущее значение.
        /// </summary>
        public object? CurrentValue { get; protected set; }

        /// <summary>
        /// Результат вычисления метрики.
        /// </summary>
        public MetricEvaluationResult Result { get; protected set; }

        /// <summary>
        /// Последнее время вычисление метрики.
        /// </summary>
        public DateTime LastTimeEvaluation { get; protected set; }

        /// <summary>
        /// Обновление значения метрики.
        /// </summary>
        /// <returns>Объект метрики.</returns>
        public virtual async Task Update()
        {
            Console.WriteLine($"Thread Id: {Thread.CurrentThread.ManagedThreadId}");

            var connection = new NpgsqlConnection(Database.ConnectionString);

            Result = MetricEvaluationResult.OK;
			LastTimeEvaluation = DateTime.Now;

			try
			{
				await connection.OpenAsync();

				CurrentValue = await UpdateInternal(connection);
			}
			catch (Exception ex)
			{
				HandleException(ex);
                CurrentValue = null;
			}
            finally
            {
				await connection.CloseAsync();
			}
        }

        /// <summary>
        /// Метод для клонирования пустого объекта метрики.
        /// </summary>
        /// <returns></returns>
        public abstract object Clone();

        /// <summary>
        /// Внутренняя реализация метода обновления значения метрики.
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        protected virtual Task<object?> UpdateInternal(NpgsqlConnection connection) 
        {
            return null;
        }

        /// <summary>
        /// Обработка исключения при вычисления метрики.
        /// </summary>
        /// <param name="exception"></param>
        protected void HandleException(Exception exception)
        {
            if (exception is not NpgsqlException)
            {
                Result = MetricEvaluationResult.INTERNAL_ERROR;
                Console.WriteLine($"Internal exception while estimating metric handled. Info: {exception.Message}");
                return;
            }

            var npgex = exception as NpgsqlException;

            if (npgex.SqlState is not null && npgex.SqlState.StartsWith("08") || npgex.Message.Contains("Failed to connect"))
            {
                Result = MetricEvaluationResult.NO_CONNECTION;
                Console.WriteLine($"Query connection exception handled.\n {npgex.Message}");
            }
            else
            {
				Result = MetricEvaluationResult.INTERNAL_ERROR;
				Console.WriteLine($"Unknonw query exception handled.\n {npgex.Message}\n Code: {npgex.SqlState ?? "null"}");
			}
        }
    }
}