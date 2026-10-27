namespace BaseLibrary.Entities
{
    public class RefreshTokenInfo
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}
