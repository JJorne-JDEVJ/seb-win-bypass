﻿/*
 * Copyright (c) 2018 ETH Zürich, Educational Development and Technology (LET)
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using SafeExamBrowser.Contracts.Behaviour.Operations;
using SafeExamBrowser.Contracts.UserInterface;

namespace SafeExamBrowser.Core.Behaviour.Operations
{
	public class DelayedInitializationOperation : IOperation
	{
		private Func<IOperation> initialize;
		private IOperation operation;

		public IProgressIndicator ProgressIndicator { get; set; }

		public DelayedInitializationOperation(Func<IOperation> initialize)
		{
			this.initialize = initialize;
		}

		public OperationResult Perform()
		{
			operation = initialize.Invoke();
			operation.ProgressIndicator = ProgressIndicator;

			return operation.Perform();
		}

		public OperationResult Repeat()
		{
			operation.ProgressIndicator = ProgressIndicator;

			return operation.Repeat();
		}

		public void Revert()
		{
			operation.ProgressIndicator = ProgressIndicator;
			operation.Revert();
		}
	}
}