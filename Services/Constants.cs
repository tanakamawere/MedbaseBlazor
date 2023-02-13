namespace MedbaseBlazor.Services
{
    public class Constants
    {
        public static double WeeklyAmount() => 1500;
        public static double MonthlyAmount() => 3500;

        public static Dictionary<string, int> SearchCategories = new()
        {
            {"Search by Id", 1},
            {"Search by Keywords", 2 }
        };
    }
}
