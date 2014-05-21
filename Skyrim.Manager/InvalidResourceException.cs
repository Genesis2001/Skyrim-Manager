// -----------------------------------------------------------------------------
//  <copyright file="InvalidResourceException.cs" company="Arizona Western College">
//      Copyright (c) Arizona Western College.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace Skyrim.Manager
{
	using System;

	public class InvalidResourceException : Exception
	{
		public InvalidResourceException(string resource) : base(String.Format("The resource {0} could not be extracted from the specified assembly.", resource))
		{
			Resource = resource;
		}

		public string Resource { get; set; }
	}
}
