using System;
using System.Collections.Generic;
using NuGet;

namespace NServiceBusStudio.Automation.NuGetExtensions
{
    public class MachineWideSettings : IMachineWideSettings
    {
        Lazy<IEnumerable<Settings>> settings;

        public MachineWideSettings()
        {
            var baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            settings = new Lazy<IEnumerable<Settings>>(() => NuGet.Settings.LoadMachineWideSettings(new PhysicalFileSystem(baseDirectory)));
        }

        public IEnumerable<Settings> Settings
        {
            get { return settings.Value; }
        }
    }
}
