// -----------------------------------------------------------------------------
//  <copyright file="MainViewModel.cs" company="Arizona Western College">
//      Copyright (c) Arizona Western College.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace Skyrim.Manager.ViewModels
{
	using System;
	using System.Collections.ObjectModel;
	using System.Collections.Specialized;
	using System.Linq;
	using System.Windows;
	using System.Windows.Input;
	using Commands;
	using Models;

	public class MainViewModel : ViewModel
	{
		private readonly Settings settings;

		public MainViewModel(Settings settings)
		{
			this.settings    = settings;
			CharacterManager = new CharacterManagerViewModel(settings);

			BrowseDataPath    = new BrowseCommand();
			BrowseInstallPath = new BrowseCommand();
			ExitCommand       = new ActionCommand(x => Application.Current.Shutdown(), x => true);

			BrowseDataPath.BrowseCompletedEvent    += selectedPath => GameDataPath = selectedPath;
			BrowseInstallPath.BrowseCompletedEvent += selectedPath => InstallPath = selectedPath;
		}

		#region Commands

		public ICommand ExitCommand { get; private set; }

		public BrowseCommand BrowseInstallPath { get; private set; }

		public BrowseCommand BrowseDataPath { get; private set; }

		#endregion

		#region Properties

		public CharacterManagerViewModel CharacterManager { get; private set; }

		public string GameDataPath
		{
			get { return settings.Game.GameDataPath; }
			set
			{
				OnPropertyChanging();
				settings.Game.GameDataPath = value;
				OnPropertyChanged();
			}
		}

		public string InstallPath
		{
			get { return settings.Game.InstallPath; }
			set
			{
				OnPropertyChanging();
				settings.Game.InstallPath = value;
				OnPropertyChanged();
			}
		}

		#endregion

	}
}
