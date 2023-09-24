namespace ETE.Identidade.API.Extensions
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int ExpiryHours { get; set; }
        public string Sender { get; set; }
        public string ValidatedIn { get; set; }
    }
}
