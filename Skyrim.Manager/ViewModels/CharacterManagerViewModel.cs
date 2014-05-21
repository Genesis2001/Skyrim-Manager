// -----------------------------------------------------------------------------
//  <copyright file="CharacterManagerViewModel.cs" company="Arizona Western College">
//      Copyright (c) Arizona Western College.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace Skyrim.Manager.ViewModels
{
	using System.Collections.ObjectModel;
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

			Characters    = new ObservableCollection<string>(settings.Characters.List);
		}

		public ObservableCollection<string> Characters { get; private set; }

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
	}
}
