// -----------------------------------------------------------------------------
//  <copyright file="MainViewModel.cs" company="Zack Loveless">
//      Copyright (c) Zack Loveless.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------
namespace Skyrim.Manager.ViewModels
{
	using System;
	using System.IO;
	using System.Security;
	using System.Windows;
	using System.Windows.Input;
	using Commands;
	using Microsoft.Win32;
	using Models;
	using Views;

	public class MainViewModel : ViewModel
	{
		private readonly Settings settings;

		public MainViewModel(Settings settings)
		{
			this.settings    = settings;

			if (!settings.Installed)
			{
				InitializeDefaults();
			}

			CharacterManager = new CharacterManagerViewModel(settings);

			BrowseDataPath         = new BrowseCommand();
			BrowseInstallPath      = new BrowseCommand();
			ExitCommand            = new ActionCommand(x => Application.Current.Shutdown(), x => true);
			ShowAboutWindowCommand = new ShowWindowCommand<AboutWindow>(owner: Application.Current.MainWindow, properties: new {ShowInTaskBar = false});

			BrowseDataPath.BrowseCompletedEvent    += selectedPath => GameDataPath = selectedPath;
			BrowseInstallPath.BrowseCompletedEvent += selectedPath => InstallPath = selectedPath;
		}

		private void GameDataPathChanging(string path)
		{
			if (!GameDataPath.Equals(path, StringComparison.OrdinalIgnoreCase))
			{
				var result = MessageBox.Show(Application.Current.MainWindow, "You changed the data path where saves will be saved. Would you like to reload saved games from this new directory?", "Directory changed", MessageBoxButton.YesNo, MessageBoxImage.Information);
				if (result == MessageBoxResult.Yes)
				{
//					CharacterManager.LoadCharacterListFromDisk();
				}
			}
		}

		#region Commands

		public ICommand ExitCommand { get; private set; }

		public BrowseCommand BrowseInstallPath { get; private set; }

		public BrowseCommand BrowseDataPath { get; private set; }

		public ICommand ShowAboutWindowCommand { get; private set; }

		#endregion

		#region Properties

		public CharacterManagerViewModel CharacterManager { get; private set; }

		public string GameDataPath
		{
			get { return settings.Game.GameDataPath; }
			set
			{
				OnPropertyChanging();
				GameDataPathChanging(value);
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

		private void InitializeDefaults()
		{
			if (settings.Installed) return;

			string docsdir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			GameDataPath = Path.Combine(docsdir, @"My Games\Skyrim");

			RegistryKey steam = null;
			try
			{
				steam = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam");
				if (steam != null)
				{
					var path = steam.GetValue("SteamPath").ToString();

					path = Path.GetFullPath(Path.Combine(path, @"steamapps\common\skyrim"));
					InstallPath = new Uri(path).LocalPath;
				}
			}
			catch (SecurityException e)
			{
				// Report error to user.
				IsValid = false;
			}
			finally
			{
				if (steam != null)
				{
					steam.Close();
				}
			}

			settings.Installed = true;
		}
	}
}
