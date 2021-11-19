[![EventHorizon.Blazor.Mascot](https://img.shields.io/nuget/v/EventHorizon.Blazor.Mascot?style=for-the-badge&label=Nuget)](https://www.nuget.org/packages/EventHorizon.Blazor.Mascot)

# About EventHorizon Blazor Mascot

All you need to display a little mascot on your website built with Blazor. 

## Usage

Below is a good starting point to displaying the mascot in your Blazor application.

~~~ bash
dotnet install EventHorizon.Blazor.Mascot
~~~

A Full Screen Razor Page (FullScreenAvatar.razor):

~~~ html
@page "/full-screen"
@implements IDisposable
@layout FullScreenLayout
@using BlazorPro.BlazorSize
@using EventHorizon.Blazor.Mascot.Actions
@using EventHorizon.Blazor.Mascot.Character
@using EventHorizon.Blazor.Mascot.Components

<PageTitle>Full Screen Avatar</PageTitle>

@if (Canvas != null)
{
    <div class="canvas__wrapper">
        <div class="canvas__container">
            <MascotCanvas Avatar="Avatar"
                      Environment="Environment"
                      SpeechBubbleDetails="SpeechBubbleDetails" />
        </div>
    </div>
}
else
{
    <div>Loading...</div>
}
~~~

The Code Behind of the Full Screen Razor Page (FullScreenAvatar.razor.cs):

~~~ csharp
namespace Mascot.Pages;

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
    // Supports a SpeechBubble, shown above the correlated Avatar.
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
~~~

## Deployment

~~~ bash
# Create NuGet Package
dotnet pack --output packages --configuration Release -p:Version=$PACKAGE_VERSION -p:PackageVersion=$PACKAGE_VERSION

# Push NuGet Package (Require NuGet API)
dotnet nuget push packages/EventHorizon.Blazor.Mascot.nuget --source https://api.nuget.org/v3/index.json --api-key $NUGET_API_KEY 
~~~

## License

MIT License

Copyright (c) 2021 Cody Merritt Anhorn

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

