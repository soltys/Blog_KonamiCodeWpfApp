﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;

namespace KonamiCodeWpfApp
{
    class KonamiCodeBehavior : Behavior<Control>
    {
        public static readonly DependencyProperty StoryboardProperty =
            DependencyProperty.Register("Storyboard", typeof(Storyboard),
                typeof(KonamiCodeBehavior), new FrameworkPropertyMetadata(null));

        public Storyboard Storyboard
        {
            get => (Storyboard)GetValue(StoryboardProperty);
            set => SetValue(StoryboardProperty, value);
        }

        private int currentKey = 0;
        private readonly Key[] konamiKeysSequence = new[]
        {
            Key.Up,Key.Up,Key.Down,Key.Down,
            Key.Left,Key.Right,Key.Left,Key.Right,
            Key.B,Key.A,
        };

        protected override void OnAttached()
        {
            this.AssociatedObject.KeyDown += AssociatedObjectOnKeyDown;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.KeyDown -= AssociatedObjectOnKeyDown;
            base.OnDetaching();
        }

        private void AssociatedObjectOnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            var correctKey = keyEventArgs.Key == konamiKeysSequence[currentKey];
            if (correctKey)
            {
                currentKey++;
                var sequenceComplete = currentKey == konamiKeysSequence.Length;
                if (sequenceComplete)
                {
                    Storyboard.Begin();
                    currentKey = 0;
                }
            }
            else
            {
                currentKey = 0;
            }
        }
    }

    class DisplayKeyboardBehavior : Behavior<TextBox>
    {
        private Window window;

        protected override void OnAttached()
        {
            this.window = Window.GetWindow(this.AssociatedObject);

            if (window ==null)
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