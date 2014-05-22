// -----------------------------------------------------------------------------
//  <copyright file="ShowWindowCommand.cs" company="Arizona Western College">
//      Copyright (c) Arizona Western College.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------
namespace Skyrim.Manager.Commands
{
	using System;
	using System.Windows;
	using System.Windows.Input;

	public class ShowWindowCommand<T> : ICommand where T : Window, new()
	{
		private readonly Window owner;
		private readonly object context;
		private readonly object properties;
		private readonly Func<object, bool> condition;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Object"/> class.
		/// </summary>
		public ShowWindowCommand(Window owner = null, object context = null, object properties = null, Func<object, bool> condition = null)
		{
			this.owner      = owner;
			this.context    = context;
			this.properties = properties;
			this.condition  = condition;
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
			return condition == null || condition(parameter);
		}

		/// <summary>
		/// Defines the method to be called when the command is invoked.
		/// </summary>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
		public void Execute(object parameter)
		{
			Window w      = (Window)Activator.CreateInstance(typeof (T));
			w.Owner       = owner;
			w.DataContext = context;

			// TODO: Extract "properties" into this window ("w") instance.

			w.Show();
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		#endregion
	}
}