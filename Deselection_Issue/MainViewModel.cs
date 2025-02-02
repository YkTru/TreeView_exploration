using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Deselection_Issue.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Deselection_Issue.ViewModels
{

    public partial class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            //🅦 if set to null: won't be able to select any item 
            // SelectedItem = HierarchicalItems.FirstOrDefault();
        }

        [ObservableProperty] private ObservableCollection<HierarchicalItemViewModel> hierarchicalItems = new();
        [ObservableProperty] private HierarchicalItemViewModel? selectedItem;


        #region Item Management

        [RelayCommand]
        private void AddTrack()
        {
            var newModel = new HierarchicalItemModel { Name = "New Track" };
            var newItem = new HierarchicalItemViewModel(newModel);

            HierarchicalItems.Add(newItem);

            //🅦 Ensure new item is selected else won't be able to select any item 
            SelectedItem = newItem;
        }


        [RelayCommand(CanExecute = nameof(CanRemoveSelectedItem))]
        private void RemoveSelectedItem()
        {
            if (SelectedItem == null)
                return;

            var siblings = SelectedItem.Parent?.Children ?? HierarchicalItems;
            int index = siblings.IndexOf(SelectedItem);
            int newIndex = -1;

            if (index > 0)
            {
                newIndex = index - 1;
            }

            else if (siblings.Count > 1)
            {
                newIndex = index + 1;
            }

            if (newIndex >= 0 && newIndex < siblings.Count)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    SelectedItem = siblings[newIndex];
                }, System.Windows.Threading.DispatcherPriority.Send);
            }
            else
            {
                SelectedItem = null;
            }

            siblings.RemoveAt(index);
        }






        private bool CanRemoveSelectedItem() => SelectedItem is not null;


        [RelayCommand(CanExecute = nameof(CanDeselect))]
        private void Deselect()
        {
            SelectedItem = null;
            RefreshCanExecute();
        }
        private bool CanDeselect() => SelectedItem is not null;

        #endregion


        #region Hierarchy Management

        [RelayCommand(CanExecute = nameof(CanAddSibling))]
        private void AddSibling()
        {
            var selected = SelectedItem;
            if (selected is null)
                return;

            var newModel = new HierarchicalItemModel { Name = $"New Sibling {GetSiblingCount() + 1}" };
            var newItem = new HierarchicalItemViewModel(newModel, selected.Parent);

            var siblings = selected.Parent?.Children ?? HierarchicalItems; //no parent = root item
            var insertIndex = siblings.IndexOf(selected) + 1; //insert just after selected item

            siblings.Insert(insertIndex, newItem);
        }
        private bool CanAddSibling() => SelectedItem is not null;


        [RelayCommand(CanExecute = nameof(CanAddChild))]
        private void AddChild()
        {
            var selected = SelectedItem;
            if (selected is null)
                return;

            var newModel = new HierarchicalItemModel { Name = $"New Child {selected.Children.Count + 1}" };
            var newItem = new HierarchicalItemViewModel(newModel, selected);

            selected.Children.Add(newItem);
            selected.IsExpanded = true;

            RefreshCanExecute();
        }
        private bool CanAddChild() => SelectedItem is not null;


        [RelayCommand(CanExecute = nameof(CanMoveUp))]
        private void MoveUp()
        {
            var selected = SelectedItem;
            if (selected is null)
                return;

            var siblings = selected.Parent?.Children ?? HierarchicalItems;
            var index = siblings.IndexOf(selected);

            if (index > 0) // Ensure it's not the first item
            {
                siblings.Move(index, index - 1);
                SelectedItem = siblings[index - 1]; // Ensure correct selection update
            }
            else if (selected.Parent != null) // Move to parent if at the top of siblings
            {
                SelectedItem = selected.Parent;
            }

            RefreshCanExecute();
        }
        private bool CanMoveUp() =>
            SelectedItem is HierarchicalItemViewModel selected &&
            (selected.Parent?.Children ?? HierarchicalItems).IndexOf(selected) > 0;


        [RelayCommand(CanExecute = nameof(CanMoveDown))]
        private void MoveDown()
        {
            var selected = SelectedItem;
            if (selected is null)
                return;

            var siblings = selected.Parent?.Children ?? HierarchicalItems;
            var index = siblings.IndexOf(selected);

            if (index < siblings.Count - 1) // not last item
            {
                siblings.Move(index, index + 1);
                SelectedItem = siblings[index + 1];
            }

            RefreshCanExecute();
        }
        private bool CanMoveDown() =>
            SelectedItem is HierarchicalItemViewModel selected &&
            (selected.Parent?.Children ?? HierarchicalItems).IndexOf(selected) < (selected.Parent?.Children ?? HierarchicalItems).Count - 1;

        #endregion


        #region Expand/Collapse
        [RelayCommand(CanExecute = nameof(CanExpandAll))]
        private void ExpandAll()
        {
            foreach (var root in HierarchicalItems)
            {
                ExpandRecursive(root, true);
            }
        }

        private bool CanExpandAll() => HierarchicalItems.Any();


        [RelayCommand(CanExecute = nameof(CanCollapseAll))]
        private void CollapseAll()
        {
            foreach (var root in HierarchicalItems)
            {
                ExpandRecursive(root, false);
            }
        }

        private bool CanCollapseAll() => HierarchicalItems.Any();

        #endregion


        #region Helpers 
        private void ExpandRecursive(HierarchicalItemViewModel item, bool expand)
        {
            item.IsExpanded = expand;
            foreach (var child in item.Children)
                ExpandRecursive(child, expand);
        }


        private int GetSiblingCount()
        {
            if (SelectedItem?.Parent != null)
            {
                return SelectedItem.Parent.Children.Count;
            }
            else
            {
                return HierarchicalItems.Count;
            }
        }

        partial void OnSelectedItemChanged(HierarchicalItemViewModel? value)
        {
            RefreshCanExecute();
        }

        private void RefreshCanExecute()
        {
            AddSiblingCommand.NotifyCanExecuteChanged();
            AddChildCommand.NotifyCanExecuteChanged();
            DeselectCommand.NotifyCanExecuteChanged();
            MoveUpCommand.NotifyCanExecuteChanged();
            MoveDownCommand.NotifyCanExecuteChanged();
            RemoveSelectedItemCommand.NotifyCanExecuteChanged();
            ExpandAllCommand.NotifyCanExecuteChanged();
            CollapseAllCommand.NotifyCanExecuteChanged();
        }
        #endregion

    }
}
