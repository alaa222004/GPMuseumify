
using GPMuseumify.DAL.Models;

namespace GPMuseumify.DAL.Repositories;

public interface ISearchRepository
{
    Task<IReadOnlyList<Statue>> SearchStatuesAsync(string query, int skip, int take);// el query eli b3tho el user 3lshan adwar 3la statues , el skip w el take 3lshan a3ml pagination
    Task<IReadOnlyList<Museum>> SearchMuseumsAsync(string query, int skip, int take);
    Task<int> CountStatuesAsync(string query);// 3lshan a3rf kam statue la2et 3la el query eli b3tho el user
    Task<int> CountMuseumsAsync(string query);
    Task<IReadOnlyList<Statue>>GetPopularStatuesAsync(int count);// 3lshan a3ml fetch l aktr statues popularity
    Task<IReadOnlyList<Museum>>GetPopularMuseumsAsync(int count);

}
