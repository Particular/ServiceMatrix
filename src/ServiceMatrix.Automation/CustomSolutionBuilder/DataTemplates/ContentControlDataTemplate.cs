namespace NServiceBusStudio.Automation.CustomSolutionBuilder.DataTemplates
{
    using NServiceBusStudio.Automation.CustomSolutionBuilder.ViewModels;
    using NuPattern.Runtime.UI.ViewModels;
    using System.Windows;
    using System.Windows.Controls;

    public class ContentControlDataTemplate : DataTemplateSelector
    {
        public DataTemplate ProductViewModelDataTemplate { get; set; }
        public DataTemplate ProductElementViewModelDataTemplate { get; set; }
        public DataTemplate LabelElementViewModelDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is LabelElementViewModel)
            {
                return LabelElementViewModelDataTemplate;
            } 
            else if (item is IProductViewModel)
            {
                return ProductViewModelDataTemplate;
            }
            else if (item is IProductElementViewModel)
            {
                return ProductElementViewModelDataTemplate;
            }

            return null;
        }
    }
}
