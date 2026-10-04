using System;

namespace CadMacro.Simulator.CommonControls.AttachedBehaviours
{
    using System.Windows;
    using System.Windows.Controls;

    public static class MediaElementBehavior
    {
        /// <summary>
        /// dependencyProperty for IsPlaying
        /// </summary>
        public static readonly DependencyProperty IsPlayingProperty =
            DependencyProperty.RegisterAttached(
                "IsPlaying",
                typeof(bool),
                typeof(MediaElementBehavior),
                new PropertyMetadata(false, OnIsPlayingChanged));
        /// <summary>
        /// getter method for isPlaying prop
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool GetIsPlaying(DependencyObject obj)
            => (bool)obj.GetValue(IsPlayingProperty);

        /// <summary>
        /// setter method for isPlaying prop
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetIsPlaying(DependencyObject obj, bool value)
            => obj.SetValue(IsPlayingProperty, value);

        private static void OnIsPlayingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MediaElement media)
            {
                if ((bool)e.NewValue)
                    media.Play();
                else
                {
                    media.Pause();
                    media.Position = TimeSpan.Zero;
                }
            }
        }
    }
}
