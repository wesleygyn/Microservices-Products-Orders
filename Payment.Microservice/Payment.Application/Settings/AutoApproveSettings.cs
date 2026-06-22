namespace Payment.Application.Settings
{
    public class AutoApproveSettings
    {
        public bool Enabled { get; set; } = false;
        public int DelaySeconds { get; set; } = 5;
    }
}