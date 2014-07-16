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

namespace Skyrim.Manager
{
	using System;
	using System.Diagnostics;
	using System.IO;
	using System.Reflection;
	using System.Windows;
	using Linq;
	using Models;
	using ViewModels;
	using Views;

    /// <summary>
	///     Interaction logic for path.xaml
	/// </summary>
	public partial class App
	{
		private MainViewModel context;
        private Settings config;

        #region Overrides of Application

		/// <summary>
		///     Raises the <see cref="E:System.Windows.Application.Startup" /> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event data.</param>
		protected override async void OnStartup(StartupEventArgs e)
		{
			if (MainWindow == null)
			{
				MainWindow = new MainWindow();
			}

			var asm  = Assembly.GetAssembly(typeof (App));
			var info = FileVersionInfo.GetVersionInfo(asm.Location);

		    string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		    string path    = Path.Combine(appData, info.ProductName, "Settings.json");

			if (!File.Exists(path))
			{
			    string temp = Path.GetDirectoryName(path);
                
                Debug.Assert(temp != null, "temp != null");

			    if (!Directory.Exists(temp))
                {
                    Directory.CreateDirectory(temp);
                }

				if (!asm.SaveResource("Settings.json", path))
				{
					throw new FileFormatException("Unable to extract default SettingsXml.xml from application manifest.");
				}
			}

		    config  = await Settings.Load(path);
            context = new MainViewModel(config);

			MainWindow.DataContext = context;
			MainWindow.Show();
		}

		/// <summary>
		///     Raises the <see cref="E:System.Windows.Application.Exit" /> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.Windows.ExitEventArgs" /> that contains the event data.</param>
		protected override async void OnExit(ExitEventArgs e)
		{
		    base.OnExit(e);

            await Settings.Save(config);
		}

		#endregion
	}
}
