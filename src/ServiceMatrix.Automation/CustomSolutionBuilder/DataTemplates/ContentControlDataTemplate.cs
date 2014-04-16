using NServiceBusStudio.Automation.CustomSolutionBuilder.ViewModels;
using NuPattern.Runtime.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NServiceBusStudio.Automation.CustomSolutionBuilder.DataTemplates
{
    public class ContentControlDataTemplate : DataTemplateSelector
    {
        public DataTemplate ProductViewModelDataTemplate { get; set; }
        public DataTemplate ProductElementViewModelDataTemplate { get; set; }
        public DataTemplate LabelElementViewModelDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is LabelElementViewModel)
            {
                return this.LabelElementViewModelDataTemplate;
            } 
            else if (item is IProductViewModel)
            {
                return this.ProductViewModelDataTemplate;
            }
            else if (item is IProductElementViewModel)
            {
                return this.ProductElementViewModelDataTemplate;
            }

            return null;
        }
    }
}
