﻿/*
 * Copyright (c) 2018 ETH Zürich, Educational Development and Technology (LET)
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using SafeExamBrowser.Contracts.Communication.Events;

namespace SafeExamBrowser.Contracts.Communication.Hosts
{
	/// <summary>
	/// Defines the functionality of the communication host for the client application component.
	/// </summary>
	public interface IClientHost : ICommunicationHost
	{
		/// <summary>
		/// The startup token used for initial authentication.
		/// </summary>
		Guid StartupToken { set; }

		/// <summary>
		/// Event fired when the runtime requests a password input from the user.
		/// </summary>
		event CommunicationEventHandler<PasswordRequestEventArgs> PasswordRequested;

		/// <summary>
		/// Event fired when the runtime denied a reconfiguration request.
		/// </summary>
		event CommunicationEventHandler<ReconfigurationEventArgs> ReconfigurationDenied;

		/// <summary>
		/// Event fired when the runtime commands the client to shutdown.
		/// </summary>
		event CommunicationEventHandler Shutdown;
	}
}