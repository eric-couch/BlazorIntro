using Microsoft.AspNetCore.Components;
using BlazorIntro.Shared;

namespace BlazorIntro.Client.Pages
{
    public partial class MovieDetails
    {
        [Parameter]
        public OMDBMovie? Movie { get; set; }
    }
}
