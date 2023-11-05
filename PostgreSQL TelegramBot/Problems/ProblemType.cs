namespace PostgreSQL_TelegramBot.Problems
{
    public enum ProblemType
    {
        DATABASE_DROPED,
        NOT_ENOUGH_DISK_SPACE,
        RAM_RUNNING_OUT,
        CPU_OVER_LOADED,
        SESSIONS_LOAD_OVERHEAD,
        TRANSACTION_LOAD_OVERHEAD,
        EXCEEDED_AVR_REQUEST,
        INDEX_DEFRAGMENTATION,
        NO_BACKUP_LONG_TIME
    }
}