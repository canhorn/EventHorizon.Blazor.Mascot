namespace EventHorizon.Blazor.Mascot.Sample.Wasm.Pages;

using System.Net.Http.Json;

using BlazorPro.BlazorSize;

using EventHorizon.Blazor.Mascot.Actions;
using EventHorizon.Blazor.Mascot.Character;

using Microsoft.AspNetCore.Components;

public partial class FullScreenAvatar
{
    [Inject]
    public IHttpClientFactory HttpClientFactory { get; set; } = null!;
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;
    [Inject]
    public IResizeListener ResizeListener { get; set; } = null!;
    protected Canvas? Canvas { get; set; }

    protected CharacterConfig? Character { get; set; }
    protected CanvasEnvironment? Environment => Canvas?.Environment;
    protected MascotAvatar? Avatar;
    protected SpeechBubbleDetails SpeechBubbleDetails = new();

    protected string AvatarConfigUrl { get; set; } = "/hinata/character.json";
    protected string AvatarConfigBaseUrl { get; set; } = string.Empty;
    protected int EnvironmentHeight { get; set; } = 300;
    protected int EnvironmentWidth { get; set; } = 1000;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if(firstRender)
        {
            ResizeListener.OnResized += HandleWindowResize;
            await Setup();
        }
    }

    private void HandleWindowResize(
        object? obj,
        BrowserWindowSize args
    )
    {
        Avatar?.Dispose();
        InvokeAsync(async () =>
        {
            await Setup();
        });
    }

    public void Dispose()
    {
        Avatar?.Dispose();
    }

    public async Task Setup()
    {
        var browserSize = await ResizeListener.GetBrowserWindowSize();
        // 200 is the top padding, so we can see the Avatar walk on the top
        EnvironmentHeight = browserSize.Height - 200; 
        EnvironmentWidth = browserSize.Width;

        var client = HttpClientFactory.CreateClient();
        AvatarConfigBaseUrl = NavigationManager.BaseUri.ToString();
        Character = await (await client.GetAsync(
            NavigationManager.ToAbsoluteUri(
                AvatarConfigUrl
            )
        )).Content.ReadFromJsonAsync<CharacterConfig>();
        if (Character is null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(AvatarConfigBaseUrl))
        {
            Character.BaseUrl = $"{AvatarConfigBaseUrl}{Character.BaseUrl}";
        }
        Avatar = new MascotAvatar(
            Canvas = new Canvas(
                new CanvasEnvironment(
                    "zbox",
                    EnvironmentHeight,
                    EnvironmentWidth
                )
            ),
            Character
        );
        Canvas.OnChange += () =>
        {
            InvokeAsync(StateHasChanged);
        };
        Avatar.Init(200, 200);
        Avatar.Behavior("zbox", new StandardAction("zbox", Avatar));
        Avatar.Action("fall", 40);
    }
}
