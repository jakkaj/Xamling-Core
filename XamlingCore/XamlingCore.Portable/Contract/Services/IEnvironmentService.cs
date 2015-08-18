namespace XamlingCore.Portable.Contract.Services
{
    public interface IEnvironmentService
    {
        string GetOSVersion();
        double GetScreenWidth();
        double GetScreenHeight();
        float GetScreenScale();
        string GetAppVersion();
    }        
}