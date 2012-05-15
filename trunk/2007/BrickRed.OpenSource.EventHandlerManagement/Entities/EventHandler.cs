using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrickRed.OpenSource.EventHandlerManagement
{
    class EventHandler
    {
        string strEventType;
        string strAssemblyInfo;
        string strClassName;
        Guid defId;

        public string CLASSNAME
        {
            get { return strClassName; }
            set { strClassName = value; }
        }

        public string ASSEMBLYINFO
        {
            get { return strAssemblyInfo; }
            set { strAssemblyInfo = value; }
        }

        public string EVENTTYPE
        {
            get { return strEventType; }
            set { strEventType = value; }
        }
        public Guid DEFID
        {
            get { return defId; }
            set { defId = value; }
        }
    }
}
