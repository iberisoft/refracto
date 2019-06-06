using System.Collections.Specialized;
using System.Windows.Controls;

namespace Refracto
{
    public class ListViewEx : ListView
    {
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            var gridView = View as GridView;
            if (gridView != null)
            {
                foreach (var column in gridView.Columns)
                {
                    column.Width = column.ActualWidth;
                    column.Width = double.NaN;
                }
            }

            base.OnItemsChanged(e);
        }
    }
}
