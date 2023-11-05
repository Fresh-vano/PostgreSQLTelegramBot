using System.Configuration;

namespace PostgreSQL_TelegramBot.Config
{
    public class BotDataBaseElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = false, DefaultValue = "Db")]
        public string Name
        {
            get
            {
                return (string)base["name"];
            }
            set
            {
                base["name"] = value;
            }
        }

		[ConfigurationProperty("owner", IsRequired = true)]
		public string Owner
		{
			get
			{
				return (string)base["owner"];
			}
			set
			{
				base["owner"] = value;
			}
		}

		[ConfigurationProperty("sshEnabled", IsRequired = false, DefaultValue = "1")]
        public string SshEnabled
        {
            get
            {
                return (string)base["sshEnabled"];
            }
            set
            {
                base["sshEnabled"] = value;
            }
        }

		[ConfigurationProperty("sshUser", IsRequired = true)]
        public string SshUser
        {
            get
            {
                return (string)base["sshUser"];
            }
            set
            {
                base["sshUser"] = value;
            }
        }


        [ConfigurationProperty("sshPassword", IsRequired = true)]
        public string SshPassword
        {
            get
            {
                return (string)base["sshPassword"];
            }
            set
            {
                base["sshPassword"] = value;
            }
        }

        [ConfigurationProperty("binDir", IsRequired = false, DefaultValue = "dir")]
        public string BinDir
        {
            get
            {
                return (string)base["binDir"];
            }
            set
            {
                base["binDir"] = value;
            }
        }

        [ConfigurationProperty("backupDir", IsRequired = false, DefaultValue = "dir")]
        public string BackupDir
        {
            get
            {
                return (string)base["backupDir"];
            }
            set
            {
                base["backupDir"] = value;
            }
        }
    }
}