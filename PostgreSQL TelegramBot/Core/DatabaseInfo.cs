
public class DatabaseInfo
{
	public string Owner { get; set; }

	public string DatabaseName { get; set; }
	public string UserName { get; set; }

	public string ConnectionString { get; set; }

	public bool SshEnabled { get; set; }

	public string SshHost { get; set; }
	public string SshUser { get; set; }
	public string SshPassword { get; set; }

	public string PGBinaryDirectory { get; set; }
	public string BackupDirectory { get; set; }
}