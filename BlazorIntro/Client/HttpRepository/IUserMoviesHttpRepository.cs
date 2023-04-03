using BlazorIntro.Shared;
using BlazorIntro.Shared.Wrappers;

namespace BlazorIntro.Client.HttpRepository;

public interface IUserMoviesHttpRepository
{
    Task<DataResponse<List<OMDBMovie>>> GetMovies();
    //Task<MovieSearchResult> SearchMovies(string title, int page);
    //Task<bool> AddMovie(string imdbId);
    Task<bool> RemoveMovie(string imdbId);
}
