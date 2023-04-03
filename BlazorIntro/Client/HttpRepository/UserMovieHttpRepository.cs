using BlazorIntro.Client.Pages;
using BlazorIntro.Shared;
using BlazorIntro.Shared.Wrappers;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;

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
    
    
    public async Task<DataResponse<List<OMDBMovie>>> GetMovies()
    {
        try
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
                return new DataResponse<List<OMDBMovie>>()
                {
                    Data = MovieDetails,
                    Message = "Success",
                    Succeeded = true
                };
            }
            return new DataResponse<List<OMDBMovie>>();
        }
        catch (HttpRequestException ex)
        {
            if (ex.Message.Contains(HttpStatusCode.Unauthorized.ToString())) {
                return new DataResponse<List<OMDBMovie>>()
                {
                    Errors = new Dictionary<string, string[]> { { "Not Authroized", new string[] { "User does not have authroization." } } },
                    Succeeded = false,
                    Message = "Not Authroized",
                    Data = new List<OMDBMovie>()
                };
            }
            return new DataResponse<List<OMDBMovie>>()
            {
                Errors = new Dictionary<string, string[]> { { "Connection Issue", new string[] { "There was an issue connecting to the server." } } },
                Succeeded = false,
                Message = "Connection Issue",
                Data = new List<OMDBMovie>()
            };
        }
        catch (NotSupportedException)
        {
            return new DataResponse<List<OMDBMovie>>()
            {
                Errors = new Dictionary<string, string[]> { { "Not Supported", new string[] { "This content type is not support." } } },
                Succeeded = false,
                Message = "Not Supported",
                Data = new List<OMDBMovie>()
            };
        }
        catch (JsonException)
        {
            return new DataResponse<List<OMDBMovie>>()
            {
                Errors = new Dictionary<string, string[]> { { "Invalid Json", new string[] { "The JSON is invalid." } } },
                Succeeded = false,
                Message = "Invalid Json",
                Data = new List<OMDBMovie>()
            };
        }
        catch (Exception ex)
        {
            return new DataResponse<List<OMDBMovie>>()
            {
                Errors = new Dictionary<string, string[]> { { ex.Message, new string[] { ex.Message } } },
                Succeeded = false,
                Message = ex.Message,
                Data = new List<OMDBMovie>()
            };
        }

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
