// -----------------------------------------------------------------------------
//  <copyright file="ActionCommand.cs" company="Arizona Western College">
//      Copyright (c) Arizona Western College.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace Skyrim.Manager.Commands
{
	using System;
	using System.Windows.Input;

	public class ActionCommand : ICommand
	{
		private readonly Action<object> execute;
		private readonly Func<object, bool> executeCondition;

		public ActionCommand(Action<object> execute, Func<object, bool> executeCondition)
		{
			this.execute = execute;
			this.executeCondition = executeCondition;
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
			return executeCondition != null && executeCondition(parameter);
		}

		/// <summary>
		/// Defines the method to be called when the command is invoked.
		/// </summary>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
		public void Execute(object parameter)
		{
			if (execute != null) execute(parameter);
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		#endregion
	}
}
