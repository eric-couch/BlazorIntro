using BlazorIntro.Client.Services;
using BlazorIntro.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Net.Http.Json;

namespace BlazorIntro.Client.Pages
{
    public partial class Search
    {
        [Inject]
        HttpClient Http { get; set; } = new();
        private string SearchTitle = String.Empty;
        private List<MovieSearchResultItems>? OMDBMovies = null;

        IQueryable<MovieSearchResultItems>? movies { get; set; } = null;

        private readonly string OMDBAPIUrl = "https://www.omdbapi.com/?apikey=";
        private readonly string OMDBAPIKey = "86c39163";
        private int pageNum = 1;
        private int totalPages = 2;
        private int totalResults = 0;
        string message = string.Empty;
        private async Task SearchOMDB()
        {
            await GetMovies();
            StateHasChanged();
        }

        private async Task NextPage()
        {
            if (pageNum < totalPages)
            {
                pageNum++;
                await GetMovies();
            }
        }

        private async Task PreviousPage()
        {
            if (pageNum > 1)
            {
                pageNum--;
                await GetMovies();
            }
        }

        private async Task GetMovies()
        {
            MovieSearchResult? movieResult = await Http.GetFromJsonAsync<MovieSearchResult>($"{OMDBAPIUrl}{OMDBAPIKey}&s={SearchTitle}&page={pageNum}");
            if (movieResult is not null)
            {
                movies = movieResult.Search.AsQueryable();
                
                if (Double.TryParse(movieResult.totalResults, out double total))
                {
                    totalResults = (int)total;
                    totalPages = (int)Math.Ceiling(total / 10);
                } else
                {
                    totalPages = 1;
                }
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
