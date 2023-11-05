using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostgreSQL_TelegramBot.Metrics;

namespace PostgreSQL_TelegramBot.Views
{
	public static class ViewMetrics
	{
		public static Dictionary<MetricType, string> Names = new Dictionary<MetricType, string>
		{
			{ MetricType.STATE_METRIC,							"Состояние БД"							},
			{ MetricType.STATE_LAST_TIME_METRIC,				"Состояние БД"							},
			{ MetricType.TRANSACTION_LONGEST_DURATION_METRIC,	"Самая долгая транзакция"				},
			{ MetricType.TIME_CURRENT_TRANSACTION_METRIC,       "Время текущей транзакции"				},
			{ MetricType.TIME_CURRENT_REQUEST_METRIC,			"Время текущего запроса"				},
			{ MetricType.DISK_SPACE_METRIC,						"Размер БД"								},
			{ MetricType.SESSION_STATS_METRIC,					"Состояние БД"							},
			{ MetricType.FREE_SPACE_METRIC,						"Свободное место"						},
			{ MetricType.BACKUP_LATEST_METRIC,                  "Последняя дата резервного копирования" },
			{ MetricType.RESOURCES_UTILIZATION_METRIC,			"Утилизация ресурсов"					}
		};

		public static Dictionary<MetricEvaluationResult, string> Results = new Dictionary<MetricEvaluationResult, string>
		{
			{ MetricEvaluationResult.OK,                        "ОК"							},
			{ MetricEvaluationResult.TIMEOUT,					"Таймаут"						},
			{ MetricEvaluationResult.NO_CONNECTION,				"Отсутствие соединения"			},
			{ MetricEvaluationResult.NO_SSH_CONNECTION,			"Отсутствие SSH-соединения"     },
			{ MetricEvaluationResult.INTERNAL_ERROR,            "Неизвестная ошибка"			},
		};
	}
}
