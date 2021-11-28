using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiFCVS.Api.Configuration
{
    public static class ModoExecusao
    {
        public static bool IsDebug
        {
            get
            {
                bool isDebug = false;
#if DEBUG
       isDebug = true;
#endif
                return isDebug;
            }
        }
    }
}
