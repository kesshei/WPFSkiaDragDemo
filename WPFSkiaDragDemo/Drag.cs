using SkiaSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFSkiaDragDemo
{
    /// <summary>
    /// 拖曳 
    /// </summary>
    public class Drag
    {
        public SKPoint centerPoint;
        public int Radius = 0;
        private bool Pressed = false;
        private bool CirclePressend = false;
        private SKPoint sKPoint = SKPoint.Empty;
        private SKPoint CirclePoint = SKPoint.Empty;
        private SKCanvas canvas;
        private float dx = 0;
        private float dy = 0;
        /// <summary>
        /// 渲染
        /// </summary>
        public void Render(SKCanvas canvas, SKTypeface Font, int Width, int Height)
        {
            this.canvas = canvas;
            centerPoint = new SKPoint(Width / 2, Height / 2);
            this.Radius = 40;
            canvas.Clear(SKColors.White);
            if (CirclePoint.IsEmpty)
            {
                CirclePoint = new SKPoint(centerPoint.X, centerPoint.Y);
            }
            if (CirclePressend)
            {
                CirclePoint = new SKPoint(sKPoint.X - dx, sKPoint.Y - dy);
                DrawCircle(this.canvas, CirclePoint);
            }
            else
            {
                DrawCircle(this.canvas, CirclePoint);
            }

            using var paint = new SKPaint
            {
                Color = SKColors.Black,
                IsAntialias = true,
                Typeface = Font,
                TextSize = 24
            };
            using var paint2 = new SKPaint
            {
                Color = SKColors.Blue,
                IsAntialias = true,
                Typeface = Font,
                TextSize = 24
            };
            var msg = $"X:{ sKPoint.X}  Y:{sKPoint.Y}  Pressed:{Pressed} CirclePressend:{CirclePressend}";
            string by = $"by 蓝创精英团队";
            canvas.DrawText(msg, 0, 30, paint);
            canvas.DrawText(by, 600, 400, paint2);
        }
        public void MouseMove(SKPoint sKPoint)
        {
            this.sKPoint = sKPoint;
            if (CirclePressend)//按下，就开始拖动
            {
                CirclePoint = sKPoint;
            }
        }
        public void MouseDown(SKPoint sKPoint, bool Pressed)
        {
            this.sKPoint = sKPoint;
            this.Pressed = Pressed;
            if (this.Pressed)
            {
                this.CirclePressend = CheckPoint(sKPoint, CirclePoint);
                if (this.CirclePressend)
                {
                    dx = sKPoint.X - CirclePoint.X;
                    dy = sKPoint.Y - CirclePoint.Y;
                }
            }
            else
            {
                this.CirclePressend = false;
            }
        }
        public bool CheckPoint(SKPoint sKPoint, SKPoint CirclePoint)
        {
            var d = Math.Sqrt(Math.Pow(sKPoint.X - CirclePoint.X, 2) + Math.Pow(sKPoint.Y - CirclePoint.Y, 2));
            return this.Radius >= d;
        }
        /// <summary>
        /// 画一个圆
        /// </summary>
        public void DrawCircle(SKCanvas canvas, SKPoint sKPoint)
        {
            using var paint = new SKPaint
            {
                Color = SKColors.Blue,
                Style = SKPaintStyle.Fill,
                IsAntialias = true,
                StrokeWidth = 2
            };
            canvas.DrawCircle(sKPoint.X, sKPoint.Y, Radius, paint);
        }
    }
}
