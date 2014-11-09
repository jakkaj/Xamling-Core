using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlingCore.Portable.Model.Location
{
    public class XCompass
    {
        public double Heading { get; set; }
        public double Accuracy { get; set; }
        public bool NeedsCalibration { get; set; }
        public bool IsValid { get; set; }
        public bool IsSupported { get; set; }
    }
}
