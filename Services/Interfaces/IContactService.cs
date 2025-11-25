using System.Threading.Tasks;

namespace Wedding_Planner.Application.Services.Interfaces
{
    public interface IContactService
    {
        Task SubmitContactFormAsync(string name, string email, string subject, string message);
    }
}
