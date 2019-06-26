using System;

namespace LearnSomeCode
{
	public struct LogEntry
	{
		public readonly string Timestamp;
		public readonly string Method;
		public readonly int LineNumber;
		public readonly string Message;


		public LogEntry(string method, int lineNumber, string message)
		{
			Timestamp = DateTime.Now.ToString("MMM dd, yy - HH:mm:ss:fff");
			Method = method;
			LineNumber = lineNumber;
			Message = message;
		}
	}
}
