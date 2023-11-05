using Npgsql;
using Renci.SshNet;

namespace PostgreSQL_TelegramBot.Metrics
{
	public class ResourcesUtilization
	{
		public int HardDiskOccupancyPercentage { get; set; } = 0;
		public int RamUsagePercentage { get; set; } = 0;
		public int CpuUsagePercentage { get; set; } = 0;

		public override string ToString()
		{
			return $"{CpuUsagePercentage}%, {RamUsagePercentage}%, {HardDiskOccupancyPercentage}%";
		}
	}

	public sealed class ResourcesUtilizationMetric : MetricBase
	{
		public override MetricType Type { get; } = MetricType.RESOURCES_UTILIZATION_METRIC;

		private const string kCpuRetrievalScript = "mpstat | awk 'NR > 3 {print 100 - $NF}'";
		private const string kRamRetrievalScript = "free | awk 'NR==2 {print int($3 / $2 * 100)}'";
		private const string kHardDiskRetrievalScript = "df -h | awk 'NR==2 {print int(($3 / ($3 + $4) * 100))}'";

		public override ResourcesUtilizationMetric Clone()
		{
			return new ResourcesUtilizationMetric();
		}

		public override async Task Update()
		{
			if (!Database.SshEnabled)
			{
				Result = MetricEvaluationResult.NO_SSH_CONNECTION;
				CurrentValue = null;
				return;
			}

			var res = new ResourcesUtilization();

			using (var client = new SshClient(Database.SshHost, Database.SshUser, Database.SshPassword))
			{
				client.Connect();

				var cpu = client.RunCommand(kCpuRetrievalScript);
				var ram = client.RunCommand(kRamRetrievalScript);
				var hd = client.RunCommand(kHardDiskRetrievalScript);

				res.CpuUsagePercentage = Convert.ToInt32(cpu.Result);
				res.RamUsagePercentage = Convert.ToInt32(ram.Result);
				res.HardDiskOccupancyPercentage = Convert.ToInt32(hd.Result);
			}

			CurrentValue = res;
		}
	}
}