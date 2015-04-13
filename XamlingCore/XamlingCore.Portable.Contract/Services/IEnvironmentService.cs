namespace XamlingCore.Portable.Contract.Services
{
    public interface IEnvironmentService
    {
        string GetOSVersion();
        int GetScreenWidth();
        int GetScreenHeight();
        float GetScreenScale();
        string GetAppVersion();
    }        
}