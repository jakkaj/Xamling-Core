using XamlingCore.iOS.Unified.Glue;

namespace XamlingCore.Tests.iOS.Glue
{
    public class ProjectGlue : iOSGlue
    {
        public override void Init()
        {
            base.Init();

            

            

            Container = Builder.Build();

        }
    }
}