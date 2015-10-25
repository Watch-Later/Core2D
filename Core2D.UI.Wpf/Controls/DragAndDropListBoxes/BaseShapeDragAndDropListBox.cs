﻿// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Core2D;

namespace Core2D.UI.Wpf.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseShapeDragAndDropListBox : DragAndDropListBox<Core2D.BaseShape>
    {
        /// <summary>
        /// 
        /// </summary>
        public BaseShapeDragAndDropListBox()
            : base()
        {
            this.Initialized += (s, e) => base.Initialize();
        }

        /// <summary>
        /// Updates DataContext binding to ImmutableArray collection property.
        /// </summary>
        /// <param name="array">The updated immutable array.</param>
        public override void UpdateDataContext(ImmutableArray<Core2D.BaseShape> array)
        {
            var editor = (Core2D.Editor)this.Tag;

            var layer = editor.Project.CurrentContainer.CurrentLayer;
            var previous = layer.Shapes;
            var next = array;
            editor.History.Snapshot(previous, next, (p) => layer.Shapes = p);
            layer.Shapes = next;
        }
    }
}
