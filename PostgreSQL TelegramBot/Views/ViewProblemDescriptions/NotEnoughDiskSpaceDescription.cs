using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostgreSQL_TelegramBot.Views.ViewProblemDescriptions
{
    public sealed class NotEnoughDiskSpaceDescription : ProblemDescription
    {
        public override string Name => "Заканчивается свободное место на диске";

		public override string Description =>
	        "Свободное место на диске заканчивается, рекомендуется очистить диск во избежании потери данных.";

		public override List<string> Links => new List<string>()
        {
            "https://stackoverflow.com/search?q=clear+log+postgresql",
            "https://stackoverflow.com/search?q=Not+Enough+Disk+Space+PostgreSQL"
        };
    }
}
