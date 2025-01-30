using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Deselection_Issue.AttachedProperties
{
    public static class TreeViewSelectedItemBehavior
    {

        #region Selected AP
        public static readonly DependencyProperty SelectedItemProperty =
           DependencyProperty.RegisterAttached(
               "SelectedItem",
               typeof(object),
               typeof(TreeViewSelectedItemBehavior),
               new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemChanged));

        public static object GetSelectedItem(DependencyObject obj) => obj.GetValue(SelectedItemProperty);
        public static void SetSelectedItem(DependencyObject obj, object value) => obj.SetValue(SelectedItemProperty, value);
        #endregion

        #region IsUpdating AP
        private static readonly DependencyProperty IsUpdatingProperty =
            DependencyProperty.RegisterAttached(
                "IsUpdating",
                typeof(bool),
                typeof(TreeViewSelectedItemBehavior),
                new PropertyMetadata(false));

        private static bool GetIsUpdating(DependencyObject obj) => (bool)obj.GetValue(IsUpdatingProperty);
        private static void SetIsUpdating(DependencyObject obj, bool value) => obj.SetValue(IsUpdatingProperty, value);
        #endregion


        #region Selection Management

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TreeView treeView)
                return;

            // Prevent event from being added multiple times
            treeView.SelectedItemChanged -= TreeView_SelectedItemChanged;
            treeView.SelectedItemChanged += TreeView_SelectedItemChanged;

            // Prevent unnecessary selection updates during rapid key presses
            if (treeView.IsKeyboardFocusWithin)
                return;

            Action selectItem = () => SelectTreeViewItem(treeView, e.NewValue);
            PerformSelection(treeView, e.NewValue, selectItem);
        }


        private static void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (sender is not TreeView treeView || GetIsUpdating(treeView))
                return;

            // Prevent flickering and multiple redundant updates
            if (GetSelectedItem(treeView) == e.NewValue)
                return;

            SetIsUpdating(treeView, true);
            SetSelectedItem(treeView, e.NewValue);
            System.Diagnostics.Debug.WriteLine($"SelectedItem set to: {e.NewValue}");
            SetIsUpdating(treeView, false);
        }



        #endregion

        #region Deselection Management

        private static void UnselectAllItems(this TreeView treeView)
        {
            foreach (var item in treeView.Items.Cast<object>()
                .Select(treeView.ItemContainerGenerator.ContainerFromItem)
                .OfType<TreeViewItem>())
            {
                item.UnselectRecursive();
            }
        }

        private static void UnselectRecursive(this TreeViewItem item)
        {
            item.IsSelected = false;

            foreach (var child in item.Items.Cast<object>()
                .Select(item.ItemContainerGenerator.ContainerFromItem)
                .OfType<TreeViewItem>())
            {
                child.UnselectRecursive();
            }
        }

        #endregion


        #region Helper Methods
        private static void SelectTreeViewItem(TreeView treeView, object selectedItem)
        {
            var item = FindTreeViewItem(treeView, selectedItem);
            if (item is not null)
            {
                item.IsSelected = true;
                item.BringIntoView();
            }
        }

        private static void PerformSelection(TreeView treeView, object selectedItem, Action selectAction)
        {
            if (selectedItem != null)
            {
                treeView.Dispatcher.BeginInvoke(DispatcherPriority.Background, selectAction);
            }
        }



        private static TreeViewItem? FindTreeViewItem(ItemsControl container, object item)
        {
            return container.Items.Cast<object>()
                .Select(i => container.ItemContainerGenerator.ContainerFromItem(i) as TreeViewItem)
                .Where(child => child is not null)
                .Select(child => child!.DataContext == item ? child : FindTreeViewItem(child, item))
                .FirstOrDefault(result => result is not null);
        }
        #endregion

    }
}
