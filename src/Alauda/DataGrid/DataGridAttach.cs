using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections;
using System.Windows.Controls.Primitives;

namespace Alauda
{
    public class DataGridAttach : DependencyObject
    {
        public static readonly DependencyProperty DataGridDoubleClickProperty =
            DependencyProperty.RegisterAttached("DataGridDoubleClickCommand", typeof(ICommand), typeof(DataGridAttach), new PropertyMetadata(new PropertyChangedCallback(AttachOrRemoveDataGridDoubleClickEvent)));
        public static void AttachOrRemoveDataGridDoubleClickEvent(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            DataGrid dataGrid = obj as DataGrid;
            if (dataGrid != null)
            {
                ICommand cmd = (ICommand)args.NewValue;

                if (args.OldValue == null && args.NewValue != null)
                {
                    dataGrid.MouseDoubleClick += ExecuteDataGridDoubleClick;
                }
                else if (args.OldValue != null && args.NewValue == null)
                {
                    dataGrid.MouseDoubleClick -= ExecuteDataGridDoubleClick;
                }
            }
        }
        private static void ExecuteDataGridDoubleClick(object sender, MouseButtonEventArgs args)
        {
            DependencyObject obj = sender as DependencyObject;
            var ancestor = FindAncestor<DataGridRow>(args.OriginalSource as DependencyObject);
            if (ancestor != null)
            {
                ICommand cmd = (ICommand)obj.GetValue(DataGridDoubleClickProperty);
                if (cmd != null)
                {
                    if (cmd.CanExecute(obj))
                    {
                        cmd.Execute(obj);
                    }
                }
            }
        }

        public static ICommand GetDataGridDoubleClickCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(DataGridDoubleClickProperty);
        }

        public static void SetDataGridDoubleClickCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(DataGridDoubleClickProperty, value);
        }


        private static T FindAncestor<T>(DependencyObject dependencyObject)
            where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);

            if (parent == null) return null;

            var parentT = parent as T;
            return parentT ?? FindAncestor<T>(parent);
        }
    }
}
