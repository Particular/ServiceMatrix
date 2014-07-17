using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace NServiceBusStudio.Automation.Util
{
    internal class IniReader
    {
        private readonly string path;

        public IniReader(string path)
        {
            this.path = path;
        }

        public IReadOnlyCollection<string> GetCategories()
        {
            const int length = 0x10000;
            return GetMultipleEntries(null, length);
        }

        public IReadOnlyCollection<string> GetKeys(string category)
        {
            const int length = 0x8000;
            return GetMultipleEntries(category, length);
        }

        public string GetValue(string section, string key)
        {
            const int length = 0xFF;
            var returnString = new string(' ', length);
            GetPrivateProfileString(section, key, null, returnString, length, path);
            return returnString.Split('\0').FirstOrDefault();
        }

        private IReadOnlyCollection<string> GetMultipleEntries(string category, int length)
        {
            var returnString = new string(' ', length);
            GetPrivateProfileString(category, null, null, returnString, length, path);
            var result = new List<string>(returnString.Split('\0'));
            result.RemoveRange(result.Count - 2, 2);
            return result;
        }

        [DllImport("KERNEL32.DLL", EntryPoint = "GetPrivateProfileStringW",
          SetLastError = true,
          CharSet = CharSet.Unicode, ExactSpelling = true,
          CallingConvention = CallingConvention.StdCall)]
        private static extern int GetPrivateProfileString(
          string lpAppName,
          string lpKeyName,
          string lpDefault,
          string lpReturnString,
          int nSize,
          string lpFilename);
    }
}
