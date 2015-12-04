using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ExplosionStudio
{
    internal sealed class ViewportAdornment1
    {
        readonly IWpfTextView textView;
        readonly IAdornmentLayer adornmentLayer;

        internal ViewportAdornment1(IWpfTextView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            textView = view;
            adornmentLayer = view.GetAdornmentLayer("ViewportAdornment1");
            ((UIElement)view).KeyDown += KeyDown;
        }

        void KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            AddExplosion(textView.Caret.Left, textView.Caret.Top + textView.Caret.Height / 2);
        }

        void AddExplosion(double left, double top)
        {
            var image = new ExplosionImage();
            Canvas.SetLeft(image, left - image.Width / 2);
            Canvas.SetTop(image, top - image.Height / 2);
            adornmentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, image, null);
        }
    }
}
