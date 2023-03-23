using BlazorIntro.Client.Pages;
using BlazorIntro.Shared;
using System.Net.Http.Json;
using System.Net;


namespace BlazorIntro.Client.HttpRepository;

public class UserMoviesHttpRepository : IUserMoviesHttpRepository
{
    public readonly HttpClient _httpClient;
    private readonly string OMDBAPIUrl = "https://www.omdbapi.com/?apikey=";
    private readonly string OMDBAPIKey = "86c39163";


    public UserMoviesHttpRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    
    public async Task<List<OMDBMovie>> GetMovies()
    {
        var MovieDetails = new List<OMDBMovie>();
        UserDto? User = await _httpClient.GetFromJsonAsync<UserDto>("api/get-movies");
        if (User?.FavoriteMovies?.Any() ?? false)
        {
            foreach (var movie in User.FavoriteMovies)
            {
                var movieDetails = await _httpClient.GetFromJsonAsync<OMDBMovie>($"{OMDBAPIUrl}{OMDBAPIKey}&i={movie.imdbId}");
                MovieDetails.Add(movieDetails!);
            }
            return MovieDetails;
        }
        return new List<OMDBMovie>();

    }
    //Task<MovieSearchResult> SearchMovies(string title, int page);
    //Task<bool> AddMovie(string imdbId);
    public async Task<bool> RemoveMovie(string imdbId)
    {
        Movie newMovie = new Movie { imdbId = imdbId };
        var res = await _httpClient.PostAsJsonAsync("api/remove-movie", newMovie);
        if (!res.IsSuccessStatusCode)
        {
            throw new ApplicationException(await res.Content.ReadAsStringAsync());
        }
        return true;
    }
}
