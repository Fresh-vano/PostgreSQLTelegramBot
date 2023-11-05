using System.Configuration;

namespace PostgreSQL_TelegramBot.Config
{
    public class BotDataBaseConfig : ConfigurationSection
    {
        [ConfigurationProperty("databases")]
        [ConfigurationCollection(typeof(BotDataBaseCollection))]
        public BotDataBaseCollection BotDataBases
        {
            get
            {
                return (BotDataBaseCollection)this["databases"];
            }
        }
    }
}