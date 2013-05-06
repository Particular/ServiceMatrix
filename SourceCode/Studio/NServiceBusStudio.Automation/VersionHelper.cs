using NuPattern.Diagnostics;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;


namespace NServiceBusStudio.Core
{
	/// <summary>
	/// Helper for toolkit versioning.
	/// </summary>
	public class VersionHelper
	{
		public const string TargetsFilename = "NServiceBus.ToolkitVersion.targets";
		private const string MSBuildNamespace = "http://schemas.microsoft.com/developer/msbuild/2003";

		private ITracer tracer;
		private string vsHive;

		public VersionHelper(ITracer tracer, string vsHive)
		{
			this.tracer = tracer;
			this.vsHive = vsHive;
		}

		public static void SyncTargets(ITracer tracer, string vsHive)
		{
			new VersionHelper(tracer, vsHive).SyncTargets();
		}

		/// <summary>
		/// Synchronizes the targets file on disk with targets file in this version of the toolkit.
		/// </summary>
		public void SyncTargets()
		{
			var targetPath = Environment.ExpandEnvironmentVariables(Path.Combine(
				@"%LocalAppData%\Microsoft\MSBuild\NServiceBus\", ToolkitConstants.Author, ToolkitConstants.ToolkitName, TargetsFilename));

			try
			{
				// Does the file exist where it needs to be ?
				if (File.Exists(targetPath))
				{
					// Load file as XML
					var version = GetTargetsVersion(targetPath);
					if (version != null && !version.Equals(new Version(ToolkitConstants.Version)))
					{
						WriteFile(targetPath);
					}
				}
				else
				{
					WriteFile(targetPath);
				}
			}
			catch (Exception ex)
			{
				tracer.Error(ex, ex.Message);
			}
		}

		private Version GetTargetsVersion(string targetsPath)
		{
			var document = XDocument.Load(targetsPath);
			var ns = XNamespace.Get(MSBuildNamespace);

			var versionElement = document.Descendants(ns + "NServiceBusStudioVersion").FirstOrDefault();
			if (versionElement != null)
			{
				return new Version((string)versionElement.Value);
			}

			return null;
		}

		private void WriteFile(string targetPath)
		{
			// Ensure directory exists
			var targetsFolder = Path.GetDirectoryName(targetPath);
			if (!Directory.Exists(targetsFolder))
			{
				Directory.CreateDirectory(targetsFolder);
			}

			var sourcePath = Environment.ExpandEnvironmentVariables(Path.Combine(
				@"%LocalAppData%\Microsoft\VisualStudio", vsHive, "Extensions", ToolkitConstants.Author,
				ToolkitConstants.ToolkitName, ToolkitConstants.Version, TargetsFilename));

			File.Copy(sourcePath, targetPath, true);
		}
	}
}
