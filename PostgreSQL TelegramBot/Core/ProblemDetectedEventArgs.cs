using PostgreSQL_TelegramBot.Problems;

namespace PostgreSQL_TelegramBot.Core
{
	public class ProblemDetectedEventArgs : EventArgs
	{
		public ProblemBase Problem { get; set; }

		public DatabaseInfo Database { get; set; }
	}
}