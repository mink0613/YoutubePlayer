﻿using System.Windows;
using System.Windows.Controls;

namespace YoutubePlayer.Common
{
    public static class WebBrowserHelper
    {
        public static readonly DependencyProperty BindableSourceProperty = 
            DependencyProperty.RegisterAttached("BindableSource", typeof(string), typeof(WebBrowserHelper), new UIPropertyMetadata(null, BindableSourcePropertyChanged));

        private static WebBrowser _self;

        public static string GetBindableSource(DependencyObject obj)
        {
            return (string)obj.GetValue(BindableSourceProperty);
        }

        public static void SetBindableSource(DependencyObject obj, string value)
        {
            obj.SetValue(BindableSourceProperty, value);
        }

        public static void BindableSourcePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            WebBrowser browser = o as WebBrowser;
            if (browser != null)
            {
                string uri = e.NewValue as string;
                browser.NavigateToString(YouTubeHelper.GetEmbeddedYouTubeVideoUrl(uri));
                _self = browser;
            }
        }
    }
}
