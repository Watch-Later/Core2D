﻿// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Collections.Generic;

namespace Dock.Model
{
    /// <summary>
    /// View contract.
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Gets view title.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets view context.
        /// </summary>
        object Context { get; }

        /// <summary>
        /// Gets or sets windows.
        /// </summary>
        IList<IViewsWindow> Windows { get; set; }

        /// <summary>
        /// Show windows.
        /// </summary>
        void ShowWindows();

        /// <summary>
        /// Close windows.
        /// </summary>
        void CloseWindows();

        /// <summary>
        /// Adds window.
        /// </summary>
        /// <param name="window">The window to add.</param>
        void AddWindow(IViewsWindow window);

        /// <summary>
        /// Removes window.
        /// </summary>
        /// <param name="window">The window to remove.</param>
        void RemoveWindow(IViewsWindow window);
    }
}