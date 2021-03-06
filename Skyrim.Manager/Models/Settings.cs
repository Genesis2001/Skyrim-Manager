﻿// -----------------------------------------------------------------------------
//  <copyright file="Settings.cs" company="Zack Loveless">
//      Copyright (c) Zack Loveless.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------
namespace Skyrim.Manager.Models
{
	using System;
	using System.IO;
	using System.Reflection;
	using System.Text;
	using System.Threading.Tasks;
	using Newtonsoft.Json;

	[JsonObject]
	public class Settings
	{
		private string path;

		#region Properties

		[JsonProperty("Installed")]
		public bool Installed { get; set; }

		[JsonProperty("Characters")]
		public CharacterList Characters { get; set; }

		[JsonProperty("Game")]
		public GameInfo Game { get; set; }

		#endregion

		#region Factory methods

		public static async Task<Settings> Load(string settingsFile)
		{
			if (!File.Exists(settingsFile))
			{
				var asm = Assembly.GetAssembly(typeof (Settings));
				asm.SaveResource("Settings.json", settingsFile);
			}

			using (Stream s = new FileStream(settingsFile, FileMode.OpenOrCreate))
			using (StreamReader reader = new StreamReader(s, new UTF8Encoding(false)))
			{
				string json = await reader.ReadToEndAsync();

				var result = JsonConvert.DeserializeObject<Settings>(json);
				result.path = settingsFile;
				return result;
			}
		}

		public static Task Save(Settings settings, string path = null)
		{
			string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
			if (String.IsNullOrEmpty(path))
			{
				path = settings.path;
			}

			using (var s = new FileStream(path, FileMode.Truncate))
			using (var writer = new StreamWriter(s, new UTF8Encoding(false)))
			{
				return writer.WriteAsync(json);
			}
		}

		#endregion

		#region Nested type: CharacterList

		[JsonObject]
		public class CharacterList
		{
			[JsonProperty("characters")]
			public Character[] List { get; set; }

			[JsonProperty("selected")]
			public string Selected { get; set; }
		}

		#endregion

		#region Nested type: GameInfo

		[JsonObject]
		public class GameInfo
		{
			public string InstallPath { get; set; }

			public string GameDataPath { get; set; }

			public string GameExe { get; set; }

			public string CommandLine { get; set; }
		}

		#endregion
	}
}
