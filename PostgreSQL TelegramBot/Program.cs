using System;
using System.Configuration;
using Npgsql;
using PostgreSQL_TelegramBot.Config;
using PostgreSQL_TelegramBot.Core;
using PostgreSQL_TelegramBot.Context;

class Program
{
    static List<DatabaseInfo> GetDatabasesList()
    {
		var dbs = new List<DatabaseInfo>();
		var tgBotCfg = (BotDataBaseConfig)ConfigurationManager.GetSection("TgBot");

		foreach (BotDataBaseElement instance in tgBotCfg.BotDataBases)
		{
			var key = instance.Name;
			var conn = ConfigurationManager.ConnectionStrings[key].ConnectionString;

			var builder = new NpgsqlConnection(conn);

			DatabaseInfo db = new DatabaseInfo()
			{
				Owner = instance.Owner,
				DatabaseName = builder.Database,
				UserName = builder.UserName,
				ConnectionString = conn,
				SshEnabled = Convert.ToBoolean(int.Parse(instance.SshEnabled)),
				SshHost = builder.Host,
				SshUser = instance.SshUser,
				SshPassword = instance.SshPassword,
				PGBinaryDirectory = instance.BinDir,
				BackupDirectory = instance.BackupDir
			};

			dbs.Add(db);
		}

		return dbs;
	}

	static void Main(string[] args)
    {
		var dbString	= ConfigurationManager.AppSettings["MongoDb"];
        var token		= ConfigurationManager.AppSettings["BotToken"];
		var databases	= GetDatabasesList();

		Storage.Initialize(dbString);
		TelegramBot bot = new TelegramBot(token, databases);

        bot.Start();

        Console.WriteLine("Bot started...");
        Console.ReadLine();
    }
}
