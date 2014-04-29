using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuPattern.Runtime;
using System.Windows.Controls;
using System.Windows;

namespace NServiceBusStudio.Automation.CustomSolutionBuilder.ViewModels
{
    public class InnerPanelViewModel
    {
        public List<InnerPanelItem> Items { get; set; }
        public string Title { get; set; }

        public InnerPanelViewModel()
        {
            Items = new List<InnerPanelItem>();
        }
    }

    public class InnerPanelItem
    {
        public string Text { get; set; }
        public IProductElement Product { get; set; }
        public string IconPath { get; set; }
    }

    public class InnerPanelTitle : InnerPanelItem
    {
        public IEnumerable<string> MenuFilters { get; set; }
        public bool ForceText { get; set; }
    }

    public class InnerPanelTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TitleTemplate { get; set; }
        public DataTemplate ItemTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is InnerPanelTitle) return TitleTemplate;
            return ItemTemplate;
        }
    }
}
