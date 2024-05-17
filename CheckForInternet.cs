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
}
