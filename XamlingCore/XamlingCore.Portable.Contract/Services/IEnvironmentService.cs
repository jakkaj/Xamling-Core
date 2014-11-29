namespace XamlingCore.iOS.Implementations
{
    public interface IEnvironmentService
    {
        string GetOSVersion();
        int GetScreenWidth();
        int GetScreenHeight();
        float GetScreenScale();
    }        
}