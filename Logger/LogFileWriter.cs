using System;
using System.IO;
using System.Text;

namespace LearnSomeCode
{
	public class LogFileWriter : ILogWriter
	{
		// --------------------------------------------------------------------------------------------------
		private string filename;
		private string extension;
		private string path;
		// --------------------------------------------------------------------------------------------------



		// --------------------------------------------------------------------------------------------------
		public LogFileWriter(string file)
		{
			try
			{
				filename = Path.GetFileNameWithoutExtension(file);
				extension = Path.GetExtension(file);
				path = Path.GetDirectoryName(file);
				if (path == "") { path = "."; }

				Directory.CreateDirectory(path);
			}
			catch
			{
				// Exception
				throw;
			}
		}
		// --------------------------------------------------------------------------------------------------



		// --------------------------------------------------------------------------------------------------
		public void Write(LogEntry[] logEntries)
		{
			try
			{
				StringBuilder sb = new StringBuilder();
				foreach (LogEntry entry in logEntries)
				{
					sb.Append("[");
					sb.Append(entry.Timestamp);
					sb.Append("] : ");

					if (entry.Method.Length > 0)
					{
						sb.Append(entry.Method);
						sb.Append(" @ ");
						sb.Append(entry.LineNumber);
						sb.Append("    \t");
					}

					sb.Append(entry.Message);
					sb.Append("\n");

					// Example:
					// [Timestamp] : Method @ Line		message
				}

				File.AppendAllText(GenerateFile(), sb.ToString());
			}
			catch
			{
				// Exception
				throw;
			}
		}

		private string GenerateFile()
		{
			string timestamp = DateTime.Now.ToString("yyyy'-'MM'-'dd");
			return (path + "/" + filename + "-" + timestamp + extension);
		}
		// --------------------------------------------------------------------------------------------------
	}
}
