using System.Net.Http.Json;
using BGE2.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BGE2.Client.Pages;

public partial class Join : ComponentBase
{
    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; }

    [Parameter]
    public string? Id { get; set; }

    private EventDto? SelectedEvent = new();

    private List<string> BoardGames = new();

    private List<AttendeeDto>? eventAttendees;
    private AttendeeDto addAttendee = new();
    private int selectedAttendee;

    private string ResultMessage = string.Empty;
    private string AlertClass = "d-none";

    private UserDto? user = new();

    protected override async Task OnInitializedAsync()
    {
        if (string.IsNullOrEmpty(Id))
        {
            _Navigation.NavigateTo("/");
            return;
        }

        await LoadEventDetails();

        BoardGames = await _PublicClient.Client.GetFromJsonAsync<List<string>>("Game/All") ?? new();
    }

    protected override async Task OnParametersSetAsync()
    {
        AuthenticationState a = await AuthState;
        if (a.User.Identity?.IsAuthenticated ?? false)
        {
            string? email = a.User.FindFirst(c => c.Type == "preferred_username")?.Value;
            if (!string.IsNullOrEmpty(email))
            {
                user = await _Client.GetFromJsonAsync<UserDto>($"user/{email}");
                addAttendee.Name = a.User.Identity?.Name ?? string.Empty;
                Console.WriteLine($"Role: {user?.Role}");
            }
        }
    }

    private bool IsAdmin()
    {
        return user?.Role.ToLower().Equals("admin") ?? false;
    }

    private async Task LoadEventDetails()
    {
        SelectedEvent = await _Client.GetFromJsonAsync<EventDto>($"Event/{Id}");
        if (SelectedEvent == null)
        {
            _Navigation.NavigateTo("/");
            return;
        }

        eventAttendees = SelectedEvent?.Attendees ?? new();
    }

    private async Task Submit()
    {
        HttpResponseMessage res = await _Client.PostAsJsonAsync($"Event/{Id}/Attendee", addAttendee);
        await ShowChanges(res.IsSuccessStatusCode, "You are registered!");
    }

    private async Task DeleteAttendee()
    {
        HttpResponseMessage res = await _Client.DeleteAsync($"Event/{Id}/Attendee/{selectedAttendee}");
        await ShowChanges(res.IsSuccessStatusCode, "Attendee Removed Successfully!");
    }

    private async Task ShowChanges(bool success, string message)
    {
        ResultMessage = success ? message : "An Error Occured!";
        AlertClass = $"alert alert-{(success ? "success" : "danger")}";
        addAttendee = new();
        StateHasChanged();

        await LoadEventDetails();

        await Task.Delay(2000);
        AlertClass = "d-none";
        ResultMessage = string.Empty;
    }
}
