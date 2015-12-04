using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ExplosionStudio
{
    class ExplosionImage : Image
    {
        public new int Width = 300;
        public new int Height = 300;

        readonly Rect boundsRect;
        readonly GeometryDrawing boundsDrawing;
        readonly List<Particle> particles;

        int Frame
        {
            get { return (int)GetValue(FrameProperty); }
            set { SetValue(FrameProperty, value); }
        }

        static readonly DependencyProperty FrameProperty = DependencyProperty.Register(nameof(Frame), typeof(int), typeof(ExplosionImage), new UIPropertyMetadata(0, new PropertyChangedCallback(OnFrameChanged)));

        internal ExplosionImage()
        {
            var random = new Random();
            particles = new List<Particle>();
            for (var i = 0; i < random.Next(10, 20); i++)
            {
                particles.Add(new Particle(new Point(Width / 2, Height / 2), random));
            }

            boundsRect = new Rect(0, 0, Width, Height);
            boundsDrawing = new GeometryDrawing();
            boundsDrawing.Brush = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            boundsDrawing.Geometry = new RectangleGeometry(boundsRect);

            var frameAnimation = new Int32Animation();
            frameAnimation.Duration = new Duration(TimeSpan.FromSeconds(5));
            frameAnimation.To = (int)frameAnimation.Duration.TimeSpan.TotalMilliseconds;
            frameAnimation.Completed += FrameAnimation_Completed;
            BeginAnimation(FrameProperty, frameAnimation);
        }

        void FrameAnimation_Completed(object sender, EventArgs e)
        {
            ((IAdornmentLayer)Parent).RemoveAdornment(this);
        }

        static void OnFrameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var explosionImage = (ExplosionImage)d;

            var drawingGroup = new DrawingGroup();
            drawingGroup.Children.Add(explosionImage.boundsDrawing);
            foreach (var particle in explosionImage.particles)
            {
                // Only draw the particle if it's inside the bounding box
                if (explosionImage.boundsRect.Contains(particle.Point))
                {
                    drawingGroup.Children.Add(particle.Drawing);
                }

                particle.Step();
            }

            var drawingImage = new DrawingImage(drawingGroup);
            explosionImage.Source = drawingImage;
        }
    }
}
