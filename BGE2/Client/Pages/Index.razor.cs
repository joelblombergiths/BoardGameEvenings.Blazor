using BGE2.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace BGE2.Client.Pages;

public partial class Index : ComponentBase
{
    private List<EventDto>? AllEvents;

    private EditEventDto SelectedEvent = new();
    private string? selectedId;

    private string ResultMessage = string.Empty;
    private string AlertClass = "d-none";

    private UserDto? user = new();
    private bool IsAdmin() => user?.Role.ToLower().Equals("admin") ?? false;

    [CascadingParameter]
    public Task<AuthenticationState> AuthState { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await LoadEvents();
    }

    protected override async Task OnParametersSetAsync()
    {
        AuthenticationState authState = await AuthState;
        if (authState.User.Identity?.IsAuthenticated ?? false)
        {
            string? email = authState.User.FindFirst(c => c.Type == "preferred_username")?.Value;
            if (!string.IsNullOrEmpty(email))
            {
                user = await _Client.GetFromJsonAsync<UserDto>($"user/{email}");
                Console.WriteLine($"Role: {user?.Role}");
            }
        }
    }

    private async Task LoadEvents()
    {
        AllEvents = await _PublicClient.Client.GetFromJsonAsync<List<EventDto>>("Event/All") ?? new();
    }

    private void SelectEvent(string id)
    {
        EventDto? eventDto = AllEvents?.FirstOrDefault(e => e.Id == id);
        if (eventDto == null)
        {
            _Navigation.NavigateTo("/");
            return;
        }

        SelectedEvent = new()
        {
            Name = eventDto.Name,
            Date = DateTime.Parse(eventDto.Date)
        };
        selectedId = id;
    }

    private async Task DeleteEvent()
    {
        HttpResponseMessage res =
            await _Client.DeleteAsync($"Event/{selectedId}");

        await ShowChanges(res.IsSuccessStatusCode, "Deleted");
    }

    private async Task SaveChanges()
    {
        HttpResponseMessage res =
            await _Client.PutAsJsonAsync($"Event/{selectedId}", SelectedEvent);

        await ShowChanges(res.IsSuccessStatusCode, "Updated");
    }

    private async Task ShowChanges(bool success, string action)
    {
        ResultMessage = success ? $"Event {action} Successfully!" : "An Error Occured!";
        AlertClass = $"alert alert-{(success ? "success" : "danger")}";
        StateHasChanged();

        await LoadEvents();

        await Task.Delay(2000);
        AlertClass = "d-none";
        ResultMessage = string.Empty;
    }
}
