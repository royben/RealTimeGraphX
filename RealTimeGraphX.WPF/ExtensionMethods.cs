using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeGraphX.WPF
{
    /// <summary>
    /// Contains a collection of extension methods.
    /// </summary>
    internal static class ExtensionMethods
    {
        /// <summary>
        /// Converts this WPF color to a GDI color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        internal static Color ToGdiColor(this System.Windows.Media.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// Converts this WPF brush to a GDI brush.
        /// </summary>
        /// <param name="brush">The brush.</param>
        /// <returns></returns>
        internal static Brush ToGdiBrush(this System.Windows.Media.Brush brush)
        {
            if (brush.GetType() == typeof(System.Windows.Media.SolidColorBrush))
            {
                return new SolidBrush((brush as System.Windows.Media.SolidColorBrush).Color.ToGdiColor());
            }
            else if (brush.GetType() == typeof(System.Windows.Media.LinearGradientBrush))
            {
                System.Windows.Media.LinearGradientBrush b = brush as System.Windows.Media.LinearGradientBrush;

                double angle = Math.Atan2(b.EndPoint.Y - b.StartPoint.Y, b.EndPoint.X - b.StartPoint.X) * 180 / Math.PI;

                LinearGradientBrush gradient = new LinearGradientBrush(new Rectangle(0, 0, 200, 100), Color.Black, Color.Black, (float)angle);

                ColorBlend blend = new ColorBlend();

                List<Color> colors = new List<Color>();
                List<float> offsets = new List<float>();

                foreach (var stop in b.GradientStops)
                {
                    colors.Add(stop.Color.ToGdiColor());
                    offsets.Add((float)stop.Offset);
                }

                blend.Colors = colors.ToArray();
                blend.Positions = offsets.ToArray();

                gradient.InterpolationColors = blend;

                return gradient;
            }
            else
            {
                return new LinearGradientBrush(new PointF(0, 0), new Point(200, 100), Color.Black, Color.Black);
            }
        }

        /// <summary>
        /// Determines whether this dependency object is running in design mode.
        /// </summary>
        /// <param name="obj">The object.</param>
        internal static bool IsInDesignMode(this System.Windows.DependencyObject obj)
        {
            return (DesignerProperties.GetIsInDesignMode(obj));
        }
    }
}
