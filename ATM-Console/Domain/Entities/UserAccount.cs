namespace AtmConsole.Domain.Entities
{
    public class UserAccount
    {
        public decimal AccountBalance { get; set; }
        public long AccountNumber { get; set; }
        public long CardNumber { get; set; }
        public int CardPin { get; set; }
        public string? FullName { get; set; }
        public int Id { get; set; }
        public bool IsLocked { get; set; }
        public int TotalLogin { get; set; }
    }
}
