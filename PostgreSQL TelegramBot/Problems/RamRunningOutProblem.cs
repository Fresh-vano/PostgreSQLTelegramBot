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
	public sealed class RamRunningOutProblem : ProblemBase
	{
		public override ProblemType Type { get; } = ProblemType.RAM_RUNNING_OUT;

		public override List<MetricType> Metrics { get; protected set; } = new List<MetricType> {
			MetricType.RESOURCES_UTILIZATION_METRIC
		};

		public RamRunningOutProblem(params ActionType[] actions) : base(actions)
		{ }

		private const int kMaxRamUsagePercentage = 90;

		public override bool IsReal(List<MetricBase> metrics)
		{
			var met = (ResourcesUtilizationMetric)metrics.FirstOrDefault();
			var data = (ResourcesUtilization)met.CurrentValue;

			return data == null ? false : (data.RamUsagePercentage > kMaxRamUsagePercentage);
		}
	}
}
