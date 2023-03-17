using BlazorIntro.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorIntro.Client.Pages
{
    public partial class Search
    {
        [Inject]
        HttpClient Http { get; set; } = new();
        private string SearchTitle = String.Empty;
        private List<MovieSearchResultItems> OMDBMovies = new();
        
        private readonly string OMDBAPIUrl = "https://www.omdbapi.com/?apikey=";
        private readonly string OMDBAPIKey = "86c39163";

        private async Task SearchOMBD()
        {
            MovieSearchResult? movieResult = await Http.GetFromJsonAsync<MovieSearchResult>($"{OMDBAPIUrl}{OMDBAPIKey}&s={SearchTitle}");
            if (movieResult is not null)
            {
                OMDBMovies = movieResult.Search.ToList();
            }
        }
    }
}
