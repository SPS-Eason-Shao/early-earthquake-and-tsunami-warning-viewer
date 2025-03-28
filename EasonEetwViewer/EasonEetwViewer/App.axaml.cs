using System.Text.Json;
using System.Text.Json.Serialization;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Logging;
using Avalonia.Markup.Xaml;
using EasonEetwViewer.Dmdata.Api.Dtos.Request;
using EasonEetwViewer.Dmdata.Api.Extensions;
using EasonEetwViewer.Dmdata.Authentication.Extensions;
using EasonEetwViewer.Dmdata.Authentication.Options;
using EasonEetwViewer.Dmdata.Telegram.Dtos.Schema;
using EasonEetwViewer.Dmdata.Telegram.Extensions;
using EasonEetwViewer.Dmdata.WebSocket.Extensions;
using EasonEetwViewer.Extensions;
using EasonEetwViewer.JmaTravelTime.Extensions;
using EasonEetwViewer.JmaTravelTime.Options;
using EasonEetwViewer.KyoshinMonitor.Dtos;
using EasonEetwViewer.KyoshinMonitor.Extensions;
using EasonEetwViewer.Services;
using EasonEetwViewer.Services.Kmoni.Extensions;
using EasonEetwViewer.Services.Kmoni.Options;
using EasonEetwViewer.Services.Logging;
using EasonEetwViewer.Services.TimeProvider;
using EasonEetwViewer.ViewModels;
using EasonEetwViewer.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EasonEetwViewer;
/// <inheritdoc/>
internal partial class App : Application
{
    /// <inheritdoc/> 
    public override void Initialize()
        => AvaloniaXamlLoader.Load(this);
    /// <summary>
    /// The service provider for the application.
    /// </summary>
    public static IServiceProvider? Service { get; private set; }

    /// <inheritdoc/>
    public override async void OnFrameworkInitializationCompleted()
    {
        IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        IConfigurationSection baseUrls = config.GetSection("BaseUrls");
        string baseApi = baseUrls["Api"]!;
        string baseTelegram = baseUrls["Telegram"]!;

        IConfigurationSection configPaths = config.GetSection("ConfigPaths");
        string authenticatorPath = configPaths["Authenticator"]!;
        string languagePath = configPaths["Language"]!;

        IServiceCollection collection = new ServiceCollection()
            .AddLogging(loggingBuilder
                => loggingBuilder
                    .AddFileLogger(
                    new StreamWriter($"{DateTime.UtcNow:yyyyMMddHHmmss}.log"),
                    LogLevel.Warning)
#if DEBUG
                    .AddDebug()
#endif
                    .SetMinimumLevel(LogLevel.Information))

            .AddSingleton(sp
                => new JsonSerializerOptions()
                {
                    NumberHandling = JsonNumberHandling.AllowReadingFromString,
                    RespectNullableAnnotations = true,
                    RespectRequiredConstructorParameters = true
                })

            .AddApiCaller(baseApi)
            .AddTelegramRetriever(baseTelegram)
            .AddWebSocket()

            .AddSingleton<ITimeProvider, DefaultTimeProvider>()

            .AddKmoniOptionsHelper()
            .AddOptions<KmoniSettingsHelperOptions>()
                .Bind(config.GetSection("KmoniSettingsHelperOptions"))
                .Services

            .AddAuthenticator(authenticatorPath)
            .AddOptions<OAuth2Options>()
                .Bind(config.GetSection("OAuth2Options"))
                .Services

            .AddKmoniHelpers()
            .AddOptions<KmoniHelperOptions>()
                .Bind(config.GetSection("KmoniHelperOptions"))
                .Services

            .AddJmaTimeTable()
            .AddOptions<JmaTimeTableOptions>()
                .Bind(config.GetSection("TimeTableOptions"))
                .Services

            .AddSingleton<MapResourcesProvider>()

            .AddSingleton<ILogSink, AvaloniaToMicrosoftLoggingAdaptor>()

            .AddSingleton(languagePath)
            .AddSingleton<MainWindowViewModel>()
            .AddSingleton<RealtimePageViewModel>()
            .AddSingleton<PastPageViewModel>()
            .AddSingleton<SettingPageViewModel>()
            .AddOptions<WebSocketStartPost>()
                .Bind(config.GetSection("WebSocketParams"))
                .Services;

        Service = collection
            .BuildServiceProvider()
            .AttachMapsuiLogging();

        Logger.Sink = Service.GetRequiredService<ILogSink>();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainWindow
            {
                DataContext = Service.GetRequiredService<MainWindowViewModel>(),
            };
        }

        base.OnFrameworkInitializationCompleted();

#if DEBUG
        RealtimePageViewModel viewModel = Service.GetRequiredService<RealtimePageViewModel>();
        JsonSerializerOptions options = Service.GetRequiredService<JsonSerializerOptions>();
        await TestEew(viewModel, options);
        await TestTsunami(viewModel, options);
#endif
    }

    private static async Task TestEew(RealtimePageViewModel viewModel, JsonSerializerOptions options)
    {
        await Task.Delay(10000);
        EewInformationSchema eewData = JsonSerializer.Deserialize<EewInformationSchema>(
            File.ReadAllText("TestEew - 2011 - 1.json"),
            options)!;
        viewModel.WebSocketClientDataReceivedEventHandler(null, new() { Telegram = eewData });

        await Task.Delay(10000);
        EewInformationSchema eewData2 = JsonSerializer.Deserialize<EewInformationSchema>(
            File.ReadAllText("TestEew - 2018.json"),
            options)!;
        viewModel.WebSocketClientDataReceivedEventHandler(null, new() { Telegram = eewData2 });

        await Task.Delay(10000);
        EewInformationSchema eewData3 = JsonSerializer.Deserialize<EewInformationSchema>(
            File.ReadAllText("TestEew - 2011 - 2.json"),
            options)!;
        viewModel.WebSocketClientDataReceivedEventHandler(null, new() { Telegram = eewData3 });

        await Task.Delay(10000);
        EewInformationSchema eewData4 = JsonSerializer.Deserialize<EewInformationSchema>(
            File.ReadAllText("TestEew - 2011 - 3.json"),
            options)!;
        viewModel.WebSocketClientDataReceivedEventHandler(null, new() { Telegram = eewData4 });
    }

    private static async Task TestTsunami(RealtimePageViewModel viewModel, JsonSerializerOptions options)
    {
        await Task.Delay(10000);
        TsunamiInformationSchema tsunamiData = JsonSerializer.Deserialize<TsunamiInformationSchema>(
            File.ReadAllText("TestTsunami.json"),
            options)!;
        viewModel.WebSocketClientDataReceivedEventHandler(null, new() { Telegram = tsunamiData });
    }

    /// <summary>
    /// Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
    /// </summary>
    /// <remarks>
    /// <see href="https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins">More Info</see>
    /// </remarks>
    private static void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        DataAnnotationsValidationPlugin[] dataValidationPluginsToRemove =
            [.. BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>()];

        // remove each entry found
        foreach (DataAnnotationsValidationPlugin plugin in dataValidationPluginsToRemove)
        {
            _ = BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}