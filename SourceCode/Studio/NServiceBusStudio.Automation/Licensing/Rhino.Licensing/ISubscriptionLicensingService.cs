namespace Rhino.Licensing
{
    using System;
    using System.ServiceModel;

    [ServiceContract]
    public interface ISubscriptionLicensingService
    {
        [OperationContract]
        string LeaseLicense(string previousLicense);
    }
}
