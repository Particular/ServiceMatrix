namespace NServiceBusStudio.Core
{
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio;

	public static class VsShellExtensions
	{
		/// <summary>
		/// Gets the current running VS hive, such as "10.0" or "10.0Exp".
		/// </summary>
		public static string GetHive(this IVsShell shell)
		{
			object value;
			var hive = "10.0";
			if (ErrorHandler.Succeeded(shell.GetProperty((int)__VSSPROPID.VSSPROPID_VirtualRegistryRoot, out value)))
				hive = ((string)value).Replace(@"Software\Microsoft\VisualStudio\", string.Empty);

			return hive;
		}
	}
}
