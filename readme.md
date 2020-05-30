#SharpIni
A simple C# .ini file parser.

##Usage
###Document Loading
The `Document` class represents an ini file and contains methods for loading
the file data either from file, a list of strings, or even a string containing
the entire file data.
~~~~
using SharpIni;
...
Document doc = new Document();

// Loading from file "config.ini".
if( !doc.LoadFromFile( "config.ini" ) )
	return -1;

// Loading from a list of strings.
string[] lines = new string[] { /* File data broken up into lines. */ };
if( !doc.LoadFromLines( lines ) )
	return -1;

// Loading from a string.
string file_data = "<file data here>";
if( !doc.LoadFromString( file_data ) )
	return -1;
...
~~~~

###Sections
The `Section` class is a list of associated key-value pairs represented by the
`Key` class. Keys belonging to a section can be accessed with the `Section[string]`
accessor, indexed by name.

~~~~
; config.ini

[Settings]
width=800
height=600
...
~~~~

~~~~
// Load in config.ini as document 'doc'.
...
Section settings = doc[ "Settings" ];

if( settings == null ) // Settings section does not exist.
	return -2;

Key width_key = settings[ "width" ];

if( width_key == null ) // Settings section does not contain width key.
	return -3;

Key height_key = doc[ "Settings" ]?[ "height" ];

if( height_key == null ) // Settings section does not contain height key.
	return -4;
...
~~~~

###Keys
The `Key` class, as previously mentioned, is a key-value pair that represents
a key in an ini file. It is identified by `Key.Name` and its value can be
accessed with `Key.Value`. It also has some convenience functions for converting
its value to different standard types.

~~~~
// Load in config.ini as document 'doc'.
...
Section settings = doc[ "Settings" ];

if( settings == null )
	return -2;

Key width_key = settings[ "width" ];

if( width_key == null )
	return -3;

// Get width key as an int.
int width = width_key.ToInt32();
// Get height key as an unsigned long int.
int width = width_key.ToUInt64();
...
~~~~

###Saving to File
All SharpIni classes return their in-file representations as a string on
calling `ToString()`, so it can be as simple as:

~~~~
using System.IO;
using SharpIni;

...
Document doc;
// Populate document.
...

try
{
	File.WriteAllText( "config.ini", doc.ToString() );
}
catch
{
	// Unable to write doc to file.
	return -4;
}

// Write succeeded.
~~~~
