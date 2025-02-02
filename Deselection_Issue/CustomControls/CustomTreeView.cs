using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace Deselection_Issue.CustomControls
{
    public class CustomTreeView : TreeView
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new CustomTreeViewItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is CustomTreeViewItem;
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove ||
                e.Action == NotifyCollectionChangedAction.Reset)
            {
                // Do nothing: we are controlling selection via our view-model command.
                return;
            }
            base.OnItemsChanged(e);
        }
    }
}