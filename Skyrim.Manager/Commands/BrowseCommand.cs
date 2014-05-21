// -----------------------------------------------------------------------------
//  <copyright file="BrowseCommand.cs" company="Arizona Western College">
//      Copyright (c) Arizona Western College.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace Skyrim.Manager.Commands
{
	using System;
	using System.Windows.Forms;
	using System.Windows.Input;

	public class BrowseCommand : ICommand
	{
		public event EventHandler<BrowseEventArgs> BrowseCompletedEvent;

		private void OnBrowseComplete(string selectedPath)
		{
			var handler = BrowseCompletedEvent;
			if (handler != null) handler(this, new BrowseEventArgs(selectedPath));
		}

		#region Implementation of ICommand

		/// <summary>
		/// Defines the method that determines whether the command can execute in its current state.
		/// </summary>
		/// <returns>
		/// true if this command can be executed; otherwise, false.
		/// </returns>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
		public bool CanExecute(object parameter)
		{
			return true;
		}

		/// <summary>
		/// Defines the method to be called when the command is invoked.
		/// </summary>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
		public void Execute(object parameter)
		{
			using (var dialog = new FolderBrowserDialog())
			{
				dialog.SelectedPath = parameter as string ?? "";
				dialog.ShowNewFolderButton = false;

				dialog.ShowDialog();
				OnBrowseComplete(dialog.SelectedPath);
			}
		}



		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		#endregion
	}

	public class BrowseEventArgs : EventArgs
	{
		public string SelectedPath { get; set; }

		public BrowseEventArgs(string selectedPath)
		{
			SelectedPath = selectedPath;
		}
	}
}