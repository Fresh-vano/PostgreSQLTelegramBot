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
    public sealed class DatabaseDroppedProblem : ProblemBase
    {
        public override ProblemType Type { get; } = ProblemType.DATABASE_DROPED;

        public override List<MetricType> Metrics { get; protected set; } = new List<MetricType>{
            MetricType.STATE_METRIC
        };

		public DatabaseDroppedProblem(params ActionType[] actions) : base(actions) 
        { }

        public override bool IsReal(List<MetricBase> metrics)
        {
            var m = (StateMetric)metrics.FirstOrDefault();
            return m.Result == MetricEvaluationResult.NO_CONNECTION;
        }
    }
}

