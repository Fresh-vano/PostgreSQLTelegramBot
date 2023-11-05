namespace PostgreSQL_TelegramBot.Actions
{
    public enum ActionType
    {
        RELOAD,
        LOGS_CLEANING,
        CLOSE_TRANSACTION,
        REBUILDING_INDEXES,
        MAKE_BACKUP,
        CLOSE_ALL_REQUESTS
    }
}