// cs1587-21.cs: XML comment is not placed on a valid language element
// Line: 11
// Compiler options: -doc:dummy.xml -warnaserror -warn:2

using System;

namespace TopNS
{
	class Foo
	{
		void  /// incorrect
		FooBar (string foo)
		{
		}
	}

}
