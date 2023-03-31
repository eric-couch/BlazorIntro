using BlazorIntro.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorIntro.Client.Pages.UserAdmin;

public partial class UserList
{
    [Inject]
    HttpClient Http { get; set; } = new();
    private List<UserDto> users = new();

    protected override async Task OnInitializedAsync()
    {
        users = await Http.GetFromJsonAsync<List<UserDto>>("api/admin");
    }
}
