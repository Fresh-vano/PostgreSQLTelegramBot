using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostgreSQL_TelegramBot.Views.ViewProblemDescriptions
{
	public class NoBackupLongTimeDescription : ProblemDescription
	{
		public override string Name => "Отсутствие резервного копирования";

		public override string Description =>
			"Резервное копирование базы данных проходило слишком долгое время назад.";

		public override List<string> Links => new List<string>() { };
	}
}
