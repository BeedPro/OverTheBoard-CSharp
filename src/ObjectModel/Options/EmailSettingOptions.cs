namespace OverTheBoard.ObjectModel.Options
{
    public class EmailSettingOptions
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Sender { get; set; }
        public bool UseSmtpServer { get; set; }
        public string SpecifiedPickupDirectory { get; set; }
    }
}