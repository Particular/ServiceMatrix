using NuPattern.Library.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuPattern.Library.Commands
{
    public class GenerateProductCodeCommandCustom : GenerateProductCodeCommand
    {
        public override void Execute()
        {
            try
            {
                base.Execute();
            }
            catch (IOException) { }
        }
    }
}
