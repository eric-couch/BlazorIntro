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
        public UserDto User { get; set; } = new UserDto();
        private readonly string OMDBAPIUrl = "https://www.omdbapi.com/?apikey=";
        private readonly string OMDBAPIKey = "";
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
    }
}
