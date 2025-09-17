using Quiz.Common.Models;

namespace Quiz.Common.Interfaces
{
    public interface IErrorLoggerService
    {
        Task LogAsync(ErrorLog log);
    }
}
