namespace Domain.Common
{
    public class DataProvider
    {
        public static string NewId() => MassTransit.NewId.Next().ToString();
        public static DateTime Now() => DateTime.Now;
    }
}
