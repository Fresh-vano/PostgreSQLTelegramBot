namespace PostgreSQL_TelegramBot.Views.ViewProblemDescriptions
{
    public sealed class RamRunningOutDescription : ProblemDescription
    {
        public override string Name => "Мало оперативной памяти";

		public override string Description =>
	        "Оперативная память почти закончилась. Рекомендуется проверить открытые соединения и выполняемые запросы.";

		public override List<string> Links => new List<string>()
        {
            "https://stackoverflow.com/search?q=+Ram+Out+PostgreSQL"
        };
    }
}
