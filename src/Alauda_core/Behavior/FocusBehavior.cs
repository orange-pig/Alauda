using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Alauda
{
    public static class FocusBehavior
    {
        public static bool GetIsFocus(DependencyObject d)
        {
            return (bool)d.GetValue(IsFocusProperty);
        }

        public static void SetIsFocus(DependencyObject d, bool val)
        {
            d.SetValue(IsFocusProperty, val);
        }

        public static readonly DependencyProperty IsFocusProperty =
            DependencyProperty.RegisterAttached(
                "IsFocus",
                typeof(bool),
                typeof(FocusBehavior),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.None,
                    (d, e) =>
                    {
                        if ((bool)e.NewValue)
                        {
                            if (d is UIElement)
                            {
                                ((UIElement)d).Focus();
                                if(d is TextBox)
                                {
                                    ((TextBox)d).SelectAll();
                                }
                            }
                        }
                    }
                )
            );
    }
}
