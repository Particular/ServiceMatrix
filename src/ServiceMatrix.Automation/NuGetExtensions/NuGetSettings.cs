using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace NServiceBusStudio.Automation.NuGetExtensions
{
    public class NuGetSettings
    {
        private const string SettingsRoot = "NuGet";
        private const string SelectedPropertyName = "SelectedProvider";
        private const string IncludePrereleaseName = "Prerelease";

        private NuGetSettings()
        {
        }

        public bool Prerelease { get; private set; }
        public int SelectedProvider { get; private set; }

        public static NuGetSettings ReadSettings()
        {
            var settingsProvider = (IVsSettingsManager)Package.GetGlobalService(typeof(SVsSettingsManager));
            if (settingsProvider == null)
            {
                return null;
            }

            IVsSettingsStore settingsStore;
            if (!(ErrorHandler.Succeeded(settingsProvider.GetReadOnlySettingsStore((uint)__VsSettingsScope.SettingsScope_UserSettings, out settingsStore))
                && settingsStore != null))
            {
                return null;
            }

            var includePreReleaseValue = 0;
            var selectedProviderValue = 0;
            if (CollectionExists(settingsStore, SettingsRoot))
            {
                if (!ErrorHandler.Succeeded(settingsStore.GetBoolOrDefault(SettingsRoot, IncludePrereleaseName, 0, out includePreReleaseValue)))
                {
                    includePreReleaseValue = 0;
                }

                if (!ErrorHandler.Succeeded(settingsStore.GetIntOrDefault(SettingsRoot, SelectedPropertyName, 0, out selectedProviderValue)))
                {
                    selectedProviderValue = 0;
                }
            }

            return new NuGetSettings
            {
                Prerelease = includePreReleaseValue != 0,
                SelectedProvider = selectedProviderValue
            };
        }

        private static bool CollectionExists(IVsSettingsStore settingsStore, string collection)
        {
            int exists;
            var hr = settingsStore.CollectionExists(collection, out exists);
            return ErrorHandler.Succeeded(hr) && exists == 1;
        }
    }
}
