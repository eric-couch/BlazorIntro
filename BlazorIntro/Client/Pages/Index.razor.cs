using BlazorIntro.Shared;
using BlazorIntro.Client.HttpRepository;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using BlazorIntro.Shared.Wrappers;

namespace BlazorIntro.Client.Pages
{
    public partial class Index
    {
        [Inject]
        public HttpClient Http { get; set; } = new();
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject]
        public IUserMoviesHttpRepository UserMoviesHttpRepository { get; set; }
        public List<OMDBMovie> MovieDetails { get; set; } = new();
        public OMDBMovie MovieAllDetails { get; set; } = new();
        public UserDto User { get; set; } = new UserDto();
        public int numCols = 6;
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
                if (UserAuth is not null && UserAuth.IsAuthenticated)
                {
                    //MovieDetails = await UserMoviesHttpRepository.GetMovies();
                    DataResponse<List<OMDBMovie>> dataResponse = await UserMoviesHttpRepository.GetMovies();
                    if (dataResponse.Succeeded)
                    {
                        MovieDetails = dataResponse.Data;
                    } else
                    {
                        toastService.ShowToast($"Error: {dataResponse.Message}", Services.ToastLevel.Error, 5000);
                    }
                }
            } catch
            {
                toastService.ShowToast("Retrieving favorite movie list failed!", Services.ToastLevel.Error, 5000);
            }
            
        }

        private async Task ShowMovieDetails(OMDBMovie movie)
        {
            MovieAllDetails = movie;
            await Task.CompletedTask;
        }
        
        private async Task RemoveFavoriteMovie(OMDBMovie movie)
        {
            await UserMoviesHttpRepository.RemoveMovie(movie.imdbID);
            MovieDetails.Remove(movie);
            StateHasChanged();
            await Task.CompletedTask;
        }

        
    }
}
