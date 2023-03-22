using BlazorIntro.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace BlazorIntro.Client.Pages
{
    public partial class Index
    {
        [Inject]
        public HttpClient Http { get; set; } = new();
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        public List<OMDBMovie> MovieDetails { get; set; } = new();
        public OMDBMovie MovieAllDetails { get; set; } = new();
        public UserDto User { get; set; } = new UserDto();
        private readonly string OMDBAPIUrl = "https://www.omdbapi.com/?apikey=";
        private readonly string OMDBAPIKey = "86c39163";
        private string message = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
            if (UserAuth is not null && UserAuth.IsAuthenticated)
            {
                User = await Http.GetFromJsonAsync<UserDto>("api/get-movies");
                foreach (var movie in User.FavoriteMovies)
                {
                    var movieDetails = await Http.GetFromJsonAsync<OMDBMovie>($"{OMDBAPIUrl}{OMDBAPIKey}&i={movie.imdbId}");
                    MovieDetails.Add(movieDetails);
                }
            }
        }

        private async Task ShowMovieDetails(OMDBMovie movie)
        {
            MovieAllDetails = movie;
            await Task.CompletedTask;
        }



        //CheckAdminStatus()
        private async Task GetRoles()
        {
            string roles = string.Empty;
            var ret = await Http.GetAsync("api/get-roles");
            //foreach (var item in ret.Content)
            //{
            //    roles += $"{item}<br />";
            //}
            message = ret.Content.ReadAsStringAsync().Result;
        }

        private async Task RemoveFavoriteMovie(OMDBMovie movie)
        {
            await Http.PostAsJsonAsync("api/remove-movie", movie);
            MovieDetails.Remove(movie);
            StateHasChanged();
            await Task.CompletedTask;
        }
    }
}
