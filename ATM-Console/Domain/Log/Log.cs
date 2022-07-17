namespace AtmConsole.Domain.Logs
{
    public class Log
    {
        public DateTime? Date { get; }
        public string? Notes { get; }

        public Log(string notes, DateTime date)
        {
            Notes = notes;
            Date = date;
        }
    }
}
