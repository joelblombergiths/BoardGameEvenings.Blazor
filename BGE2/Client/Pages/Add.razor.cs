using BGE2.Shared;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;

namespace BGE2.Client.Pages;

public partial class Add : ComponentBase
{
    private EditEventDto EditEvent { get; set; } = new();

    private string ResultMessage = string.Empty;
    private string AlertClass = "d-none";


    private async Task Submit()
    {
        HttpResponseMessage res = await _Client.PostAsJsonAsync(_Client.BaseAddress + "Event", EditEvent);
        ResultMessage = res.IsSuccessStatusCode ? "Event Created Successfully!" : "An Error Occured!";
        AlertClass = $"alert alert-{(res.IsSuccessStatusCode ? "success" : "danger")}";
        StateHasChanged();

        await Task.Delay(2500);
        _Navigation.NavigateTo("/");
    }
}