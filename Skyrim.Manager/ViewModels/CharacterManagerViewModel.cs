// -----------------------------------------------------------------------------
//  <copyright file="CharacterManagerViewModel.cs" company="Arizona Western College">
//      Copyright (c) Arizona Western College.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace Skyrim.Manager.ViewModels
{
	using System;
	using System.Collections.ObjectModel;
	using System.IO;
	using System.Linq;
	using System.Text;
	using Models;

	public class CharacterManagerViewModel : ViewModel
	{
		private readonly Settings settings;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Object"/> class.
		/// </summary>
		public CharacterManagerViewModel(Settings settings)
		{
			this.settings = settings;

			var characters = settings.Characters.List ?? Enumerable.Empty<Character>();
			Characters = new ObservableCollection<Character>(characters);
//			if (!settings.Installed)
//			{
//				LoadCharacterListFromDisk();
//			}
		}

		public ObservableCollection<Character> Characters { get; private set; }

		public string Current
		{
			get { return settings.Characters.Selected; }
			set
			{
				OnPropertyChanging();
				settings.Characters.Selected = value;
				OnPropertyChanged();
			}
		}

		private static string GetCharacterName(string savedGame)
		{
			// Credit: Ben "aca20031" Buzbee
			// http://www.benbuzbee.com
			var data = File.ReadAllBytes(savedGame);
			var nameLength = data[26] << 8 | data[25];

			return Encoding.UTF8.GetString(data, 27, nameLength);
		}

		public void LoadCharacterListFromDisk()
		{
			// Full backup of current saves.
			// Backup();

			var cDirectory = Path.Combine(settings.Game.GameDataPath, "Saves");
			if (!Directory.Exists(cDirectory))
			{
				Directory.CreateDirectory(cDirectory);
			}

			var saves = Directory.GetFiles(cDirectory, "*.ess");
			foreach (var item in saves)
			{
				string file = Path.GetFileName(item);
				string cName = GetCharacterName(item);

				// Search for it in the collection
				Character c = Characters.SingleOrDefault(x => x.Name.Equals(cName, StringComparison.OrdinalIgnoreCase));
				if (c == null)
				{
					c = new Character(cName);
					Characters.Add(c);
				}

				if (file != null && !c.Saves.Any(x => x.EndsWith(file, StringComparison.OrdinalIgnoreCase)))
				{
					c.Saves.Add(item);
				}
			}
		}
	}
}
