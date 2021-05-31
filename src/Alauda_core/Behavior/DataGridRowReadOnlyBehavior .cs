using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Alauda
{
    /// <summary>
    /// Custom behavior that allows for DataGrid Rows to be ReadOnly on per-row basis
    /// </summary>
    public class DataGridRowReadOnlyBehavior : Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            if (this.AssociatedObject == null)
                throw new InvalidOperationException("AssociatedObject must not be null");

            AssociatedObject.BeginningEdit += AssociatedObject_BeginningEdit;
        }

        private void AssociatedObject_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            var isReadOnlyRow = ReadOnlyService.GetIsReadOnly(e.Row);
            if (isReadOnlyRow)
                e.Cancel = true;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.BeginningEdit -= AssociatedObject_BeginningEdit;
        }
    }
}
