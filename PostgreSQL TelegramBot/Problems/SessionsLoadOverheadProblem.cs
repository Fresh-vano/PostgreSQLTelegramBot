using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostgreSQL_TelegramBot.Actions;
using PostgreSQL_TelegramBot.Metrics;
using PostgreSQL_TelegramBot.Problems;

namespace PostgreSQL_TelegramBot.Problems
{
	public sealed class SessionsLoadOverheadProblem : ProblemBase
	{
		public override ProblemType Type { get; } = ProblemType.SESSIONS_LOAD_OVERHEAD;

		public override List<MetricType> Metrics { get; protected set; } = new List<MetricType> {
			MetricType.SESSION_STATS_METRIC
		};

		public SessionsLoadOverheadProblem(params ActionType[] actions) : base(actions)
		{ }

		private const int kMaxSessionsCount = 1000;

		public override bool IsReal(List<MetricBase> metrics)
		{
			var met = (SessionStatsMetric)metrics[0];

			return (met.CurrentValue is null)
				? false
				: ((SessionStats)met.CurrentValue).Total > kMaxSessionsCount;
		}
	}
}
