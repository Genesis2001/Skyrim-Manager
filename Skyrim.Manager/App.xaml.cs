// -----------------------------------------------------------------------------
//  <copyright file="App.xaml.cs" company="Zack Loveless">
//      Copyright (c) Zack Loveless.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------
namespace Skyrim.Manager
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Windows;
	using Models;
	using ViewModels;
	using Views;

	public partial class App
	{
		private MainViewModel viewModel;
		private Settings settings;

		#region Overrides of Application

		protected override async void OnStartup(StartupEventArgs e)
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			path        = Path.Combine(path, @"My Games\Skyrim\Settings.json");
			
			settings         = await Settings.Load(path);
			viewModel = new MainViewModel(settings);

			if (MainWindow == null)
			{
				MainWindow = new MainWindow();
			}

			MainWindow.DataContext = viewModel;
			MainWindow.Show();
		}
		
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Application.Exit"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.Windows.ExitEventArgs"/> that contains the event data.</param>
		protected override async void OnExit(ExitEventArgs e)
		{
			settings.Characters.List = viewModel.CharacterManager.Characters.ToArray();

			await Settings.Save(settings);
		}

		#endregion
	}
}
