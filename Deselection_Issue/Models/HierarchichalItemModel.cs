using System;
using System.Collections.Generic;

namespace Deselection_Issue.Models
{
    public class HierarchicalItemModel
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public List<HierarchicalItemModel> Children { get; set; }
    }
}