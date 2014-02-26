using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SignVSIX
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("usage: SignVSIX.exe {VSIXPATH} {CertificatePath} [CertificatePassword]");
                return;
            }

            var vsixPath = args[0];
            var certificatePath = args[1];
            var certificatePassword = default(string);

            if (args.Length >= 3)
            {
                certificatePassword = args[2];
            }

            var result = new SignVSIX()
            {
                VSIXPath = vsixPath,
                CertificatePath = certificatePath,
                CertificatePassword = certificatePassword
            }.Execute();

            if (result)
            {
                Console.WriteLine("Sign VSIX successfully.");
            }
            else
            {
                Console.WriteLine("Sign VSIX failed.");
            }

        }

        
    }
}
