using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Patterning.Runtime.UI;
using Microsoft.VisualStudio.Patterning.Runtime;

namespace NServiceBusStudio.Automation.CustomSolutionBuilder.ViewModels
{
    public class LabelElementViewModel : ElementViewModel
    {
        // Methods
        public LabelElementViewModel(IAbstractElement element, SolutionBuilderContext ctx)
            : base(element, ctx)
        {
        }

        // Properties
        public string Label { get; set; }

        public void AddMenuOption(MenuOptionViewModel menuOption)
        {
            this.MenuOptions.Add(menuOption);
        }
    }

    public class LabelProductElementViewModel : ProductViewModel
    {
        // Methods
        public LabelProductElementViewModel(IProduct element, SolutionBuilderContext ctx)
            : base(element, ctx)
        {
        }

        // Properties
        public string Label { get; set; }

        public void AddMenuOption(MenuOptionViewModel menuOption)
        {
            this.MenuOptions.Add(menuOption);
        }
    }

}
