using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace Deselection_Issue.CustomControls
{
    public class CustomTreeViewItem : TreeViewItem
    {
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove ||
                e.Action == NotifyCollectionChangedAction.Reset)
            {
                // Do nothing: bypass auto-selecting a sibling/parent.
                return;
            }
            base.OnItemsChanged(e);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new CustomTreeViewItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is CustomTreeViewItem;
        }
    }
}