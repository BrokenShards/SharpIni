// Program.cs //

using System;
using SharpIni;

namespace SharpIniTest
{
	class Program
	{
		static int Main( string[] args )
		{
			Console.WriteLine( "Enter ini file location." );

			string input = Console.ReadLine();

			Document doc = new Document();

			if( !doc.LoadFromFile( input.Trim() ) )
			{
				Console.WriteLine( "Reading ini file failed." );
				Console.ReadLine();
				return -1;
			}

			Console.WriteLine( "Reading ini file succeeded. Printing now.\n" );

			foreach( var sec in doc.Sections )
			{
				Console.WriteLine( "Section Name: " + sec.Value.Name );

				foreach( var key in sec.Value.Keys )
					Console.WriteLine( "\tKey Name: " + key.Value.Name + " Key Value: " + key.Value.Value );
			}

			Console.ReadLine();
			return 0;
		}
	}
}
