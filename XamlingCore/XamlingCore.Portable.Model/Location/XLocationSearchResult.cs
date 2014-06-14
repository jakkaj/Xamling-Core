namespace XamlingCore.Portable.DTO.Location
{
    public class XLocationSearchResult<T>
    {
        private readonly T _result;
        private readonly XLocationSearchStatus _status;

        public XLocationSearchResult(T result, XLocationSearchStatus status = XLocationSearchStatus.Okay)
        {
            _result = result;
            _status = status;
        }

        public T Result
        {
            get { return _result; }
        }

        public XLocationSearchStatus Status
        {
            get { return _status; }
        }
    }

    public enum XLocationSearchStatus
    {
        Okay, 
        NoLocationFound,
        LocationDisabled
    }
}
