﻿using System;
using System.IO;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace Sentiment
{
    /// <summary>
    /// Uses SVG instead of bitmap data to display an image
    /// All SCG files must be embedded as a resource in the /Resources folder
    /// </summary>
    public class SvgImage : SKCanvasView
    {
        string _fileContent;
        int _padding = 6;
        int _iconSize = 24;

        public SvgImage()
        {
            PaintSurface += OnPaintSurface;
            WidthRequest = _iconSize + _padding * 2;
            HeightRequest = _iconSize + _padding * 2;

            if (App.Instance.IsDesignMode)
                BackgroundColor = Color.FromHex("#7000");

#pragma warning disable CS0618 // Type or member is obsolete
            GestureRecognizers.Add(new TapGestureRecognizer((obj) => Clicked?.Invoke(this, new EventArgs())));
#pragma warning restore CS0618 // Type or member is obsolete
        }

        public event EventHandler<EventArgs> Clicked;

        public static readonly BindableProperty ColorProperty =
            BindableProperty.Create(nameof(Color), typeof(Color), typeof(SvgImage), Color.White);

        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly BindableProperty AddPaddingProperty =
            BindableProperty.Create(nameof(AddPadding), typeof(bool), typeof(SvgImage), false);

        public bool AddPadding
        {
            get { return (bool)GetValue(AddPaddingProperty); }
            set { SetValue(AddPaddingProperty, value); }
        }

        public static readonly BindableProperty SourceProperty =
            BindableProperty.Create(nameof(Source), typeof(string), typeof(SvgImage), null);

        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            if (string.IsNullOrEmpty(Source))
                return;

            if (Clicked == null && !AddPadding)
                _padding = 0;

            try
            {
                if (_fileContent == null)
                    _fileContent = Source.GetFileContents();

                if (Clicked == null)
                    _padding = 0;

                var svg = new SkiaSharp.Extended.Svg.SKSvg();
                var bytes = System.Text.Encoding.UTF8.GetBytes(_fileContent);
                var stream = new MemoryStream(bytes);

                svg.Load(stream);
                var canvas = e.Surface.Canvas;
                using (var paint = new SKPaint())
                {
                    if (Color != Color.Lime)
                    {
                        //Set the paint color
                        paint.ColorFilter = SKColorFilter.CreateBlendMode(Color.ToSKColor(), SKBlendMode.SrcIn);
                    }

                    int multiplier = (int)(e.Info.Width / WidthRequest);

                    //Scale up the SVG image to fill the canvas
                    float canvasMin = Math.Min(e.Info.Width - _padding * 2 * multiplier, e.Info.Height - _padding * 2 * multiplier);
                    float svgMax = Math.Max(svg.Picture.CullRect.Width, svg.Picture.CullRect.Height);
                    float scale = canvasMin / svgMax;
                    var matrix = SKMatrix.MakeScale(scale, scale);
                    matrix.TransX = _padding * multiplier;
                    matrix.TransY = _padding * multiplier;

                    canvas.Clear(Color.Transparent.ToSKColor());
                    canvas.DrawPicture(svg.Picture, ref matrix, paint);
                }
            }
            catch (Exception ex)
            {
                //Log.Instance.WriteLine($"Error drawing SvgImage w/ ImagePath {Source}: {ex}");
            }
        }
    }
}
