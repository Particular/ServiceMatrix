namespace NServiceBusStudio
{
    public class ApplicationIsDirty
    {
        public static void SetTrue()
        {
            Application.ResetIsDirtyFlag();
        }
    }
}
