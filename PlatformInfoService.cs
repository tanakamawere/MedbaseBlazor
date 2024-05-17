using MedbaseLibrary.Services;

namespace MedbaseBlazor
{
    public class PlatformInfoService : IPlatformInfoService
    {
        public bool IsMaui()
        {
            return false;
        }
    }
}
