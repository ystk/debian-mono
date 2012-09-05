//
// ServiceCredentialsElement.cs
//
// Author:
//	Atsushi Enomoto <atsushi@ximian.com>
//
// Copyright (C) 2006 Novell, Inc.  http://www.novell.com
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.IdentityModel.Tokens;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Diagnostics;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.MsmqIntegration;
using System.ServiceModel.PeerResolvers;
using System.ServiceModel.Security;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace System.ServiceModel.Configuration
{
	public sealed class ServiceCredentialsElement
		 : BehaviorExtensionElement
	{
		public ServiceCredentialsElement () {
		}


		// Properties

		public override Type BehaviorType {
			get { return typeof (ServiceCredentials); }
		}

		[ConfigurationProperty ("clientCertificate",
			 Options = ConfigurationPropertyOptions.None)]
		public X509InitiatorCertificateServiceElement ClientCertificate {
			get { return (X509InitiatorCertificateServiceElement) base ["clientCertificate"]; }
		}

		[ConfigurationProperty ("issuedTokenAuthentication",
			 Options = ConfigurationPropertyOptions.None)]
		public IssuedTokenServiceElement IssuedTokenAuthentication {
			get { return (IssuedTokenServiceElement) base ["issuedTokenAuthentication"]; }
		}

		[ConfigurationProperty ("peer",
			 Options = ConfigurationPropertyOptions.None)]
		public PeerCredentialElement Peer {
			get { return (PeerCredentialElement) base ["peer"]; }
		}

		protected override ConfigurationPropertyCollection Properties {
			get { return base.Properties; }
		}

		[ConfigurationProperty ("secureConversationAuthentication",
			 Options = ConfigurationPropertyOptions.None)]
		public SecureConversationServiceElement SecureConversationAuthentication {
			get { return (SecureConversationServiceElement) base ["secureConversationAuthentication"]; }
		}

		[ConfigurationProperty ("serviceCertificate",
			 Options = ConfigurationPropertyOptions.None)]
		public X509RecipientCertificateServiceElement ServiceCertificate {
			get { return (X509RecipientCertificateServiceElement) base ["serviceCertificate"]; }
		}

		[StringValidator (MinLength = 0,
			MaxLength = int.MaxValue,
			 InvalidCharacters = null)]
		[ConfigurationProperty ("type",
			 Options = ConfigurationPropertyOptions.None,
			 DefaultValue = "")]
		public string Type {
			get { return (string) base ["type"]; }
			set { base ["type"] = value; }
		}

		[ConfigurationProperty ("userNameAuthentication",
			 Options = ConfigurationPropertyOptions.None)]
		public UserNameServiceElement UserNameAuthentication {
			get { return (UserNameServiceElement) base ["userNameAuthentication"]; }
		}

		[ConfigurationProperty ("windowsAuthentication",
			 Options = ConfigurationPropertyOptions.None)]
		public WindowsServiceElement WindowsAuthentication {
			get { return (WindowsServiceElement) base ["windowsAuthentication"]; }
		}

		[MonoTODO]
		protected internal override object CreateBehavior () {
			throw new NotImplementedException ();
		}

	}

}
