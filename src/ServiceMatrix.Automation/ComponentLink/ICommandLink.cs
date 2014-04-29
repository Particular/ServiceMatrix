using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbstractEndpoint;

namespace NServiceBusStudio
{
    partial interface ICommandLink
    {
        string GetBaseTypeCodeDefinition();
        string GetClassNameSuffix();
    }
    
    partial class CommandLink
    {
        public string GetBaseTypeCodeDefinition()
        {
            var c = ComponentBaseType;

            if (c != null && c != string.Empty)
            {
                return ": " + c;
            }
            else
                return string.Empty;
        }

        public string GetClassNameSuffix()
        {
            var result = (Parent.Parent.DeployedTo != null) ?
                Parent.Parent.DeployedTo.Select(ep => ep.CustomizationFuncs().GetClassNameSuffix(Parent.Parent))
                .FirstOrDefault(s => s != null && s != string.Empty)?? string.Empty
                : string.Empty;
            return result;
        }
    }
}
