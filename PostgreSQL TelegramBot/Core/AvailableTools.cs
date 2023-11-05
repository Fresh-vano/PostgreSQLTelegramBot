using PostgreSQL_TelegramBot.Actions;
using PostgreSQL_TelegramBot.Metrics;
using PostgreSQL_TelegramBot.Problems;

namespace PostgreSQL_TelegramBot.Core
{
	public static class AvailableTools
	{
		public static readonly List<MetricBase> Metrics = new List<MetricBase>()
		{
			new DiskSpaceMetric(),
			new ResourcesUtilizationMetric(),
			new SessionStatsMetric(),
			new StateLastTimeMetric(),
			new StateMetric(),
			new BackupLatestMetric(),
		};

		public static readonly List<ProblemBase> Problems = new List<ProblemBase>()
		{
			new RamRunningOutProblem(
				ActionType.RELOAD, 
				ActionType.CLOSE_TRANSACTION, 
				ActionType.REBUILDING_INDEXES
			),
			new SessionsLoadOverheadProblem(
				ActionType.RELOAD, 
				ActionType.CLOSE_TRANSACTION
			),
			new DatabaseDroppedProblem(
				ActionType.RELOAD, 
				ActionType.LOGS_CLEANING
			),
			new NotEnoughDiskSpaceProblem(
				ActionType.RELOAD, 
				ActionType.LOGS_CLEANING, 
				ActionType.CLOSE_TRANSACTION,
				ActionType.REBUILDING_INDEXES
			),
			new NoBackupLongTimeProblem(
				ActionType.MAKE_BACKUP
			)
		};
	}
}