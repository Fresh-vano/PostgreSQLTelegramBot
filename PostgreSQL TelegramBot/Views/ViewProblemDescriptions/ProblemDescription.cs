using System.Text;

namespace PostgreSQL_TelegramBot.Views.ViewProblemDescriptions
{
	public abstract class ProblemDescription
	{
		public abstract string Name { get; }

		public abstract string Description { get; }

		public abstract List<string> Links { get; }

		public override string ToString()
		{
			StringBuilder res = new StringBuilder();
			return res
				.Append($"Проблема: {Name}\n")
				.Append($"Описание: {Description}\n")
				.Append($"Подробную информацию можно получить по ссылкам: {string.Join(Environment.NewLine, Links)}\n")
				.ToString();
		}
	}
}