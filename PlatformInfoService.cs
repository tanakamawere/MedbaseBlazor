using MedbaseLibrary.Services;

namespace MedbaseBlazor
{
    public class PlatformInfoService : IPlatformInfoService
    {
        public bool IsMauiHybrid => throw new NotImplementedException();

        public bool IsMaui()
        {
            return false;
        }
    }
}
