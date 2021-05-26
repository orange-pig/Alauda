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

namespace Alauda
{
    public class TextField : TextBox
    {
        public static DependencyProperty CursorColorProperty =
            DependencyProperty.Register("CursorColor", typeof(Color), typeof(TextField),
                new FrameworkPropertyMetadata(Colors.DeepSkyBlue, FrameworkPropertyMetadataOptions.AffectsArrange, null));

        public static DependencyProperty OnSubmittedCommandProperty =
            DependencyProperty.Register("OnSubmittedCommand", typeof(ICommand), typeof(TextField),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange, null));


        static TextField()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextField), new FrameworkPropertyMetadata(typeof(TextField)));
        }


        public TextField()
        {
            this.KeyDown += TextField_KeyDown;
        }


        public Color CursorColor
        {
            get => (Color)GetValue(CursorColorProperty);
            set => SetValue(CursorColorProperty, value);
        }

        public ICommand OnSubmittedCommand
        {
            get => (ICommand)GetValue(OnSubmittedCommandProperty);
            set => SetValue(OnSubmittedCommandProperty, value);
        }


        private void TextField_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (OnSubmittedCommand != null && OnSubmittedCommand.CanExecute(this))
                    OnSubmittedCommand?.Execute(this);
            }
        }
    }
}
