namespace HostLibrary.Helpers
{
    public class JwtSection
    {
        public string? Key { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public int ExpiryInMinutes { get; set; } = 60;
    }
}
