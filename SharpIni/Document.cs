// Document.cs //

using System;
using System.IO;
using System.Collections.Generic;

namespace SharpIni
{
	/// <summary>
	///   A document made up of sections.
	/// </summary>
	public class Document
	{
		/// <summary>
		///   Constructs the instance.
		/// </summary>
		public Document()
		{
			m_sections = new Dictionary<string, Section>();
		}

		/// <summary>
		///   The sections contained in the document indexed by their names.
		/// </summary>
		/// <param name="name">
		///   The name of the section to access.
		/// </param>
		/// <returns>
		///   The section with the given name if it exists, otherwise null.
		/// </returns>
		public Section this[ string name ]
		{
			get
			{
				if( !Contains( name ) )
					return null;

				return m_sections[ name ];
			}
			set
			{
				if( !Add( value, true ) )
					throw new ArgumentException( "Unable to add section to document." );
			}
		}

		/// <summary>
		///   If the document contains no sections.
		/// </summary>
		public bool Empty
		{
			get { return Count == 0; }
		}
		/// <summary>
		///   The amount of sections the document contains.
		/// </summary>
		public int Count
		{
			get { return m_sections.Count; }
		}

		/// <summary>
		///   Gets the sections that make up the document indexed by their names.
		/// </summary>
		public Dictionary<string, Section> Sections
		{
			get { return m_sections; }
		}

		/// <summary>
		///   If the document contains a section with the given name.
		/// </summary>
		/// <param name="section">
		///   The section name.
		/// </param>
		/// <returns>
		///   True if a section with the given name exists and false otherwise.
		/// </returns>
		public bool Contains( string section )
		{
			return m_sections.ContainsKey( section );
		}
		/// <summary>
		///   If the given section in the document contains a key with the 
		///   given name.
		/// </summary>
		/// <param name="section">
		///   The section name.
		/// </param>
		/// <param name="key">
		///   The key name.
		/// </param>
		/// <returns>
		///   True if a key exists with the given name in the section and false 
		///   otherwise.
		/// </returns>
		public bool Contains( string section, string key )
		{
			if( !Contains( section ) )
				return false;

			return m_sections[ section ].Contains( key );
		}

		/// <summary>
		///   Gets a section by name if it exists within the document.
		/// </summary>
		/// <param name="section">
		///   The name of the section.
		/// </param>
		/// <returns>
		///   The section with the given name if it exists, otherwise null.
		/// </returns>
		public Section Get( string section )
		{
			if( !Contains( section ) )
				return null;

			return m_sections[ section ];
		}
		/// <summary>
		///   Gets a key by name within a given section if it exists within
		///   the document.
		/// </summary>
		/// <param name="section">
		///   The name of the section.
		/// </param>
		/// <param name="key">
		///   The name of the key.
		/// </param>
		/// <returns>
		///   The key with the given name if it exists, otherwise null.
		/// </returns>
		public Key Get( string section, string key )
		{
			if( !Contains( section, key ) )
				return null;

			return m_sections[ section ].Get( key );
		}

		/// <summary>
		///   Add a section to the document.
		/// </summary>
		/// <param name="section">
		///   The section to add.
		/// </param>
		/// <param name="replace">
		///   Replace other section on name conflict.
		/// </param>
		/// <returns>
		///   True if the section was added to the document and false otherwise.
		/// </returns>
		public bool Add( Section section, bool replace = false )
		{
			if( section == null || !Naming.IsValid( section.Name ) )
				return false;

			if( Contains( section.Name ) )
			{
				if( !replace )
					return false;

				m_sections[ section.Name ] = section;
			}
			else
				m_sections.Add( section.Name, section );

			return true;
		}
		/// <summary>
		///   Adds a key to a section to the document.
		/// </summary>
		/// <param name="section">
		///   The section.
		/// </param>
		/// <param name="key">
		///   The key to add.
		/// </param>
		/// <param name="create">
		///   Create section if it does not exist?
		/// </param>
		/// <param name="replace">
		///   Replace other key on name conflict.
		/// </param>
		/// <returns>
		///   True if the key was added to the document section and false otherwise.
		/// </returns>
		public bool Add( string section, Key key, bool create = true, bool replace = false )
		{
			if( key == null || !Naming.IsValid( key.Name ) || !Naming.IsValid( section ) )
				return false;

			if( !Contains( section ) )
			{
				if( !create )
					return false;

				if( !Add( new Section( section ) ) )
					return false;
			}

			return m_sections[ section ].Add( key, replace );
		}

		/// <summary>
		///   Removes a section from the document.
		/// </summary>
		/// <param name="section">
		///   The name of the section to remove.
		/// </param>
		/// <returns>
		///   True if the section was removed, false otherwise.
		/// </returns>
		public bool Remove( string section )
		{
			return m_sections.Remove( section );
		}
		/// <summary>
		///   Removes a key from a section in the document.
		/// </summary>
		/// <param name="section">
		///   The name of the section.
		/// </param>
		/// <param name="key">
		///   The name of the key to remove.
		/// </param>
		/// <returns>
		///   True if the key was removed, false otherwise.
		/// </returns>
		public bool Remove( string section, string key )
		{
			if( !Contains( section ) )
				return false;

			return m_sections[ section ].Remove( key );
		}

		/// <summary>
		///   Clears the document, removing all sections.
		/// </summary>
		public void Clear()
		{
			m_sections.Clear();
		}

		/// <summary>
		///   Loads document data from a string.
		/// </summary>
		/// <param name="str">
		///   The string containing document data.
		/// </param>
		/// <returns>
		///   True on success or false on failure.
		/// </returns>
		public bool LoadFromString( string str )
		{
			if( string.IsNullOrWhiteSpace( str ) )
				return false;

			str = str.Replace( "\r\n", "\n" );
			str = str.Replace( '\r',   '\n' );

			return LoadFromLines( str.Split( '\n' ) );
		}
		/// <summary>
		///   Load document file data from an array of lines.
		/// </summary>
		/// <param name="lines">
		///   The file lines to parse.
		/// </param>
		/// <returns>
		///   True if successful and false otherwise.
		/// </returns>
		public bool LoadFromLines( string[] lines )
		{
			Section sec = null;

			foreach( string line in lines )
			{
				if( string.IsNullOrEmpty( line ) || ( line.Trim()[ 0 ] == '#' || line.Trim()[ 0 ] == ';' ) )
				{
					Logger.Instance.Debug( "Comment or empty line found, skipping line." );
					continue;
				}

				if( sec == null )
				{
					if( !IsValidSection( line ) )
					{
						Logger.Instance.Error( "Was expection a section but something else was found." );
						return false;
					}

					sec = new Section();

					if( !sec.LoadNameFromLine( line ) )
					{
						Logger.Instance.Error( "Unable to load section from line: " + line + "." );
						return false;
					}
				}
				else
				{
					if( IsValidKey( line ) )
					{
						if( !sec.LoadKeyFromLine( line ) )
						{
							Logger.Instance.Error( "Unable to load key from line: " + line + "." );
							return false;
						}
					}
					else if( IsValidSection( line ) )
					{
						if( sec.Empty )
						{
							Logger.Instance.Error( "Unable to add empty sections." );
							return false;
						}

						if( !Add( sec ) )
						{
							Logger.Instance.Error( "Unable to add completed section." );
							return false;
						}

						sec = new Section();

						if( !sec.LoadNameFromLine( line ) )
						{
							Logger.Instance.Error( "Unable to load section from line: " + line + "." );
							return false;
						}
					}
					else
					{
						Logger.Instance.Error( "Invalid line found." );
						return false;
					}
				}
			}

			if( !Add( sec ) )
			{
				Logger.Instance.Error( "Unable to add completed section." );
				return false;
			}

			return true;
		}
		/// <summary>
		///   Loads the document data from a file.
		/// </summary>
		/// <param name="path">
		///   The path of the file to load.
		/// </param>
		/// <returns>
		///   True if successful and false otherwise.
		/// </returns>
		public bool LoadFromFile( string path )
		{
			if( string.IsNullOrEmpty( path ) )
				return false;

			if( path.Length >= 3 && path[ 0 ] == '"' && path[ path.Length - 1 ] == '"' )
				path = path.Substring( 1, path.Length - 2 ).Trim();

			if( !File.Exists( path ) )
				return false;

			string[] lines = null;

			try
			{
				lines = File.ReadAllLines( path );
			}
			catch( Exception e )
			{
				Logger.Instance.Error( "Unable to read all lines of file. " + e.Message );
				return false;
			}

			return LoadFromLines( lines );
		}

		/// <summary>
		///   The file text data.
		/// </summary>
		/// <returns>
		///   The document text as it would be in file.
		/// </returns>
		public override string ToString()
		{
			string handle = string.Empty;
			
			foreach( var s in m_sections )
				handle += s.Value.ToString() + "\n";

			return handle;
		}

		/// <summary>
		///   Checks if the given line is a valid section header.
		/// </summary>
		/// <param name="line">
		///   The line to parse.
		/// </param>
		/// <returns>
		///   True if valid and false otherwise.
		/// </returns>
		static bool IsValidSection( string line )
		{
			return Section.CreateFromLine( line ) != null;
		}
		/// <summary>
		///   Checks if the given line is a valid key.
		/// </summary>
		/// <param name="line">
		///   The line to parse.
		/// </param>
		/// <returns>
		///   True if valid and false otherwise.
		/// </returns>
		static bool IsValidKey( string line )
		{
			return Key.CreateFromLine( line ) != null;
		}

		Dictionary<string, Section> m_sections;
	}
}
