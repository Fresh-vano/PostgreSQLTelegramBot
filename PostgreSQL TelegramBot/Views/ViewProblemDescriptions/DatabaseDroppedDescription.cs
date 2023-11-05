namespace PostgreSQL_TelegramBot.Views.ViewProblemDescriptions
{
    public sealed class DatabaseDroppedDescription : ProblemDescription
    {
        public override string Name => "Остановка базы данных";

		public override string Description =>
            "База данных неожиданно прекратила свою работу.";

        public override List<string> Links => new List<string>()
        {
            "https://stackoverflow.com/search?q=drop+database",
            "https://stackoverflow.com/search?q=How+to+recover+a+dropped+database"
        };
    }
}
