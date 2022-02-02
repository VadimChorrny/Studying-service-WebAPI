using System.Threading.Tasks;
using DTO;

namespace BLL.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailDto mailRequest);

        Task SendInfoAboutStartStudyEmailAsync(StartStudyMailDto request);

        Task SendWelcomeEmailAsync(WelcomeMailDto request);
    }
}