namespace AtmConsole.Domain.Users
{
    public class User
    {
        public string? Address;
        public int? Age;
        public string? City;
        public string? Country;
        public string FullName;
        public string? Email;
        public string? Phone;
        public string? PostalCode;
        public long UserId;

        public User(long userId, string fullName)
        {
            UserId = userId;
            FullName = fullName;
        }
    }
}
