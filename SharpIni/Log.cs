// Log.cs //

using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace SharpIni
{
	/// <summary>
	///   A class for handling logging to a file.
	/// </summary>
	public class Logger
	{
		private readonly string DatetimeFormat;
		private readonly string Filename;

		/// <summary>
		///   Initialize a new instance of SimpleLogger class.
		/// </summary>
		/// <remarks>
		///   Log file will be created automatically if it does not yet exist,
		///   otherwise it can either append to the existing file or overwrite
		///   it completely. The default behaviour is to overwrite.
		/// </remarks>
		/// <param name="append">
		///   True to append to existing log file, False to overwrite and
		///	  create a new log file.
		/// </param>
		private Logger( bool append = false )
		{
			DatetimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
			Filename = Assembly.GetExecutingAssembly().GetName().Name + ".log";
			OutputToConsole = false;

			// Log file header line
			var logHeader = Filename + " is created.";

			if( !File.Exists( Filename ) )
				WriteLine( DateTime.Now.ToString( DatetimeFormat ) + " " + logHeader, false );
			else
				WriteLine( DateTime.Now.ToString( DatetimeFormat ) + " " + logHeader, append );
		}

		/// <summary>
		///   The single instance of the logger.
		/// </summary>
		public static Logger Instance
		{
			get
			{
				if( instance == null )
				{
					lock( syncRoot )
					{
						if( instance == null )
							instance = new Logger( true );
					}
				}

				return instance;
			}
		}

		/// <summary>
		///   If the logger should output to the console as well as the file.
		/// </summary>
		public bool OutputToConsole { get; set; }

		/// <summary>
		///   Log a debug message. The message will also output to the console
		///   if running in a debug configuration.
		/// </summary>
		/// <param name="text">
		///   Log message.
		/// </param>
		public void Debug( string text )
		{
			WriteFormattedLog( LogLevel.DEBUG, text );
		}

		/// <summary>
		///   Log an error message.
		/// </summary>
		/// <param name="text">
		///   Log message.
		/// </param>
		public void Error( string text )
		{
			WriteFormattedLog( LogLevel.ERROR, text );
		}

		/// <summary>
		///   Log a fatal error message.
		/// </summary>
		/// <param name="text">
		///   Log message.
		/// </param>
		public void Fatal( string text )
		{
			WriteFormattedLog( LogLevel.FATAL, text );
		}

		/// <summary>
		///   Log an info message
		/// </summary>
		/// <param name="text">
		///   Log message.
		/// </param>
		public void Info( string text )
		{
			WriteFormattedLog( LogLevel.INFO, text );
		}

		/// <summary>
		///   Log a waning message.
		/// </summary>
		/// <param name="text">
		///   Log message.
		/// </param>
		public void Warning( string text )
		{
			WriteFormattedLog( LogLevel.WARNING, text );
		}

		/// <summary>
		///   Format a log message based on log level.
		/// </summary>
		/// <param name="level">
		///   Log level.
		/// </param>
		/// <param name="text">
		///   Log message.
		/// </param>
		private void WriteFormattedLog( LogLevel level, string text )
		{
			string pretext;
			switch( level )
			{
				case LogLevel.INFO:    pretext = DateTime.Now.ToString( DatetimeFormat ) + " [INFO]    "; break;
				case LogLevel.DEBUG:   pretext = DateTime.Now.ToString( DatetimeFormat ) + " [DEBUG]   "; break;
				case LogLevel.WARNING: pretext = DateTime.Now.ToString( DatetimeFormat ) + " [WARNING] "; break;
				case LogLevel.ERROR:   pretext = DateTime.Now.ToString( DatetimeFormat ) + " [ERROR]   "; break;
				case LogLevel.FATAL:   pretext = DateTime.Now.ToString( DatetimeFormat ) + " [FATAL]   "; break;
				default:               pretext = DateTime.Now.ToString( DatetimeFormat ) + "           "; break;
			}

#		if DEBUG
			if( level == LogLevel.DEBUG && !OutputToConsole )
				Console.WriteLine( pretext + text );
#		endif

			if( OutputToConsole )
				Console.WriteLine( pretext + text );

			WriteLine( pretext + text );
		}

		/// <summary>
		///   Write a line of formatted log message into a log file.
		/// </summary>
		/// <param name="text">
		///   Formatted log message.
		/// </param>
		/// <param name="append">
		///   True to append, False to overwrite the file.
		/// </param>
		/// <exception>
		///   See <see cref="StreamWriter(string,bool,Encoding)"/> and
		///   <see cref="TextWriter.WriteLine(string)"/>.
		/// </exception>
		private void WriteLine( string text, bool append = true )
		{
			try
			{
				using( StreamWriter Writer = new StreamWriter( Filename, append, Encoding.UTF8 ) )
				{
					if( !string.IsNullOrWhiteSpace( text ) )
						Writer.WriteLine( text );
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		///   Supported log level
		/// </summary>
		[Flags]
		private enum LogLevel
		{
			INFO,
			DEBUG,
			WARNING,
			ERROR,
			FATAL
		}

		private static volatile Logger instance;
		private static readonly object syncRoot = new Object();
	}
}
