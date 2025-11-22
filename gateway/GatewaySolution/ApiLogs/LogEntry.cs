namespace Gateway_ocelot_Solution.ApiLogs
{
    public class LogEntry
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Level { get; set; } = "Information";
        public string Message { get; set; }
        public string? Method { get; set; }
        public string? Path { get; set; }
        public int? StatusCode { get; set; }
        public object? Request { get; set; }
        public object? Response { get; set; }
        public double RequestSizeKb { get; set; } = 0;
        public double ResponseSizeKb { get; set; } = 0;
        public string? Exception { get; set; }
    }
}
