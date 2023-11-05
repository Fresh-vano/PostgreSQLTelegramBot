using System;
using System.Configuration;

namespace PostgreSQL_TelegramBot.Config
{
    public class BotDataBaseCollection : ConfigurationElementCollection
    {
        public BotDataBaseElement this[int index]
        {
            get
            {
                return (BotDataBaseElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value);
            }
        }

        public new BotDataBaseElement this[string key]
        {
            get
            {
                return (BotDataBaseElement)BaseGet(key);
            }
            set
            {
                if (BaseGet(key) != null)
                    BaseRemoveAt(BaseIndexOf(BaseGet(key)));

                BaseAdd(value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new BotDataBaseElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((BotDataBaseElement)element).Name;
        }
    }
}