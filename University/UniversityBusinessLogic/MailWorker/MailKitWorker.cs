using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Text;
using UniversityContracts.BindingModels;

namespace UniversityBusinessLogic.MailWorker
{
    public class MailKitWorker : AbstractMailWorker
    {
        public MailKitWorker()
        {

        }
        protected override async Task SendMailAsync(MailSendInfoBindingModel info)
        {
            using var objMailMessage = new MailMessage();
            using var objSmtpClient = new SmtpClient(_smtpClientHost, _smtpClientPort);
            try
            {
                objMailMessage.From = new MailAddress(_mailLogin);
                objMailMessage.To.Add(new MailAddress(info.MailAddress));
                objMailMessage.Subject = info.Subject;
                objMailMessage.Body = info.Text;
                objMailMessage.SubjectEncoding = Encoding.UTF8;
                objMailMessage.BodyEncoding = Encoding.UTF8;

                Attachment data = new Attachment(info.FilePath, MediaTypeNames.Application.Octet);
                ContentDisposition disposition = data.ContentDisposition;
                disposition.CreationDate = System.IO.File.GetCreationTime(info.FilePath);
                disposition.ModificationDate = System.IO.File.GetLastWriteTime(info.FilePath);
                disposition.ReadDate = System.IO.File.GetLastAccessTime(info.FilePath);
                objMailMessage.Attachments.Add(data);

                objSmtpClient.UseDefaultCredentials = false;
                objSmtpClient.EnableSsl = true;
                objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                objSmtpClient.Credentials = new NetworkCredential(_mailLogin, _mailPassword);
                await Task.Run(() => objSmtpClient.Send(objMailMessage));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
