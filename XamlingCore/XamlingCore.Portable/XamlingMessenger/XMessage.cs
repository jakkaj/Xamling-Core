using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlingCore.Portable.XamlingMessenger
{
    public class XMessage
    {
    }

    public static class XMessageExtension
    {
        public static void Send(this XMessage message)
        {
            XMessenger.Default.Send(message);
        }
    }
}
