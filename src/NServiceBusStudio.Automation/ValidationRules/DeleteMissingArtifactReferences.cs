using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using NuPattern.Runtime;
using System.Diagnostics;
using NuPattern.Runtime.Validation;
using NuPattern.Diagnostics;
using NuPattern;
using NuPattern.Runtime.References;
using NuPattern.VisualStudio.Solution;

namespace NServiceBusStudio.Automation.ValidationRules
{
    /// <summary>
    /// Deletes missing artifact references.
    /// </summary>
    [Category("General")]
    [CLSCompliant(false)]
    public class DeleteMissingArtifactReferences : ValidationRule
    {
        private static readonly ITracer tracer = Tracer.Get<DeleteMissingArtifactReferences>();

        /// <summary>
        /// Gets or sets the current element to validate.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement Element { get; set; }

        [Required]
        [Import(AllowDefault = true)]
        public IUriReferenceService UriService { get; set; }

        /// <summary>
        /// Evaluates the violations for the rule.
        /// </summary>
        public override IEnumerable<ValidationResult> Validate()
        {
            Validator.ValidateObject(this, new ValidationContext(this, null, null), true);

            var references = SolutionArtifactLinkReference.GetReferences(this.Element).ToArray();

            using (var tx = this.Element.BeginTransaction())
            {

                try
                {
                    for (int i = 0; i < references.Length; i++)
                    {
                        var reference = references[i];

                        //tracer.TraceData(TraceEventType.Verbose, Resources.EventTracing_StatusEventId,
                        //    Strings.DeleteMissingArtifactReferences.VerifyingLinks(
                        //        this.Element.Info != null ? this.Element.Info.DisplayName : "",
                        //        this.Element.InstanceName,
                        //        i + 1, references.Length));

                        if (UriService.TryResolveUri<IItemContainer>(reference) == null)
                        {
                            var instance = this.Element.References.FirstOrDefault(x =>
                                x.Kind == ReferenceKindConstants.ArtifactLink &&
                                x.Value == reference.OriginalString);

                            if (instance != null)
                            {
                                try
                                {
                                    instance.Delete();
                                }
                                catch (Exception ex)
                                {
                                    tracer.Error(ex.Message);
                                }
                            }

                        }
                    }
                    if (tx != null)
                    {
                        tx.Commit();
                    }
                }
                finally
                {
                    //tracer.TraceData(TraceEventType.Verbose, Resources.EventTracing_StatusEventId, Strings.EventTracing.Ready);
                }
            }
            return Enumerable.Empty<ValidationResult>();
        }
    }
}
