

using GPMuseumify.DAL.Models;
using System.Threading.Tasks;

namespace GPMuseumify.DAL.Repositories;

public interface IUserHistoryRepository
{
    Task <IReadOnlyList<UserHistory>> GetUserHistoryAsync(Guid userId, int skip, int take);// يقرا التاريخ بتاع اليوزر
    Task<int> CountByUserIdAsync(Guid userId);// يعدي عدد العناصر في التاريخ بتاع اليوزر
    Task<UserHistory> AddAsync(UserHistory history); // يضيف عنصر جديد في التاريخ بتاع اليوزر
    Task<UserHistory?> GetByIdWithDetailsAsync(Guid id);// يجيب عنصر من التاريخ بتاع اليوزر مع التفاصيل بتاعته

}
