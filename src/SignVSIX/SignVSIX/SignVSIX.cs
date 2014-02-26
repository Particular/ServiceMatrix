using System;
using System.IO;
using System.IO.Packaging;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace SignVSIX
{
    public class SignVSIX
    {
        /// <summary>
        /// Path to the VSIX file to be signed
        /// </summary>
        public string VSIXPath { get; set; }

        /// <summary>
        /// Path to the Certificate used to sign
        /// </summary>
        public string CertificatePath { get; set; }

        /// <summary>
        /// Certificate's Password
        /// </summary>
        public string CertificatePassword { get; set; }

        /// <summary>
        /// Main msbuild task method
        /// </summary>
        /// <returns></returns>
        public bool Execute()
        {
            if (!File.Exists(this.VSIXPath))
            {
                Console.WriteLine("VSIX not found ({0}).", this.VSIXPath);
                return false;
            }

            if (!File.Exists(this.CertificatePath))
            {
                Console.WriteLine("Certificate not found ({0}).", this.CertificatePath);
                return false;
            }

            // Open and sign VSIX
            using (var package = Package.Open(this.VSIXPath))
            {
                return this.SignAllParts(package);
            }
        }

        /// <summary>
        /// Main signing process
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        private bool SignAllParts(Package package)
        {
            if (package == null)
                throw new ArgumentNullException("SignAllParts(package)");

            // Create the DigitalSignature Manager
            PackageDigitalSignatureManager dsm =
                new PackageDigitalSignatureManager(package);
            dsm.CertificateOption =
                CertificateEmbeddingOption.InSignaturePart;

            // Create a list of all the part URIs in the package to sign
            // (GetParts() also includes PackageRelationship parts).
            System.Collections.Generic.List<Uri> toSign =
                new System.Collections.Generic.List<Uri>();
            foreach (PackagePart packagePart in package.GetParts())
            {
                // Add all package parts to the list for signing.
                toSign.Add(packagePart.Uri);
            }

            // Add the URI for SignatureOrigin PackageRelationship part.
            // The SignatureOrigin relationship is created when Sign() is called.
            // Signing the SignatureOrigin relationship disables counter-signatures.
            toSign.Add(PackUriHelper.GetRelationshipPartUri(dsm.SignatureOrigin));

            // Also sign the SignatureOrigin part.
            toSign.Add(dsm.SignatureOrigin);

            // Add the package relationship to the signature origin to be signed.
            toSign.Add(PackUriHelper.GetRelationshipPartUri(new Uri("/", UriKind.RelativeOrAbsolute)));

            // Sign() will prompt the user to select a Certificate to sign with.
            try
            {
                var cert = new X509Certificate2(this.CertificatePath, (String.IsNullOrEmpty(this.CertificatePassword) ? null : this.CertificatePassword));
                dsm.Sign(toSign, cert);
            }

            // If there are no certificates or the SmartCard manager is
            // not running, catch the exception and show an error message.
            catch (CryptographicException ex)
            {
                Console.WriteLine(
                    "Cannot Sign: {0}", ex.Message);
            }

            return dsm.IsSigned && dsm.VerifySignatures(true) == VerifyResult.Success;

        }// end:SignAllParts()
    }
}
