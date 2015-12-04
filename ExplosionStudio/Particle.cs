using System;
using System.Windows;
using System.Windows.Media;

namespace ExplosionStudio
{
    class Particle
    {
        double xVelocity;
        double yVelocity;
        double yAcceleration;
        double sizeVelocity;
        double alphaVelocity;
        double size;
        double alpha;
        EllipseGeometry geometry;
        SolidColorBrush brush;
        internal GeometryDrawing Drawing { get; }
        internal Point Point => geometry.Center;

        internal Particle(Point point, Random random)
        {
            size = 2.0 + random.NextDouble() * 2.0;
            alpha = 0.2 + random.NextDouble() * 0.8;

            xVelocity = (random.NextDouble() * 1.6) - 0.8;
            yVelocity = -(0.7 + random.NextDouble() * 2.0);
            yAcceleration = 0.02 + random.NextDouble() * 0.08;
            sizeVelocity = (random.NextDouble() * 1.2) - 0.6;
            alphaVelocity = -(random.NextDouble() * 0.05);

            geometry = new EllipseGeometry(point, size, size);
            brush = new SolidColorBrush(Color.FromArgb(PercentageToByte(alpha), 255, 200, 200));

            Drawing = new GeometryDrawing();
            Drawing.Brush = brush;
            Drawing.Geometry = geometry;
        }

        internal void Step()
        {
            size += sizeVelocity;
            alpha += alphaVelocity;

            brush.Color = Color.FromArgb(PercentageToByte(alpha), brush.Color.R, brush.Color.G, brush.Color.B);

            var center = geometry.Center;
            center.Offset(xVelocity, yVelocity);
            geometry.Center = center;

            yVelocity += yAcceleration;
        }

        byte PercentageToByte(double percentage)
        {
            percentage = Math.Max(0.0, Math.Min(1.0, percentage));
            return (byte)(byte.MaxValue * percentage);
        }
    }
}
