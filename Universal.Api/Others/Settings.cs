namespace Universal.Api
{
    public class Settings
    {
        public int JwtExpiresIn { get; set; }
        public string JwtSecret { get; set; }
        public string SqldbConnString { get; set; }
        public string StorageConnString { get; set; }
        public string ServiceBusConnString { get; set; }
    }
}
