using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace WPFSkiaDragDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SKTypeface Font;
        public Drag drawClock = new Drag();
        public MainWindow()
        {
            InitializeComponent();
            // 获取宋体在字体集合中的下标
            var index = SKFontManager.Default.FontFamilies.ToList().IndexOf("微软雅黑");
            // 创建宋体字形
            Font = SKFontManager.Default.GetFontStyles(index).CreateTypeface(0);
            skContainer.PaintSurface += SkContainer_PaintSurface;
            skContainer.MouseDown += SkContainer_MouseDown;
            skContainer.MouseUp += SkContainer_MouseUp;
            skContainer.MouseMove += SkContainer_MouseMove;
            _ = Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        Dispatcher.Invoke(() =>
                        {
                            skContainer.InvalidateVisual();
                        });
                        _ = SpinWait.SpinUntil(() => false, 1000 / 60);//每秒60帧
                    }
                    catch
                    {
                        break;
                    }
                }
            });
        }

        private void SkContainer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var cur = e.GetPosition(sender as IInputElement);
            drawClock.MouseDown(new SKPoint((float)cur.X, (float)cur.Y), true);
        }
        private void SkContainer_MouseUp(object sender, MouseEventArgs e)
        {
            var cur = e.GetPosition(sender as IInputElement);
            drawClock.MouseDown(new SKPoint((float)cur.X, (float)cur.Y), false);
        }
        private void SkContainer_MouseMove(object sender, MouseEventArgs e)
        {
            var cur = e.GetPosition(sender as IInputElement);
            drawClock.MouseMove(new SKPoint((float)cur.X, (float)cur.Y));
        }

        private void SkContainer_PaintSurface(object? sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            drawClock.Render(canvas, Font, e.Info.Width, e.Info.Height);
        }
    }
}
