using PostgreSQL_TelegramBot.Actions;

namespace PostgreSQL_TelegramBot.Views
{
	public static class ViewActions
	{
		public static Dictionary<ActionType, string> Descriptions = new Dictionary<ActionType, string>
		{
			{ ActionType.RELOAD,				"Перезагрузить базу данных"			},
			{ ActionType.LOGS_CLEANING,			"Очистить мусорные данные в памяти"	},
			{ ActionType.CLOSE_TRANSACTION ,	"Завершить все транзакции"			},
			{ ActionType.REBUILDING_INDEXES,	"Перестроить индексы всех таблиц"	},
			{ ActionType.MAKE_BACKUP,			"Выполнить резервное копирование"	},
			{ ActionType.CLOSE_ALL_REQUESTS,	"Закрыть все запросы"				}
		};
	}
}
