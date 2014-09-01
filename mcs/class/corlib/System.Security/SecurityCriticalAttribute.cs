//
// System.Security.SecurityCriticalAttribute implementation
//
// Author:
//	Sebastien Pouliot  <sebastien@ximian.com>
//
// Copyright (C) 2005, 2009 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

namespace System.Security {

#if NET_2_1

	[AttributeUsage (AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Struct |
		AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property |
		AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate,
		AllowMultiple=false, Inherited=false)]
	public sealed class SecurityCriticalAttribute : Attribute {

		public SecurityCriticalAttribute ()
		{
		}
	}

#else
	[MonoTODO ("Only supported by the runtime when CoreCLR is enabled")]
#if NET_4_0
	[AttributeUsage (AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct |
		AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method |
		AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Delegate,
		AllowMultiple=false, Inherited=false)]
#else
	[AttributeUsage (AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Struct |
		AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event |
		AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Delegate,
		AllowMultiple=false, Inherited=false)]
#endif
	public sealed class SecurityCriticalAttribute : Attribute {

		private SecurityCriticalScope _scope;

		public SecurityCriticalAttribute ()
			: base ()
		{
			_scope = SecurityCriticalScope.Explicit;
		}

		public SecurityCriticalAttribute (SecurityCriticalScope scope)
			: base ()
		{
			switch (scope) {
			case SecurityCriticalScope.Everything:
				_scope = SecurityCriticalScope.Everything;
				break;
			default:
				// that includes all bad enums values
				_scope = SecurityCriticalScope.Explicit;
				break;
			}
		}

#if NET_4_0
		[Obsolete]
#endif
		public SecurityCriticalScope Scope {
			get { return _scope; }
		}
	}
#endif
}
