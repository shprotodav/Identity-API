namespace TestTask.Identity.Common
{
    public class AppSettings
    {
        public string ConnectionString { get; set; }
        public string KeyVaultDomain { get; set; }
        public string AppId { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Token { get; set; }
    }
}
