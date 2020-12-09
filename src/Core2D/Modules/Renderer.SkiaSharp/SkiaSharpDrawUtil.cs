﻿using System;
using Core2D.Shapes;
using Core2D.Style;
using SkiaSharp;

namespace Core2D.Renderer.SkiaSharp
{
    internal static class SkiaSharpDrawUtil
    {
        public static SKRect ToSKRect(double x, double y, double width, double height)
        {
            float left = (float)x;
            float top = (float)y;
            float right = (float)(x + width);
            float bottom = (float)(y + height);
            return new SKRect(left, top, right, bottom);
        }

        public static SKRect CreateRect(PointShapeViewModel tl, PointShapeViewModel br)
        {
            float left = (float)Math.Min(tl.X, br.X);
            float top = (float)Math.Min(tl.Y, br.Y);
            float right = (float)Math.Max(tl.X, br.X);
            float bottom = (float)Math.Max(tl.Y, br.Y);
            return new SKRect(left, top, right, bottom);
        }

        public static SKColor ToSKColor(BaseColorViewModel colorViewModel)
        {
            return colorViewModel switch
            {
                ArgbColorViewModelViewModel argbColor => new SKColor(argbColor.R, argbColor.G, argbColor.B, argbColor.A),
                _ => throw new NotSupportedException($"The {colorViewModel.GetType()} color type is not supported."),
            };
        }

        public static SKPaint ToSKPaintBrush(BaseColorViewModel colorViewModel)
        {
            var brush = new SKPaint();

            brush.Style = SKPaintStyle.Fill;
            brush.IsAntialias = true;
            brush.IsStroke = false;
            brush.LcdRenderText = true;
            brush.SubpixelText = true;
            brush.Color = ToSKColor(colorViewModel);

            return brush;
        }

        public static SKStrokeCap ToStrokeCap(ShapeStyleViewModel styleViewModel)
        {
            return styleViewModel.Stroke.LineCap switch
            {
                LineCap.Square => SKStrokeCap.Square,
                LineCap.Round => SKStrokeCap.Round,
                _ => SKStrokeCap.Butt,
            };
        }

        public static SKPaint ToSKPaintPen(ShapeStyleViewModel styleViewModel, double strokeWidth)
        {
            var pen = new SKPaint();

            var pathEffect = default(SKPathEffect);
            if (styleViewModel.Stroke.Dashes != null)
            {
                var intervals = StyleHelper.ConvertDashesToFloatArray(styleViewModel.Stroke.Dashes, strokeWidth);
                var phase = (float)(styleViewModel.Stroke.DashOffset * strokeWidth);
                if (intervals != null)
                {
                    pathEffect = SKPathEffect.CreateDash(intervals, phase);
                }
            }

            pen.Style = SKPaintStyle.Stroke;
            pen.IsAntialias = true;
            pen.IsStroke = true;
            pen.StrokeWidth = (float)strokeWidth;
            pen.Color = ToSKColor(styleViewModel.Stroke.ColorViewModel);
            pen.StrokeCap = ToStrokeCap(styleViewModel);
            pen.PathEffect = pathEffect;

            return pen;
        }

        public static SKPaint ToSKPaintPen(BaseColorViewModel colorViewModel, double strokeWidth)
        {
            var pen = new SKPaint();

            var pathEffect = default(SKPathEffect);

            pen.Style = SKPaintStyle.Stroke;
            pen.IsAntialias = true;
            pen.IsStroke = true;
            pen.StrokeWidth = (float)strokeWidth;
            pen.Color = ToSKColor(colorViewModel);
            pen.StrokeCap = SKStrokeCap.Butt;
            pen.PathEffect = pathEffect;

            return pen;
        }

        public static SKPoint GetTextOrigin(ShapeStyleViewModel styleViewModel, ref SKRect rect, ref SKRect size)
        {
            double rwidth = Math.Abs(rect.Right - rect.Left);
            double rheight = Math.Abs(rect.Bottom - rect.Top);
            double swidth = Math.Abs(size.Right - size.Left);
            double sheight = Math.Abs(size.Bottom - size.Top);
            var ox = styleViewModel.TextStyleViewModel.TextHAlignment switch
            {
                TextHAlignment.Left => rect.Left,
                TextHAlignment.Right => rect.Right - swidth,
                _ => (rect.Left + rwidth / 2f) - (swidth / 2f),
            };
            var oy = styleViewModel.TextStyleViewModel.TextVAlignment switch
            {
                TextVAlignment.Top => rect.Top,
                TextVAlignment.Bottom => rect.Bottom - sheight,
                _ => (rect.Bottom - rheight / 2f) - (sheight / 2f),
            };
            return new SKPoint((float)ox, (float)oy);
        }

        public static SKPaint GetSKPaint(string text, ShapeStyleViewModel shapeStyleViewModel, PointShapeViewModel topLeft, PointShapeViewModel bottomRight, out SKPoint origin)
        {
            var pen = ToSKPaintBrush(shapeStyleViewModel.Stroke.ColorViewModel);

            var weight = SKFontStyleWeight.Normal;

            if (shapeStyleViewModel.TextStyleViewModel.FontStyle.HasFlag(FontStyleFlags.Bold))
            {
                weight |= SKFontStyleWeight.Bold;
            }

            var style = SKFontStyleSlant.Upright;

            if (shapeStyleViewModel.TextStyleViewModel.FontStyle.HasFlag(FontStyleFlags.Italic))
            {
                style |= SKFontStyleSlant.Italic;
            }

            var tf = SKTypeface.FromFamilyName(shapeStyleViewModel.TextStyleViewModel.FontName, weight, SKFontStyleWidth.Normal, style);
            pen.Typeface = tf;
            pen.TextEncoding = SKTextEncoding.Utf16;
            pen.TextSize = (float)(shapeStyleViewModel.TextStyleViewModel.FontSize);

            pen.TextAlign = shapeStyleViewModel.TextStyleViewModel.TextHAlignment switch
            {
                TextHAlignment.Center => SKTextAlign.Center,
                TextHAlignment.Right => SKTextAlign.Right,
                _ => SKTextAlign.Left,
            };

            var metrics = pen.FontMetrics;
            var mAscent = metrics.Ascent;
            var mDescent = metrics.Descent;
            var rect = CreateRect(topLeft, bottomRight);
            float x = rect.Left;
            float y = rect.Top;
            float width = rect.Width;
            float height = rect.Height;

            switch (shapeStyleViewModel.TextStyleViewModel.TextVAlignment)
            {
                default:
                case TextVAlignment.Top:
                    y -= mAscent;
                    break;

                case TextVAlignment.Center:
                    y += (height / 2.0f) - (mAscent / 2.0f) - mDescent / 2.0f;
                    break;

                case TextVAlignment.Bottom:
                    y += height - mDescent;
                    break;
            }

            switch (shapeStyleViewModel.TextStyleViewModel.TextHAlignment)
            {
                default:
                case TextHAlignment.Left:
                    // x = x;
                    break;

                case TextHAlignment.Center:
                    x += width / 2.0f;
                    break;

                case TextHAlignment.Right:
                    x += width;
                    break;
            }

            origin = new SKPoint(x, y);

            return pen;
        }
    }
}
