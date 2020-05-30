// Section.cs //

using System;
using System.Collections.Generic;

namespace SharpIni
{
	/// <summary>
	///		A section/header containing a name and the <see cref="Key"/>s it owns.
	/// </summary>
	public class Section
	{
		/// <summary>
		///	  Constructs the section with an optional name.
		/// </summary>
		/// <param name="name">
		///   The name of the section.
		/// </param>
		public Section( string name = null )
		{
			m_name = Naming.IsValid( name ) ? name : null;
			m_keys = new Dictionary<string, Key>();
		}

		/// <summary>
		///	  Allows access to sections' <see cref="Key"/>s by name.
		/// </summary>
		/// <remarks>
		///	  When assigning a <see cref="Key"/>, <see cref="Add(Key, bool)"/>
		///	  will be called with the replace parameter set to true.
		/// </remarks>
		/// <param name="name">
		///   The name of the <see cref="Key"/> to find.
		/// </param>
		/// <exception cref="ArgumentException">
		///	  Thrown when assigning if the call to <see cref="Add(Key, bool)"/>
		///	  fails or if the <paramref name="name"/> does not have the same 
		///	  name as the value.
		/// </exception>
		/// <returns>
		///	  Returns a key with the given name if it is contained by the 
		///	  instance.
		/// </returns>
		public Key this[ string name ]
		{
			get
			{
				if( !Contains( name ) )
					return null;

				return m_keys[ name ];
			}
			set
			{
				if( value.Name != name || !Add( value, true ) )
					throw new ArgumentException( "Unable to add key to section." );
			}
		}

		/// <summary>
		///   The name of the section.
		/// </summary>
		public string Name
		{
			get { return m_name; }
			set
			{
				if( !Naming.IsValid( value ) )
					throw new System.ArgumentException( value + " is not a valid name for an ini section." );

				m_name = value;
			}
		}

		/// <summary>
		///   Get a dictionary of the keys mapped by their string names.
		/// </summary>
		public Dictionary<string, Key> Keys
		{
			get { return m_keys; }
		}

		/// <summary>
		///   If the section contains no keys.
		/// </summary>
		public bool Empty
		{
			get { return Count == 0; }
		}
		/// <summary>
		///   The amount of keys in the section.
		/// </summary>
		public int Count
		{
			get { return m_keys.Count; }
		}

		/// <summary>
		///   If the section contains a key with the given name.
		/// </summary>
		/// <param name="name">
		///   The name to check.
		/// </param>
		/// <returns>
		///   True if the section contains a key with the given name and false
		///   otherwise.
		/// </returns>
		public bool Contains( string name )
		{
			return m_keys.ContainsKey( name );
		}

		/// <summary>
		///   Returns the key mapped to the given name and null if it does 
		///   not exist.
		/// </summary>
		/// <param name="name">
		///   The name to check.
		/// </param>
		/// <returns>
		///   The key with the given name or null if none exist.
		/// </returns>
		public Key Get( string name )
		{
			if( !Contains( name ) )
				return null;

			return m_keys[ name ];
		}

		/// <summary>
		///   Adds a key to the section.
		/// </summary>
		/// <param name="key">
		///   The key to add.
		/// </param>
		/// <param name="replace">
		///   If the given key should overwrite keys with the same name.
		/// </param>
		/// <returns>
		///   True if the key was sucessfully added to the section and false
		///   otherwise.
		/// </returns>
		public bool Add( Key key, bool replace = false )
		{
			if( key == null || !Naming.IsValid( key.Name ) )
				return false;

			if( Contains( key.Name ) )
			{
				if( !replace )
					return false;

				m_keys[ key.Name ] = key;
			}
			else
				m_keys.Add( key.Name, key );

			return true;
		}
		/// <summary>
		///   Removes a key by name.
		/// </summary>
		/// <param name="name">
		///   The name of the key to remove.
		/// </param>
		/// <returns>
		///   True if the key was removed successfully and false otherwise.
		/// </returns>
		public bool Remove( string name )
		{
			return m_keys.Remove( name );
		}
		/// <summary>
		///   Clears the section, removing all keys.
		/// </summary>
		public void Clear()
		{
			m_keys.Clear();
		}

		/// <summary>
		///   Loads the name from a section declaration line.
		/// </summary>
		/// <param name="line">
		///   The line to parse name from.
		/// </param>
		/// <returns>
		///   True if successful and false otherwise.
		/// </returns>
		public bool LoadNameFromLine( string line )
		{
			if( string.IsNullOrEmpty( line ) )
				return false;

			string handle = line.Trim();

			if( handle.Length < 3 || ( handle[ 0 ] != '[' && handle[ handle.Length - 1 ] != ']' ) )
				return false;

			try
			{
				Name = handle.Substring( 1, handle.Length - 2 ).Trim();
			}
			catch( System.Exception e )
			{
				Logger.Instance.Error( e.Message );
				return false;
			}

			return true;
		}
		/// <summary>
		///   <see cref="Key.LoadFromLine(string)"/>
		/// </summary>
		public bool LoadKeyFromLine( string line, bool replace = false )
		{
			Key key = new Key();

			if( !key.LoadFromLine( line ) )
				return false;

			return Add( key, replace );
		}

		/// <summary>
		///   The entire section as it would be in file as a string.
		/// </summary>
		/// <returns>
		///   The section as it would be in file as a string.
		/// </returns>
		public override string ToString()
		{
			string handle = "[" + Name + "]";

			foreach( var k in Keys )
				handle += "\n" + k.Key + " = " + k.Value;

			return handle;
		}

		/// <summary>
		///   Creates a new section and loads the name data from a line in a file.
		/// </summary>
		/// <param name="line">
		///   File line.
		/// </param>
		/// <returns>
		///   A section created from the file line on success or null on
		///   failure.
		/// </returns>
		public static Section CreateFromLine( string line )
		{
			Section sec = new Section();

			if( !sec.LoadNameFromLine( line ) )
				return null;

			return sec;
		}

		private string m_name;
		private Dictionary<string, Key> m_keys;
	}
}
