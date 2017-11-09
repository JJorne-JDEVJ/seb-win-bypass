﻿/*
 * Copyright (c) 2017 ETH Zürich, Educational Development and Technology (LET)
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using SafeExamBrowser.Contracts.Behaviour;
using SafeExamBrowser.Contracts.Configuration;
using SafeExamBrowser.Contracts.Configuration.Settings;
using SafeExamBrowser.Contracts.I18n;
using SafeExamBrowser.Contracts.Logging;
using SafeExamBrowser.Contracts.SystemComponents;
using SafeExamBrowser.Contracts.UserInterface;
using SafeExamBrowser.Contracts.UserInterface.Taskbar;
using SafeExamBrowser.Core.Notifications;

namespace SafeExamBrowser.Core.Behaviour.Operations
{
	public class TaskbarOperation : IOperation
	{
		private ILogger logger;
		private INotificationController logController;
		private ITaskbarSettings settings;
		private ISystemComponent<ISystemKeyboardLayoutControl> keyboardLayout;
		private ISystemComponent<ISystemPowerSupplyControl> powerSupply;
		private ISystemInfo systemInfo;
		private ITaskbar taskbar;
		private IUserInterfaceFactory uiFactory;
		private IText text;

		public ISplashScreen SplashScreen { private get; set; }

		public TaskbarOperation(
			ILogger logger,
			ITaskbarSettings settings,
			ISystemComponent<ISystemKeyboardLayoutControl> keyboardLayout,
			ISystemComponent<ISystemPowerSupplyControl> powerSupply,
			ISystemInfo systemInfo,
			ITaskbar taskbar,
			IText text,
			IUserInterfaceFactory uiFactory)
		{
			this.logger = logger;
			this.settings = settings;
			this.keyboardLayout = keyboardLayout;
			this.powerSupply = powerSupply;
			this.systemInfo = systemInfo;
			this.taskbar = taskbar;
			this.text = text;
			this.uiFactory = uiFactory;
		}

		public void Perform()
		{
			logger.Info("Initializing taskbar...");
			SplashScreen.UpdateText(TextKey.SplashScreen_InitializeTaskbar);

			if (settings.AllowApplicationLog)
			{
				CreateLogNotification();
			}

			if (settings.AllowKeyboardLayout)
			{
				AddKeyboardLayoutControl();
			}

			if (systemInfo.HasBattery)
			{
				AddPowerSupplyControl();
			}
		}

		public void Revert()
		{
			logger.Info("Terminating taskbar...");
			SplashScreen.UpdateText(TextKey.SplashScreen_TerminateTaskbar);

			if (settings.AllowApplicationLog)
			{
				logController?.Terminate();
			}

			if (settings.AllowKeyboardLayout)
			{
				keyboardLayout.Terminate();
			}

			if (systemInfo.HasBattery)
			{
				powerSupply.Terminate();
			}
		}

		private void AddKeyboardLayoutControl()
		{
			var control = uiFactory.CreateKeyboardLayoutControl();

			keyboardLayout.Initialize(control);
			taskbar.AddSystemControl(control);
		}

		private void AddPowerSupplyControl()
		{
			var control = uiFactory.CreatePowerSupplyControl();

			powerSupply.Initialize(control);
			taskbar.AddSystemControl(control);
		}

		private void CreateLogNotification()
		{
			var logInfo = new LogNotificationInfo(text);
			var logNotification = uiFactory.CreateNotification(logInfo);

			logController = new LogNotificationController(logger, text, uiFactory);
			logController.RegisterNotification(logNotification);

			taskbar.AddNotification(logNotification);
		}
	}
}
