// Key.cs //

using System;

namespace SharpIni
{
	/// <summary>
	///	  A key/property containing a name and value.
	/// </summary>
	public class Key
	{
		/// <summary>
		///	  Constructs the instance with an optional name and value.
		///	</summary>
		/// <remarks>
		///	  <para>
		///	    If the given name is invalid, <see cref="Name"/> will be set to 
		///	    <see cref="string.Empty"/> (See <see cref="Naming.IsValid(string)"/>).
		///	  </para>
		///	  <para>
		///	    If the given value is null, <see cref="Value"/> will be set to 
		///	    <see cref="string.Empty"/>.
		///	  </para>
		///	</remarks>
		/// <param name="name">
		///   The name of the key.
		/// </param>
		/// <param name="value">
		///   The value of the key.
		/// </param>
		public Key( string name = null, string value = null )
		{
			m_name = Naming.IsValid( name ) ? name : string.Empty;
			Value  = value ?? string.Empty;
		}

		/// <summary> 
		///   The name of the key.
		///	</summary>
		/// <exception cref="ArgumentException">
		///   If assigned to an invalid name (See <see cref="Naming.IsValid(string)"/>).
		///	</exception>
		public string Name
		{
			get { return m_name; }
			set
			{
				if( !Naming.IsValid( value ) )
					throw new ArgumentException( value + " is not a valid name for an ini key." );

				m_name = value;
			}
		}
		/// <summary>
		///   The value of the key.
		///	</summary>
		public string Value { get; set; }


		/// <summary>
		///   Parses a line from a file and assigns the data to the instance.
		///	</summary>
		/// <param name="line">
		///   The line to parse.
		/// </param>
		/// <returns>
		///   True if the string was parsed as a key and the name is valid, 
		///   otherwise false.
		///	</returns>
		public bool LoadFromLine( string line )
		{
			if( string.IsNullOrEmpty( line ) || !line.Contains( "=" ) )
				return false;

			string handle = line.Trim();

			int eq = handle.IndexOf( "=" );

			try
			{
				Name = handle.Substring( 0, eq ).Trim();
			}
			catch( Exception e )
			{
				Logger.Instance.Error( e.Message );
				return false;
			}

			if( handle.Length > eq + 1 )
				Value = handle.Substring( eq + 1 ).Trim();
			else
				Value = string.Empty;

			return true;
		}


		/// <summary>
		///   Returns <see cref="Value"/> parsed as a 64-bit signed integer.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		///   If <see cref="Value"/> is null.
		///	</exception>
		/// <exception cref="FormatException">
		///   If <see cref="Value"/> is formatted incorrectly for the type.
		///	</exception>
		/// <exception cref="OverflowException">
		///   If the parsed value of <see cref="Value"/> is out of range for 
		///   the type.
		///	</exception>
		public Int64 ToInt64()
		{
			try
			{
				return Int64.Parse( Value );
			}
			catch
			{
				throw;
			}
		}
		/// <summary>
		///   Returns <see cref="Value"/> parsed as a 64-bit unsigned integer.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		///   If <see cref="Value"/> is null.
		///	</exception>
		/// <exception cref="FormatException">
		///   If <see cref="Value"/> is formatted incorrectly for the type.
		///	</exception>
		/// <exception cref="OverflowException">
		///   If the parsed value of <see cref="Value"/> is out of range for 
		///   the type.
		///	</exception>
		public UInt64 ToUInt64()
		{
			try
			{
				return UInt64.Parse( Value );
			}
			catch
			{
				throw;
			}
		}
		/// <summary>
		///   Returns <see cref="Value"/> parsed as a 32-bit signed integer.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		///   If <see cref="Value"/> is null.
		///	</exception>
		/// <exception cref="FormatException">
		///   If <see cref="Value"/> is formatted incorrectly for the type.
		///	</exception>
		/// <exception cref="OverflowException">
		///   If the parsed value of <see cref="Value"/> is out of range for 
		///   the type.
		///	</exception>
		public Int32 ToInt32()
		{
			try
			{
				return Int32.Parse( Value );
			}
			catch
			{
				throw;
			}
		}
		/// <summary>
		///   Returns <see cref="Value"/> parsed as a 32-bit unsigned integer.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		///   If <see cref="Value"/> is null.
		///	</exception>
		/// <exception cref="FormatException">
		///   If <see cref="Value"/> is formatted incorrectly for the type.
		///	</exception>
		/// <exception cref="OverflowException">
		///   If the parsed value of <see cref="Value"/> is out of range for 
		///   the type.
		///	</exception>
		public UInt32 ToUInt32()
		{
			try
			{
				return UInt32.Parse( Value );
			}
			catch
			{
				throw;
			}
		}
		/// <summary>
		///   Returns <see cref="Value"/> parsed as a 16-bit signed integer.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		///   If <see cref="Value"/> is null.
		///	</exception>
		/// <exception cref="FormatException">
		///   If <see cref="Value"/> is formatted incorrectly for the type.
		///	</exception>
		/// <exception cref="OverflowException">
		///   If the parsed value of <see cref="Value"/> is out of range for
		///   the type.
		///	</exception>
		public Int16 ToInt16()
		{
			try
			{
				return Int16.Parse( Value );
			}
			catch
			{
				throw;
			}
		}
		/// <summary>
		///   Returns <see cref="Value"/> parsed as a 16-bit unsigned integer.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		///   If <see cref="Value"/> is null.
		///	</exception>
		/// <exception cref="FormatException">
		///   If <see cref="Value"/> is formatted incorrectly for the type.
		///	</exception>
		/// <exception cref="OverflowException">
		///   If the parsed value of <see cref="Value"/> is out of range for 
		///   the type.
		///	</exception>
		public UInt16 ToUInt16()
		{
			try
			{
				return UInt16.Parse( Value );
			}
			catch
			{
				throw;
			}
		}
		/// <summary>
		///   Returns <see cref="Value"/> parsed as an 8-bit signed integer.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		///   If <see cref="Value"/> is null.
		///	</exception>
		/// <exception cref="FormatException">
		///   If <see cref="Value"/> is formatted incorrectly for the type.
		///	</exception>
		/// <exception cref="OverflowException">
		///   If the parsed value of <see cref="Value"/> is out of range for 
		///   the type.
		///	</exception>
		public sbyte ToInt8()
		{			
			try
			{
				return sbyte.Parse( Value );
			}
			catch
			{
				throw;
			}
		}
		/// <summary>
		///   Returns <see cref="Value"/> parsed as an 8-bit unsigned integer.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		///   If <see cref="Value"/> is null.
		///	</exception>
		/// <exception cref="FormatException">
		///   If <see cref="Value"/> is formatted incorrectly for the type.
		///	</exception>
		/// <exception cref="OverflowException">
		///   If the parsed value of <see cref="Value"/> is out of range for
		///   the type.
		///	</exception>
		public byte ToUInt8()
		{
			try
			{
				return byte.Parse( Value );
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		///   Returns <see cref="Value"/> parsed as a single-precision floating
		///   point number.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		///   If <see cref="Value"/> is null.
		///	</exception>
		/// <exception cref="FormatException">
		///   If <see cref="Value"/> is formatted incorrectly for the type.
		///	</exception>
		/// <exception cref="OverflowException">
		///   If the parsed value of <see cref="Value"/> is out of range for 
		///   the type.
		///	</exception>
		public float ToFloat()
		{
			try
			{
				return float.Parse( Value );
			}
			catch
			{
				throw;
			}
		}
		/// <summary>
		///   Returns <see cref="Value"/> parsed as a double-precision floating
		///   point number.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		///   If <see cref="Value"/> is null.
		///	</exception>
		/// <exception cref="FormatException">
		///   If <see cref="Value"/> is formatted incorrectly for the type.
		///	</exception>
		/// <exception cref="OverflowException">
		///   If the parsed value of <see cref="Value"/> is out of range for 
		///   the type.
		///	</exception>
		public double ToDouble()
		{
			try
			{
				return double.Parse( Value );
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		///   Returns <see cref="Value"/> parsed as a boolean value.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		///   If <see cref="Value"/> is null.
		///	</exception>
		/// <exception cref="FormatException">
		///   If <see cref="Value"/> is formatted incorrectly for the type.
		///	</exception>
		public bool ToBool()
		{
			if( Value == null )
				throw new ArgumentNullException( "The value is null and thus cannot be parsed as " +
												 "a boolean value." );

			if( Value.ToUpper() == "TRUE" )
				return true;
			else if( Value.ToUpper() == "FALSE" )
				return false;
			else
			{
				bool val;

				try
				{
					val = ( ToInt32() != 0 );
				}
				catch
				{
					throw new FormatException( "The value cannot be parsed as an integer to be " +
											   "cast to boolean value." );
				}

				return val;
			}

			throw new FormatException( "The value cannot be parsed as a boolean value." );
		}

		/// <summary>
		///   Returns the key as it would appear in a file.
		/// </summary>
		/// <returns>
		///   A string representing the key as it would appear in a file.
		///   (<see cref="Name"/> = <see cref="Value"/>).
		///	</returns>
		public override string ToString()
		{
			return Name + " = " + ( Value == null ? Value : string.Empty );
		}

		/// <summary>
		///   Parses a line from a file and assigns the data to a new instance.
		///	</summary>
		/// <param name="line">
		///   The line to parse.
		/// </param>
		/// <returns>
		///   The newly created instance, null on failure.
		///	</returns>
		public static Key CreateFromLine( string line )
		{
			Key key = new Key();

			if( !key.LoadFromLine( line ) )
				return null;

			return key;
		}

		private string m_name;
	}
}
