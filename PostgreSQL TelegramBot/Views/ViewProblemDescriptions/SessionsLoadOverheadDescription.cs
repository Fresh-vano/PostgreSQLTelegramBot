namespace PostgreSQL_TelegramBot.Views.ViewProblemDescriptions
{
    public sealed class SessionsLoadOverheadDescription : ProblemDescription
    {
        public override string Name => "Превышено количество сессий";

		public override string Description =>
	        "Количество одновременных сессий слишком велико. Завершите ненужные сессии.";

		public override List<string> Links => new List<string>()
        {
            "https://stackoverflow.com/questions/30778015/how-to-increase-the-max-connections-in-postgres",
            "https://stackoverflow.com/search?q=connections+postgresql"
        };
    }
}
