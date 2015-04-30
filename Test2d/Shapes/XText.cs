﻿// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;

namespace Test2d
{
    public class XText : BaseShape
    {
        private XPoint _topLeft;
        private XPoint _bottomRight;
        private bool _isFilled;
        private string _text;

        public XPoint TopLeft
        {
            get { return _topLeft; }
            set
            {
                if (value != _topLeft)
                {
                    _topLeft = value;
                    Notify("TopLeft");
                }
            }
        }

        public XPoint BottomRight
        {
            get { return _bottomRight; }
            set
            {
                if (value != _bottomRight)
                {
                    _bottomRight = value;
                    Notify("BottomRight");
                }
            }
        }

        public bool IsFilled
        {
            get { return _isFilled; }
            set
            {
                if (value != _isFilled)
                {
                    _isFilled = value;
                    Notify("IsFilled");
                }
            }
        }
        
        public string Text
        {
            get { return _text; }
            set
            {
                if (value != _text)
                {
                    _text = value;
                    Notify("Text");
                }
            }
        }
        
        public override void Draw(object dc, IRenderer renderer, double dx, double dy)
        {
            if (State.HasFlag(ShapeState.Visible))
            {
                renderer.Draw(dc, this, dx, dy);
            }

            if (renderer.SelectedShape != null)
            {
                if (this == renderer.SelectedShape)
                {
                    _topLeft.Draw(dc, renderer, dx, dy);
                    _bottomRight.Draw(dc, renderer, dx, dy);
                }
                else if (_topLeft == renderer.SelectedShape)
                {
                    _topLeft.Draw(dc, renderer, dx, dy);
                }
                else if (_bottomRight == renderer.SelectedShape)
                {
                    _bottomRight.Draw(dc, renderer, dx, dy);
                }
            }
            
            if (renderer.SelectedShapes != null)
            {
                if (renderer.SelectedShapes.Contains(this))
                {
                    _topLeft.Draw(dc, renderer, dx, dy);
                    _bottomRight.Draw(dc, renderer, dx, dy);
                }
            }
        }

        public override void Move(double dx, double dy)
        {
            TopLeft.Move(dx, dy);
            BottomRight.Move(dx, dy);
        }

        public static XText Create(
            double x1, double y1,
            double x2, double y2,
            ShapeStyle style,
            BaseShape point,
            string text,
            bool isFilled = false,
            string name = "")
        {
            return new XText()
            {
                Name = name,
                Style = style,
                TopLeft = XPoint.Create(x1, y1, point),
                BottomRight = XPoint.Create(x2, y2, point),
                IsFilled = isFilled,
                Text = text
            };
        }

        public static XText Create(
            double x, double y,
            ShapeStyle style,
            BaseShape point,
            string text,
            bool isFilled = false,
            string name = "")
        {
            return Create(x, y, x, y, style, point, text, isFilled, name);
        }
    }
}
