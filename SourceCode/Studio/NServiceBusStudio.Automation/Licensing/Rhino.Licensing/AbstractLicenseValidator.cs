namespace Rhino.Licensing
{
    using NuPattern.Diagnostics;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using System.Security.Cryptography.Xml;
    using System.Threading;
    using System.Xml;
    

    public abstract class AbstractLicenseValidator
    {
        protected static readonly ITracer Log = Tracer.Get<AbstractLicenseValidator>();
        private readonly Timer nextLeaseTimer;
        private readonly string publicKey;

        public event Action<InvalidationType> LicenseInvalidated;

        protected AbstractLicenseValidator(string publicKey)
        {
            this.LeaseTimeout = TimeSpan.FromHours(5.0);
            this.LicenseAttributes = new Dictionary<string, string>();
            this.nextLeaseTimer = new Timer(new TimerCallback(this.LeaseLicenseAgain));
            this.publicKey = publicKey;
        }

        public virtual void AssertValidLicense()
        {
            this.LicenseAttributes.Clear();
            if (!this.HasExistingLicense())
            {
                Log.Warn("Could not validate existing license\r\n{0}", new object[] { this.License });
                throw new LicenseNotFoundException();
            }
        }

        private bool HasExistingLicense()
        {
            bool flag;
            try
            {
                if (!this.TryLoadingLicenseValuesFromValidatedXml())
                {
                    Log.Warn("Failed validating license:\r\n{0}", new object[] { this.License });
                    return false;
                }
                Log.Info("License expiration date is {0}", new object[] { this.ExpirationDate });
                if (DateTime.UtcNow >= this.ExpirationDate)
                {
                    throw new LicenseExpiredException("Expiration Date : " + this.ExpirationDate);
                }
                flag = true;
            }
            catch (RhinoLicensingException)
            {
                throw;
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }

        private void LeaseLicenseAgain(object state)
        {
            if (!this.HasExistingLicense())
            {
                this.RaiseLicenseInvalidated();
            }
        }

        private void RaiseLicenseInvalidated()
        {
            Action<InvalidationType> licenseInvalidated = this.LicenseInvalidated;
            if (licenseInvalidated == null)
            {
                throw new InvalidOperationException("License was invalidated, but there is no one subscribe to the LicenseInvalidated event");
            }
            licenseInvalidated(InvalidationType.TimeExpired);
        }

        private bool TryGetValidDocument(string licensePublicKey, XmlDocument doc)
        {
            RSACryptoServiceProvider key = new RSACryptoServiceProvider();
            key.FromXmlString(licensePublicKey);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("sig", "http://www.w3.org/2000/09/xmldsig#");
            SignedXml xml = new SignedXml(doc);
            XmlElement element = (XmlElement)doc.SelectSingleNode("//sig:Signature", nsmgr);
            if (element == null)
            {
                Log.Warn("Could not find this signature node on license:\r\n{0}", new object[] { this.License });
                return false;
            }
            xml.LoadXml(element);
            return xml.CheckSignature(key);
        }

        public bool TryLoadingLicenseValuesFromValidatedXml()
        {
            bool flag;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(this.License);
                if (!this.TryGetValidDocument(this.publicKey, doc))
                {
                    Log.Warn("Could not validate xml signature of:\r\n{0}", new object[] { this.License });
                    return false;
                }
                if (doc.FirstChild == null)
                {
                    Log.Warn("Could not find first child of:\r\n{0}", new object[] { this.License });
                    return false;
                }
                bool flag2 = this.ValidateXmlDocumentLicense(doc);
                if (flag2)
                {
                    this.nextLeaseTimer.Change(this.LeaseTimeout, this.LeaseTimeout);
                }
                flag = flag2;
            }
            catch (RhinoLicensingException)
            {
                throw;
            }
            catch (Exception exception)
            {
                Log.Error("Could not validate license", exception);
                flag = false;
            }
            return flag;
        }

        internal bool ValidateXmlDocumentLicense(XmlDocument doc)
        {
            XmlNode node = doc.SelectSingleNode("/license/@id");
            if (node == null)
            {
                Log.Warn("Could not find id attribute in license:\r\n{0}", new object[] { this.License });
                return false;
            }
            this.UserId = new Guid(node.Value);
            XmlNode node2 = doc.SelectSingleNode("/license/@expiration");
            if (node2 == null)
            {
                Log.Warn("Could not find expiration in license:\r\n{0}", new object[] { this.License });
                return false;
            }
            this.ExpirationDate = DateTime.ParseExact(node2.Value, "yyyy-MM-ddTHH:mm:ss.fffffff", CultureInfo.InvariantCulture);
            XmlNode node3 = doc.SelectSingleNode("/license/@type");
            if (node3 == null)
            {
                Log.Warn("Could not find license type in {0}", new object[] { node3 });
                return false;
            }
            this.LicenseType = (Rhino.Licensing.LicenseType)Enum.Parse(typeof(Rhino.Licensing.LicenseType), node3.Value);
            XmlNode node4 = doc.SelectSingleNode("/license/name/text()");
            if (node4 == null)
            {
                Log.Warn("Could not find licensee's name in license:\r\n{0}", new object[] { this.License });
                return false;
            }
            this.Name = node4.Value;
            foreach (XmlAttribute attribute in doc.SelectSingleNode("/license").Attributes)
            {
                if ((!(attribute.Name == "type") && !(attribute.Name == "expiration")) && !(attribute.Name == "id"))
                {
                    this.LicenseAttributes[attribute.Name] = attribute.Value;
                }
            }
            return true;
        }

        public DateTime ExpirationDate { get; private set; }

        public TimeSpan LeaseTimeout { get; set; }

        protected abstract string License { get; set; }

        public IDictionary<string, string> LicenseAttributes { get; private set; }

        public Rhino.Licensing.LicenseType LicenseType { get; private set; }

        public string Name { get; private set; }

        public Guid UserId { get; private set; }
    }
}
