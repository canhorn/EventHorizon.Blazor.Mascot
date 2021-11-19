namespace EventHorizon.Blazor.Mascot.Pages;

using System.Net.Http.Json;

using EventHorizon.Blazor.Mascot.Actions;
using EventHorizon.Blazor.Mascot.Character;

using Microsoft.AspNetCore.Components;

public partial class Playground
    : ComponentBase,
    IDisposable
{
    [Inject]
    public IHttpClientFactory HttpClientFactory { get; set; } = null!;

    protected Canvas? Canvas { get; set; }
    protected CanvasEnvironment? Environment => Canvas?.Environment;
    protected MascotAvatar? Avatar;
    protected SpeechBubbleDetails SpeechBubbleDetails = new();

    protected string AvatarConfigUrl { get; set; } = "characters/hinata/character.json";
    protected string AvatarConfigBaseUrl { get; set; } = "https://canhorn.github.io/EventHorizon.Shimeji/";
    protected int EnvironmentHeight { get; set; } = 300;
    protected int EnvironmentWidth { get; set; } = 1000;
    public string ErrorMessage { get; private set; } = string.Empty;

    protected async Task HandleUpdateEnvironment()
    {
        await OnInitializedAsync();
    }

    public string GetPositionDetails()
    {
        return $"X: {Avatar?.Position.X ?? -9999}, Y: {Avatar?.Position.Y ?? -9999}";
    }

    protected override async Task OnInitializedAsync()
    {
        base.OnInitialized();

        try
        {
            ErrorMessage = string.Empty;
            var client = HttpClientFactory.CreateClient();

            var characterResult = await client.GetAsync(
                $"{AvatarConfigBaseUrl}{AvatarConfigUrl}"
            );
            var character = await characterResult.Content.ReadFromJsonAsync<CharacterConfig>();
            if (character is null)
            {
                return;
            }
            if (!string.IsNullOrEmpty(
                AvatarConfigBaseUrl
            ))
            {
                character.BaseUrl = $"{AvatarConfigBaseUrl}{character.BaseUrl}";
            }
            Avatar?.Dispose();

            // TODO: Get Canvas Environment sizing from browser
            Avatar = new MascotAvatar(
                Canvas = new Mascot.Canvas(
                    new CanvasEnvironment(
                        "zbox",
                        EnvironmentHeight,
                        EnvironmentWidth
                    )
                ),
                character
            );
            Canvas.OnChange += () => { InvokeAsync(StateHasChanged); };
            Avatar.Init(
                200,
                200
            );
            Avatar.Behavior(
                "zbox",
                new StandardAction(
                    "zbox",
                    Avatar
                )
            );
            Avatar.Action(
                "fall",
                40
            );
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load Characeter: {ex.Message}";
        }
    }

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
    public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
    {
        Avatar?.Dispose();
    }

    private void LoadEevee()
    {
        AvatarConfigUrl = "characters/eevee/character.json";
        AvatarConfigBaseUrl = "https://canhorn.github.io/EventHorizon.Shimeji/";
    }

    private void LoadHinata()
    {
        AvatarConfigUrl = "characters/hinata/character.json";
        AvatarConfigBaseUrl = "https://canhorn.github.io/EventHorizon.Shimeji/";
    }
}
