// Naming.cs //

namespace SharpIni
{
	/// <summary>
	///   Contains all methods involved with naming.
	/// </summary>
	public static class Naming
	{
		/// <summary>
		///   Checks if a string is a valid variable name.
		/// </summary>
		/// <param name="name">
		///   The string to check.
		/// </param>
		/// <returns>
		///   True if the string is a valid name and false otherwise.
		///	</returns>
		public static bool IsValid( string name )
		{
			if( string.IsNullOrEmpty( name ) || ( !char.IsLetter( name[ 0 ] ) && name[ 0 ] != '_' ) )
				return false;

			for( int i = 1; i < name.Length; i++ )
				if( !char.IsLetterOrDigit( name[ i ] ) && name[ i ] != '_' )
					return false;

			return true;
		}
	}
}
