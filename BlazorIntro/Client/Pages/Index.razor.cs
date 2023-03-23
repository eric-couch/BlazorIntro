using BlazorIntro.Shared;
using BlazorIntro.Client.HttpRepository;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace BlazorIntro.Client.Pages
{
    public partial class Index
    {
        //[Inject]
        //public HttpClient Http { get; set; } = new();
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject]
        public IUserMoviesHttpRepository UserMoviesHttpRepository { get; set; }
        public List<OMDBMovie> MovieDetails { get; set; } = new();
        public OMDBMovie MovieAllDetails { get; set; } = new();
        public UserDto User { get; set; } = new UserDto();
        
        private string message = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
            if (UserAuth is not null && UserAuth.IsAuthenticated)
            {
                MovieDetails = await UserMoviesHttpRepository.GetMovies();
            }
        }

        private async Task ShowMovieDetails(OMDBMovie movie)
        {
            MovieAllDetails = movie;
            await Task.CompletedTask;
        }
        
        private async Task RemoveFavoriteMovie(OMDBMovie movie)
        {
            //await Http.PostAsJsonAsync("api/remove-movie", movie.imdbID);
            await UserMoviesHttpRepository.RemoveMovie(movie.imdbID);
            MovieDetails.Remove(movie);
            StateHasChanged();
            await Task.CompletedTask;
        }
    }
}
