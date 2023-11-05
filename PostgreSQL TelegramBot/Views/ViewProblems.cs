using PostgreSQL_TelegramBot.Metrics;
using PostgreSQL_TelegramBot.Problems;
using PostgreSQL_TelegramBot.Views.ViewProblemDescriptions;

namespace PostgreSQL_TelegramBot.Views
{
    public static class ViewProblems
    {
        public static Dictionary<ProblemType, ProblemDescription> Descriptions = new Dictionary<ProblemType, ProblemDescription>
        {
            { ProblemType.RAM_RUNNING_OUT,          new RamRunningOutDescription()          },
            { ProblemType.SESSIONS_LOAD_OVERHEAD,   new SessionsLoadOverheadDescription()   },
            { ProblemType.DATABASE_DROPED,          new DatabaseDroppedDescription()        },
            { ProblemType.NOT_ENOUGH_DISK_SPACE,    new NotEnoughDiskSpaceDescription()     },
			{ ProblemType.NO_BACKUP_LONG_TIME,      new NoBackupLongTimeDescription()       },
		};
    }
}
