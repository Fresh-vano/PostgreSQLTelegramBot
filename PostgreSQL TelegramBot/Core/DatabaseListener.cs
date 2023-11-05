using Npgsql;
using System.Collections.Generic;
using PostgreSQL_TelegramBot.Metrics;

namespace PostgreSQL_TelegramBot.Core
{
	public class DatabaseListener
	{
		public DatabaseInfo Database { get; }

		public Dictionary<MetricType, MetricBase> Metrics { get; }

		private Task _task;

		private CancellationTokenSource _cancellationTokenSource;

		public DatabaseListener(DatabaseInfo database)
		{
			Database = database;
			Metrics = new Dictionary<MetricType, MetricBase>();

			foreach (var metric in AvailableTools.Metrics)
			{
				var m = (MetricBase)metric.Clone();
				m.Database = Database;
				Metrics.Add(m.Type, m);
			}
		}

		public async Task Run()
		{
			_cancellationTokenSource = new CancellationTokenSource();
			_task = new Task(async () =>
			{
				while (!_cancellationTokenSource.IsCancellationRequested)
				{
					await MainLoop();
					Thread.Sleep(15000);
				}
			}, _cancellationTokenSource.Token);
			_task.Start();
		}

		public async Task Stop()
		{
			_cancellationTokenSource.Cancel();
		}

		public event EventHandler<ProblemDetectedEventArgs> ProblemDetected;

		private async Task MainLoop()
		{
			var metrics = Metrics;

			foreach (var metric in metrics)
			{
				await metric.Value.Update();
			}

			foreach (var problem in AvailableTools.Problems)
			{
				List<MetricBase> problemMetrics = problem.Metrics.Select(x => metrics[x]).ToList();
				if (problem.IsReal(problemMetrics))
				{
					var args = new ProblemDetectedEventArgs()
					{
						Problem = problem,
						Database = Database,	
					};

					ProblemDetected?.Invoke(this, args);
				}
			}
		}
	}
}