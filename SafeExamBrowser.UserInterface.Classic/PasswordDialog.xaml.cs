﻿/*
 * Copyright (c) 2018 ETH Zürich, Educational Development and Technology (LET)
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Windows;
using SafeExamBrowser.Contracts.I18n;
using SafeExamBrowser.Contracts.UserInterface.Windows;

namespace SafeExamBrowser.UserInterface.Classic
{
	public partial class PasswordDialog : Window, IPasswordDialog
	{
		private IText text;
		private WindowClosingEventHandler closing;

		event WindowClosingEventHandler IWindow.Closing
		{
			add { closing += value; }
			remove { closing -= value; }
		}

		public PasswordDialog(string message, string title, IText text)
		{
			this.text = text;

			InitializeComponent();
			InitializePasswordDialog(message, title);
		}

		public void BringToForeground()
		{
			Dispatcher.Invoke(Activate);
		}

		public IPasswordDialogResult Show(IWindow parent = null)
		{
			return Dispatcher.Invoke(() =>
			{
				var result = new PasswordDialogResult { Success = false };

				if (parent is Window)
				{
					Owner = parent as Window;
					WindowStartupLocation = WindowStartupLocation.CenterOwner;
				}

				if (ShowDialog() is true)
				{
					result.Password = Password.Password;
					result.Success = true;
				}

				return result;
			});
		}

		private void InitializePasswordDialog(string message, string title)
		{
			Message.Text = message;
			Title = title;
			WindowStartupLocation = WindowStartupLocation.CenterScreen;

			CancelButton.Content = text.Get(TextKey.PasswordDialog_Cancel);
			CancelButton.Click += CancelButton_Click;

			ConfirmButton.Content = text.Get(TextKey.PasswordDialog_Confirm);
			ConfirmButton.Click += ConfirmButton_Click;

			Closing += (o, args) => closing?.Invoke();
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}

		private void ConfirmButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			Close();
		}

		private class PasswordDialogResult : IPasswordDialogResult
		{
			public string Password { get; set; }
			public bool Success { get; set; }
		}
	}
}