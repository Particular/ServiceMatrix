using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
