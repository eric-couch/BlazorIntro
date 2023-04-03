using Microsoft.AspNetCore.Components;
using BlazorIntro.Shared;

namespace BlazorIntro.Client.Pages
{
    public partial class MovieDetails
    {
        [Parameter]
        public OMDBMovie? Movie { get; set; }
        [Parameter]
        public EventCallback<OMDBMovie> OnRemoveFavoriteMovie { get; set; }
        private async Task RemoveFavoriteMovie(OMDBMovie Movie)
        {
            await OnRemoveFavoriteMovie.InvokeAsync(Movie);
        }
    }
}
