using System;

namespace XamlingCore.Portable.Util.Util
{
    public static class DateUtil
    {
        public static DateTime TimeCodeToDateTime(long timecode)
        {
            var epoc = new DateTime(1970, 1, 1);
            var added = epoc.AddSeconds(Convert.ToInt32(timecode));

            return added;
        }
    }
}
