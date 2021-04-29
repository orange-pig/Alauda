using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;

namespace Alauda
{
    public class AppWindow : Window
    {

        static AppWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AppWindow), new FrameworkPropertyMetadata(typeof(AppWindow)));

        }

        private WindowChrome _chrome;

        public AppWindow()
        {
            _chrome = new WindowChrome()
            {
                ResizeBorderThickness = new Thickness(0),
                CaptionHeight = 28,
                CornerRadius = new CornerRadius(0),
                GlassFrameThickness = new Thickness(0)
            };

            WindowChrome.SetWindowChrome(this, _chrome);
        }

        public double CaptionHeight
        {
            get => _chrome.CaptionHeight;
            set
            {
                _chrome.CaptionHeight = value;
                WindowChrome.SetWindowChrome(this, _chrome);
            }
        }
    }
}
