// -----------------------------------------------------------------------------
//  <copyright file="Character.cs" company="Arizona Western College">
//      Copyright (c) Arizona Western College.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace Skyrim.Manager.Models
{
	using System.Collections.Generic;
	using Newtonsoft.Json;

	[JsonObject]
	public class Character
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Object"/> class.
		/// </summary>
		public Character(string name)
		{
			Name = name;
			Saves = new List<string>();
		}

		[JsonProperty("name")]
		public string Name { get; internal set; }

		[JsonProperty("saves")]
		public List<string> Saves { get; set; }

		#region Overrides of Object

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		public override string ToString()
		{
			return Name;
		}

		#endregion
	}
}
