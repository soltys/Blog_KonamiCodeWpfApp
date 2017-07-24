using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace KonamiCodeWpfApp
{
    class DisplayKeyboardBehavior : Behavior<TextBox>
    {
        private Window window;

        protected override void OnAttached()
        {
            this.window = Window.GetWindow(this.AssociatedObject);

            if (window == null)
            {
                throw new ArgumentNullException();
            }

            window.PreviewKeyDown += AssociatedObjectOnKeyDown;

            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            window.PreviewKeyDown -= AssociatedObjectOnKeyDown;
            base.OnDetaching();
        }

        private void AssociatedObjectOnKeyDown(object sender, KeyEventArgs e)
        {
            string keyChar = GetChar(e.Key);

            var oldText = this.AssociatedObject.Text;
            var newText = oldText + keyChar;
            if (newText.Length > 10)
            {
                newText = newText.Substring(newText.Length - 10, 10);
            }
            this.AssociatedObject.Text = newText;

        }

        private static string GetChar(Key key)
        {
            string keyChar = key.ToString();

            if (key == Key.Up)
            {
                keyChar = "↑";
            }
            else if (key == Key.Down)
            {
                keyChar = "↓";
            }
            else if (key == Key.Right)
            {
                keyChar = "→";
            }
            else if (key == Key.Left)
            {
                keyChar = "←";
            }

            return keyChar;
        }
    }
}