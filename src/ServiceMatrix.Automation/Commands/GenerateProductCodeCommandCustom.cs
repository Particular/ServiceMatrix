using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using NServiceBusStudio.AutomationSetup;
using NuPattern.Diagnostics;
using NuPattern.VisualStudio;
using NuPattern.VisualStudio.Solution;

namespace NuPattern.Library.Commands
{
    public class GenerateProductCodeCommandCustom : GenerateProductCodeCommand
    {
        private static readonly ITracer tracer = Tracer.Get<GenerateProductCodeCommandCustom>();

        private DTE dte;

        ICollection<TemplateReference> templates;

        [Browsable(false)]
        internal DTE Dte
        {
            get
            {
                return dte ?? (dte = ServiceProvider.GetService<SDTE, DTE>());
            }
        }

        public string TargetNsbVersion { get; set; }

        [Required]
        [Import(AllowDefault = true)]
        public IUserMessageService UserMessageService { get; set; }

        [Required]
        [Import(AllowDefault = true)]
        public ITemplateMappings TemplateMappings { get; set; }

        public override void Execute()
        {
            EnsureNoProjectDesignersAreOpen();

            if (!EnsureCorrectTemplate())
            {
                return;
            }

            try
            {
                base.Execute();
            }
            catch (IOException ex)
            {
                tracer.Error(ex, "The file {0} is locked by a process and cannot be regenerated. Please, close the locking process and try again.", TargetFileName);
            }
        }

        private bool EnsureCorrectTemplate()
        {
            if (string.IsNullOrEmpty(TargetNsbVersion))
            {
                tracer.Verbose("Using configured template for command with no target version for uri '{0}", TemplateUri);
                return true;
            }

            EnsureTemplates();
            if (templates.Count == 0)
            {
                tracer.Verbose("Using configured template for command with no version-specific templates for uri '{0}", TemplateUri);
                return true;
            }

            var version = GetVersion(TargetNsbVersion);
            if (version == null)
            {
                tracer.Warn("Version value '{1}' was not valid for template '{0}", TemplateUri, TargetNsbVersion);
                return false;
            }

            var newTemplate = templates.Where(t => version.Major == t.Version.Major && version >= t.Version).Select(t => t.Template).FirstOrDefault();
            if (newTemplate == null)
            {
                tracer.Error("Skipping code generation for command with version-specific template for {0} with version '{1}'", TemplateUri, TargetNsbVersion);
                return false;
            }

            Template = newTemplate;

            tracer.Info("Chose template '{0}' for version '{1}'", Template, version);

            return true;
        }

        private Version GetVersion(string versionString)
        {
            // If the version syntax in the TargetNsbProperty is modified, this needs to be updated appropriately to match that change.
            versionString = versionString.Replace("NSB ", "");
            Version version;
            return Version.TryParse(versionString, out version) ? version : null;
        }

        private void EnsureTemplates()
        {
            if (templates != null)
            {
                return;
            }

            var mappings = TemplateMappings.GetMappingsForBaseTemplate(TemplateUri.ToString());
            templates =
                mappings == null
                    ? new List<TemplateReference>(0)
                    : mappings.Select(t => new TemplateReference { Version = t.Version, Template = CreateLazyTemplate(new Uri(t.TemplateUri)) }).ToList();
        }

        private Lazy<ITemplate> CreateLazyTemplate(Uri uri)
        {
            return new Lazy<ITemplate>(() =>
            {
                var template = UriService.TryResolveUri<ITemplate>(uri);
                if (template == null)
                    throw new FileNotFoundException("Template not found");

                return template;
            });
        }

        void EnsureNoProjectDesignersAreOpen()
        {
            var notified = UserMessageService == null;

            // TODO: Remove this function when NuPattern implements a workaround. 
            // The workaround is to close all the project designers, which can cause exceptions in NuPattern's t4 unfold logic.
            // Related NuPattern issue: https://github.com/NuPattern/NuPattern/issues/2
            foreach (var window in Dte.Windows.OfType<Window>().Where(w => w.Type == vsWindowType.vsWindowTypeDocument))
            {
                try
                {
                    // just so this expression compiles
                    var ignore = window.ProjectItem;
                }
                catch (InvalidCastException)
                {
                    if (!notified)
                    {
                        notified = true;

                        UserMessageService.ShowInformation("ServiceMatrix has detected that some Project Designers are open. These designers will now be closed in order to proceed with code generation.");
                    }

                    window.Close();
                }
            }
        }

        private class TemplateReference
        {
            public Version Version { get; set; }
            public Lazy<ITemplate> Template { get; set; }
        }
    }
}
