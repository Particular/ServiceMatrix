namespace NServiceBusStudio.Automation.Dialog
{
    public interface IEndpointPicker : IServicePicker
    {
        string ComponentName { get; set; }
    }
}
