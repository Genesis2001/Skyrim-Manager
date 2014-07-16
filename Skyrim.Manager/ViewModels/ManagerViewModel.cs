// ----------------------------------------------------------------
// Skyrim Manager
// Copyright (c) 2013. Zack Loveless.
// 
// Original author(s) for this source file: Zack Loveless
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// ----------------------------------------------------------------

namespace Skyrim.Manager.ViewModels
{
	using System;
	using System.Windows;
	using System.Windows.Forms;
	using System.Windows.Input;
	using Commands;
	using Models;
	using Views;

    public class ManagerViewModel : ObservableObject
	{
		private readonly Settings settingsXml;

        public ManagerViewModel(Settings settingsXml, Action<Object> shutdownMethod)
		{
			this.settingsXml = settingsXml;

			ExitCommand = new ActionCommand(shutdownMethod, o => true);
			DataPathBrowseCommand = new ActionCommand(o => SettingsXml.Paths.GameDataPath = Browse(SettingsXml.Paths.GameDataPath), o => true);
			InstallPathBrowseCommand = new ActionCommand(o => SettingsXml.Paths.InstallPath = Browse(SettingsXml.Paths.InstallPath), o => true);
			SaveCommand = new ActionCommand(o => ConfigViewModel.Save(settingsXml, settingsXml.FileName), o => true);
			ShowAboutDialogCommand = new ActionCommand(ShowAboutDialog, o => true);
		}

		public ICommand DataPathBrowseCommand { get; private set; }
		public ICommand ExitCommand { get; private set; }
		public ICommand InstallPathBrowseCommand { get; private set; }
		public ICommand SaveCommand { get; private set; }
		public ICommand ShowAboutDialogCommand { get; private set; }

        public Settings SettingsXml
		{
			get { return settingsXml; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		public void ShowAboutDialog(object obj)
		{
			var owner = obj as Window;
			if (owner != null)
			{
				var dialog = new AboutWindow {ShowInTaskbar = false, Owner = owner};

				dialog.ShowDialog();
			}
		}

		/// <summary>
		///     Browses for a folder on the file system and returns the path as a <see cref="T:System.String" />
		/// </summary>
		/// <param name="defaultPath"></param>
		/// <returns></returns>
		public string Browse(string defaultPath = "")
		{
			using (var dialog = new FolderBrowserDialog())
			{
				dialog.RootFolder = Environment.SpecialFolder.MyComputer;
				dialog.SelectedPath = defaultPath;

				return (dialog.ShowDialog() == DialogResult.OK) ? dialog.SelectedPath : defaultPath;
			}
		}
	}
}
