using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Alauda
{
    public class MultiSelectorAttach : DependencyObject
    {
        public static IList GetSelectedItemList(DependencyObject obj)
        {
            return (IList)obj.GetValue(SelectedItemListProperty);
        }

        public static void SetSelectedItemList(DependencyObject obj, IList value)
        {
            obj.SetValue(SelectedItemListProperty, value);
        }

        // Using a DependencyProperty as the backing store for SelectedItemList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemListProperty =
            DependencyProperty.RegisterAttached("SelectedItemList", typeof(IList), typeof(MultiSelectorAttach), new PropertyMetadata(new PropertyChangedCallback(SelectedItemListPropertyChanged)));
        private static void SelectedItemListPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MultiSelector selector = sender as MultiSelector;
            if (selector != null)
            {
                if (e.OldValue == null && e.NewValue != null)
                {
                    selector.SelectionChanged += Selector_SelectionChanged; ;
                }
                else if (e.OldValue != null && e.NewValue == null)
                {
                    selector.SelectionChanged -= Selector_SelectionChanged;
                }
            }
        }
        private static void Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selector = sender as MultiSelector;

            SetSelectedItemList(selector, selector.SelectedItems);
        }
    }
}
