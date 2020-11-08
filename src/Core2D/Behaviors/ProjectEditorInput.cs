﻿using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.PanAndZoom;
using Core2D.Editor;
using Core2D.Input;

namespace Core2D.Behaviors
{
    public class ProjectEditorInput
    {
        private readonly Control _control = null;
        private AvaloniaInputSource _inputSource = null;
        private ProjectEditorInputTarget _inputTarget = null;
        private InputProcessor _inputProcessor = null;

        public ProjectEditorInput(Control control)
        {
            _control = control;
            _control.GetObservable(Control.DataContextProperty).Subscribe(Changed);
        }

        public void InvalidateChild(double zoomX, double zoomY, double offsetX, double offsetY)
        {
            if (!(_control.DataContext is ProjectEditor projectEditor))
            {
                return;
            }

            var state = projectEditor.PageState;
            if (state != null)
            {
                state.ZoomX = zoomX;
                state.ZoomY = zoomY;
                state.PanX = offsetX;
                state.PanY = offsetY;
            }
        }

        public void Changed(object context)
        {
            Detach();
            Attach();
        }

        public void Attach()
        {
            if (!(_control.DataContext is ProjectEditor projectEditor))
            {
                return;
            }

            var presenterViewData = _control.Find<Control>("presenterViewData");
            var presenterViewTemplate = _control.Find<Control>("presenterViewTemplate");
            var presenterViewEditor = _control.Find<Control>("presenterViewEditor");
            var zoomBorder = _control.Find<ZoomBorder>("zoomBorder");

            if (projectEditor.CanvasPlatform is IEditorCanvasPlatform canvasPlatform)
            {
                canvasPlatform.InvalidateControl = () =>
                {
                    presenterViewData?.InvalidateVisual();
                    presenterViewTemplate?.InvalidateVisual();
                    presenterViewEditor?.InvalidateVisual();
                };
                canvasPlatform.ResetZoom = () => zoomBorder?.Reset();
                canvasPlatform.FillZoom = () => zoomBorder?.Fill();
                canvasPlatform.UniformZoom = () => zoomBorder?.Uniform();
                canvasPlatform.UniformToFillZoom = () => zoomBorder?.UniformToFill();
                canvasPlatform.AutoFitZoom = () => zoomBorder?.AutoFit();
                canvasPlatform.InZoom = () => zoomBorder?.ZoomIn();
                canvasPlatform.OutZoom = () => zoomBorder?.ZoomOut();
                canvasPlatform.Zoom = zoomBorder;
            }

            if (zoomBorder != null)
            {
                zoomBorder.InvalidatedChild = InvalidateChild;
            }

            _inputSource = new AvaloniaInputSource(zoomBorder, presenterViewEditor, p => p);
            _inputTarget = new ProjectEditorInputTarget(projectEditor);
            _inputProcessor = new InputProcessor();
            _inputProcessor.Connect(_inputSource, _inputTarget);
        }

        public void Detach()
        {
            if (!(_control.DataContext is ProjectEditor projectEditor))
            {
                return;
            }

            var zoomBorder = _control.Find<ZoomBorder>("zoomBorder");

            if (projectEditor.CanvasPlatform is IEditorCanvasPlatform canvasPlatform)
            {
                canvasPlatform.InvalidateControl = null;
                canvasPlatform.ResetZoom = null;
                canvasPlatform.FillZoom = null;
                canvasPlatform.UniformZoom = null;
                canvasPlatform.UniformToFillZoom = null;
                canvasPlatform.AutoFitZoom = null;
                canvasPlatform.InZoom = null;
                canvasPlatform.OutZoom = null;
                canvasPlatform.Zoom = null;
            }

            if (zoomBorder != null)
            {
                zoomBorder.InvalidatedChild = null;
            }

            _inputProcessor?.Dispose();
            _inputProcessor = null;
            _inputTarget = null;
            _inputSource = null;
        }
    }
}