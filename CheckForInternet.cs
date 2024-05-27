using MedbaseLibrary.Services;

namespace MedbaseBlazor
{
    public class CheckForInternet : ICheckForInternet
    {
        public bool InternetAvailable()
        {
            return true;
        }
    }
    public class PlatformInfoService : IPlatformInfoService
    {
        public bool IsMauiHybrid => throw new NotImplementedException();

        public bool IsMaui()
        {
            return false;
        }
    }
}
