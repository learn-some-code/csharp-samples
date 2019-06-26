using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace LearnSomeCode
{
	public class Logger
	{
		// --------------------------------------------------------------------------------------------------
		// Variables
		ILogWriter writer;
		int interval;
		bool logging;
		bool verbose;
		bool done;

		Task writingTask;
		ConcurrentQueue<LogEntry> logQueue;
		CancellationTokenSource delayToken;
		// --------------------------------------------------------------------------------------------------



		// --------------------------------------------------------------------------------------------------
		public Logger(ILogWriter logWriter, int writeInterval,
				bool enableLogging = true, bool verboseMode = false)
		{
			writer = logWriter;
			interval = writeInterval;
			logging = enableLogging;
			verbose = verboseMode;
			logQueue = new ConcurrentQueue<LogEntry>();
			delayToken = new CancellationTokenSource();
		}
		// --------------------------------------------------------------------------------------------------



		// --------------------------------------------------------------------------------------------------
		public void Start()
		{
			if (!logging) { return; }

			done = false;
			writingTask = Task.Run(() => WriteLoop());
		}

		public void Stop()
		{
			if (!logging) { return; }

			done = true;
			delayToken.Cancel();
			writingTask.Wait();
			WriteOut();
		}
		// --------------------------------------------------------------------------------------------------



		// --------------------------------------------------------------------------------------------------
		public void Record(string message, [CallerMemberName] string sourceMethod = "",
						[CallerLineNumber] int sourceLineNum = 0)
		{
			if (!logging) { return; }
			if (!verbose)
			{
				sourceMethod = "";
				sourceLineNum = 0;
			}

			// Add log entry to queue
			logQueue.Enqueue(new LogEntry(sourceMethod, sourceLineNum, message));
		}
		// --------------------------------------------------------------------------------------------------



		// --------------------------------------------------------------------------------------------------
		private async Task WriteLoop()
		{
			while (!done)
			{
				try
				{
					// Delay before writing
					await Task.Delay(interval, delayToken.Token);
					WriteOut();
				}
				catch { }
			}
		}

		private void WriteOut()
		{
			// Make sure there are log entries in the queue
			if (!logQueue.TryPeek(out LogEntry temp)) { return; }

			// Collect all log entries from queue
			List<LogEntry> logEntries = new List<LogEntry>();
			while (logQueue.TryDequeue(out LogEntry entry))
			{
				logEntries.Add(entry);
			}

			// Send entries to writer
			writer.Write(logEntries.ToArray());
		}
		// --------------------------------------------------------------------------------------------------
	}
}
