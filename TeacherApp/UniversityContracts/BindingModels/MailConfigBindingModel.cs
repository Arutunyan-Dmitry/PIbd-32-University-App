namespace UniversityContracts.BindingModels
{
    public class MailConfigBindingModel
    {
        public string MailLogin { get; set; }
        public string MailPassword { get; set; }
        public string SmtpClientHost { get; set; }
        public int SmtpClientPort { get; set; }
        public string PopHost { get; set; }
        public int PopPort { get; set; }
    }
}
