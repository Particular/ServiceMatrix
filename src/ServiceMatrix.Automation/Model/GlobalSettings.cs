namespace NServiceBusStudio.Automation.Model
{
    public class GlobalSettings 
    {
        static GlobalSettings instance = new GlobalSettings();
        bool isLicenseValid = true;

        public bool IsLicenseValid
        {
            get { return isLicenseValid; }
            set
            {
                isLicenseValid = value;
            }
        }

        public static GlobalSettings Instance
        {
            get
            {
                return instance;
            }   
        }
    }
}
