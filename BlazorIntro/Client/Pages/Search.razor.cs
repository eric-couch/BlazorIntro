using BlazorIntro.Client.Services;
using BlazorIntro.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http.Json;

namespace BlazorIntro.Client.Pages
{
    public partial class Search
    {
        [Inject]
        HttpClient Http { get; set; } = new();
        private string SearchTitle = String.Empty;
        private List<MovieSearchResultItems> OMDBMovies = new();

        IQueryable<MovieSearchResultItems> movies { get; set; }

        private readonly string OMDBAPIUrl = "https://www.omdbapi.com/?apikey=";
        private readonly string OMDBAPIKey = "86c39163";
        private int pageNum = 1;
        string message = string.Empty;

        private async Task SearchOMBD()
        {
            //MovieSearchResult? movieResult = await Http.GetFromJsonAsync<MovieSearchResult>($"{OMDBAPIUrl}{OMDBAPIKey}&s={SearchTitle}&p={pageNum}");
            //if (movieResult is not null)
            //{
            //    //OMDBMovies = movieResult.Search.ToList();
            //    movies = movieResult.Search.AsQueryable();
            //}
            await GetMovies();
        }

        private async Task NextPage()
        {
            pageNum++;
            await GetMovies();
        }

        private async Task GetMovies()
        {
            MovieSearchResult? movieResult = await Http.GetFromJsonAsync<MovieSearchResult>($"{OMDBAPIUrl}{OMDBAPIKey}&s={SearchTitle}&page={pageNum}");
            if (movieResult is not null)
            {
                //OMDBMovies = movieResult.Search.ToList();
                movies = movieResult.Search.AsQueryable();
            }
        }

        private async void AddMovie(MovieSearchResultItems m)
        {
            Movie newMovie = new() { imdbId = m.imdbID };
            var res = await Http.PostAsJsonAsync("api/add-movie", newMovie);
            if (!res.IsSuccessStatusCode)
            {
                toastService.ShowToast($"Adding movie {m.Title} Failed!", ToastLevel.Error, 5000);
            } else
            {
                toastService.ShowToast($"Added movie {m.Title} successfully!", ToastLevel.Success, 5000);
            }
        }
    }
}
