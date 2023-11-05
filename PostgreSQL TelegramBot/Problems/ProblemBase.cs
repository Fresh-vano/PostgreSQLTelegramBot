using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostgreSQL_TelegramBot.Metrics;
using PostgreSQL_TelegramBot.Actions;

namespace PostgreSQL_TelegramBot.Problems
{
    public abstract class ProblemBase
    {
        public abstract ProblemType Type { get; }

        public abstract List<MetricType> Metrics { get; protected set; }

        public List<ActionType> Actions { get; protected set; }

        public abstract bool IsReal(List<MetricBase> metrics);

        public ProblemBase(params ActionType[] actions)
        {
            Actions = new List<ActionType> { };
            
            foreach (var action in actions)
            {
                Actions.Add(action);
            }
        }
    }
}