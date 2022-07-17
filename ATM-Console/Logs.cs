namespace AtmConsole
{
    public class Logs
    {
        public string? Notes { get; }
        public DateTime? Date { get; }

        public Logs(string notes, DateTime date)
        {
            Notes = notes;
            Date = date;
        }
    }
}
