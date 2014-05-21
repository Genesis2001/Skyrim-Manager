// -----------------------------------------------------------------------------
//  <copyright file="ViewModel.cs" company="Arizona Western College">
//      Copyright (c) Arizona Western College.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------

namespace Skyrim.Manager.ViewModels
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using System.Reflection;
	using System.Runtime.CompilerServices;

	public abstract class ViewModel : INotifyPropertyChanged, INotifyPropertyChanging, IDataErrorInfo
	{
		protected ViewModel()
		{
			IsValid = true;
		}
		
		/// <summary>
		/// Gets a value indicating whether the model state is valid or contains errors.
		/// </summary>
		public bool IsValid { get; protected set; }

		#region Implementation of INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

		#region Implementation of INotifyPropertyChanging

		public event PropertyChangingEventHandler PropertyChanging;

		protected virtual void OnPropertyChanging([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanging;
			if (handler != null) handler(this, new PropertyChangingEventArgs(propertyName));
		}

		#endregion

		#region Implementation of IDataErrorInfo

		/// <summary>
		/// Gets the error message for the property with the given name.
		/// </summary>
		/// <returns>
		/// The error message for the property. The default is an empty string ("").
		/// </returns>
		/// <param name="propertyName">The name of the property whose error message to get. </param>
		public string this[string propertyName]
		{
			get
			{
				PropertyInfo propertyInfo = GetType().GetProperty(propertyName);

				var results = new List<ValidationResult>();
				var result = Validator.TryValidateProperty(
				                                        propertyInfo.GetValue(this, null),
														new ValidationContext(this, null, null)
														{
															MemberName = propertyName
														},
														results);

				if (!result)
				{
					var validationResult = results.First();
					IsValid = false;

					return validationResult.ErrorMessage;
				}

				return String.Empty;
			}
		}

		/// <summary>
		/// Gets an error message indicating what is wrong with this object.
		/// </summary>
		/// <returns>
		/// An error message indicating what is wrong with this object. The default is an empty string ("").
		/// </returns>
		public string Error { get; protected set; }

		#endregion
	}
}
