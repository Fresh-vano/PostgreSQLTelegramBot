using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostgreSQL_TelegramBot.Actions;
using PostgreSQL_TelegramBot.Metrics;

namespace PostgreSQL_TelegramBot.Problems
{
	public sealed class NoBackupLongTimeProblem : ProblemBase
	{
		public override ProblemType Type { get; } = ProblemType.NO_BACKUP_LONG_TIME;

		public override List<MetricType> Metrics { get; protected set; } = new List<MetricType>{
			MetricType.BACKUP_LATEST_METRIC
		};

		public NoBackupLongTimeProblem(params ActionType[] actions) : base(actions)
		{ }

		public override bool IsReal(List<MetricBase> metrics)
		{
			var m = (BackupLatestMetric)metrics[0];

			if (m.CurrentValue is null)
			{
				return false;
			}

			var lastBackupDate = (DateTime)m.CurrentValue;

			return GetBackupDeadline(lastBackupDate).Ticks < DateTime.Now.Ticks;
		}

		private DateTime GetBackupDeadline(DateTime last)
		{
			return last.AddDays(7);
		}
	}
}
