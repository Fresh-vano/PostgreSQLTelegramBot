using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostgreSQL_TelegramBot.Problems;
using PostgreSQL_TelegramBot.Metrics;
using PostgreSQL_TelegramBot.Actions;

namespace PostgreSQL_TelegramBot.Problems
{
    public sealed class NotEnoughDiskSpaceProblem : ProblemBase
    {
        public override ProblemType Type { get; } = ProblemType.NOT_ENOUGH_DISK_SPACE;

        public override List<MetricType> Metrics { get; protected set; } = new List<MetricType>{
            MetricType.RESOURCES_UTILIZATION_METRIC
        };

		public NotEnoughDiskSpaceProblem(params ActionType[] actions) : base(actions)
		{ }

        private const int kMaxDiskSpaceOccupancyPercentage = 85;

        public override bool IsReal(List<MetricBase> metrics)
        {
            var met = (ResourcesUtilizationMetric)metrics.FirstOrDefault();
            var data = (ResourcesUtilization)met.CurrentValue;

            return data == null ? false : (data.HardDiskOccupancyPercentage > kMaxDiskSpaceOccupancyPercentage);
        }
    }
}
