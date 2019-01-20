using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using RealTimeGraphX;
using RealTimeGraphX.UWP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

/// <summary>
/// Contains a collection of extension methods.
/// </summary>
internal static class ExtensionMethods
{
    /// <summary>
    /// Draws a polyline.
    /// </summary>
    /// <param name="session">The session.</param>
    /// <param name="points">The points.</param>
    /// <param name="color">The color.</param>
    /// <param name="strokeThickness">The stroke thickness.</param>
    public static void DrawPolyline(this CanvasDrawingSession session, List<Vector2> points, Color color, float strokeThickness)
    {
        CanvasPathBuilder pathBuilder = new CanvasPathBuilder(session);

        pathBuilder.BeginFigure(points[0].X, points[0].Y);

        for (int i = 1; i < points.Count; i++)
        {
            pathBuilder.AddLine(points[i].X, points[i].Y);
        }

        pathBuilder.EndFigure(CanvasFigureLoop.Open);

        session.DrawGeometry(CanvasGeometry.CreatePath(pathBuilder), color, strokeThickness);
    }

    /// <summary>
    /// Fills a polygon.
    /// </summary>
    /// <param name="session">The session.</param>
    /// <param name="surface">The surface.</param>
    /// <param name="series">The series.</param>
    /// <param name="points">The points.</param>
    /// <param name="brush">The brush.</param>
    public static void FillPolygon(this CanvasDrawingSession session, UwpGraphSurface surface, UwpGraphDataSeries series, List<Vector2> points, Brush brush)
    {
        var size = surface.GetSize();
        var geometry = CanvasGeometry.CreatePolygon(session, points.ToArray());
        var v = series.GetCanvasBrush(session, size.Width, size.Height);
        session.FillGeometry(geometry, v);
    }
}
