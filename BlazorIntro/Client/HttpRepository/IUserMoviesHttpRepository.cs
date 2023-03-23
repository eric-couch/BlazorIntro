using BlazorIntro.Shared;

namespace BlazorIntro.Client.HttpRepository;

public interface IUserMoviesHttpRepository
{
    Task<List<OMDBMovie>> GetMovies();
    //Task<MovieSearchResult> SearchMovies(string title, int page);
    //Task<bool> AddMovie(string imdbId);
    Task<bool> RemoveMovie(string imdbId);
}
