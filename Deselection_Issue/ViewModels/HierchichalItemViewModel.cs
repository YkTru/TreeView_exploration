using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Deselection_Issue.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace Deselection_Issue.ViewModels
{
    public partial class HierarchicalItemViewModel : ObservableObject
    {
        public HierarchicalItemModel Model { get; }


        [ObservableProperty] private bool isExpanded;
        [ObservableProperty] private string name;
        public HierarchicalItemViewModel? Parent { get; private set; }
        public ObservableCollection<HierarchicalItemViewModel> Children { get; }




        public HierarchicalItemViewModel(HierarchicalItemModel model, HierarchicalItemViewModel? parent = null)
        {
            Model = model;
            Name = model.Name;
            Parent = parent;
            Children = new(InitializeChildren(model));

        }

        partial void OnNameChanged(string value)
        {
            Model.Name = value;
        }


        // Helpers
        private IEnumerable<HierarchicalItemViewModel> InitializeChildren(HierarchicalItemModel model) =>
            model.Children?.Select(CreateChild) ?? Enumerable.Empty<HierarchicalItemViewModel>();

        private HierarchicalItemViewModel CreateChild(HierarchicalItemModel childModel) =>
            new(childModel, this);


    }
}