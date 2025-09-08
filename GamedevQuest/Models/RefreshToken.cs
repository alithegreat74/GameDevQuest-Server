namespace GamedevQuest.Models
{
    public class RefreshToken
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public string Token {  get; private set; }
        public string IpAddress { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public RefreshToken(int userId, string token, string ipAddress, DateTime createdAt, DateTime expiresAt) 
        {
            this.UserId = userId;
            this.CreatedAt = createdAt;
            this.ExpiresAt = expiresAt;
            this.Token = token;
            this.IpAddress = ipAddress;
        }

    }
}
