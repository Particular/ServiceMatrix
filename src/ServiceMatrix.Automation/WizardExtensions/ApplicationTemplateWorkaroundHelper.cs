namespace NServiceBusStudio.Automation.WizardExtensions
{
    using System;
    using System.IO;

    static class ApplicationTemplateWorkaroundHelper
    {
        public static object[] ReplaceCustomParameters(object[] customParameters)
        {
            var templatePath = customParameters.Length > 0 ? (string)customParameters[0] : null;
            if (templatePath != null)
            {
                string zipPath;
                string rootPath;
                if (string.Equals("AppV5.vstemplate", Path.GetFileName(templatePath), StringComparison.OrdinalIgnoreCase)
                    && (zipPath = Path.GetDirectoryName(templatePath)) != null
                    && (rootPath = Path.GetDirectoryName(zipPath)) != null)
                {
                    // replace the template for matching purposes
                    var newCustomParameters = new object[customParameters.Length];
                    Array.Copy(customParameters, newCustomParameters, customParameters.Length);
                    newCustomParameters[0] = Path.Combine(Path.Combine(rootPath, "App.zip"), "App.vstemplate");
                    return newCustomParameters;
                }
            }

            return customParameters;
        }
    }
}