using System.Globalization;
using System.Text.Json;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using EasonEetwViewer.Authentication;
using EasonEetwViewer.HttpRequest;
using EasonEetwViewer.Services;
using EasonEetwViewer.Services.KmoniOptions;
using EasonEetwViewer.ViewModels;
using EasonEetwViewer.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace EasonEetwViewer;
public partial class App : Application
{
    /// <inheritdoc/> 
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public static IServiceProvider Service { get; private set; }

    private static KmoniOptions GetKmoniOptions(string kmoniOptionsPath)
    {
        IKmoniDto kmoniDto;
        try
        {
            IKmoniDto? serialisedDto = JsonSerializer.Deserialize<KmoniSerialisableOptions>(File.ReadAllText(kmoniOptionsPath));
            kmoniDto = serialisedDto is not null ? serialisedDto : new KmoniDefaultOptions();
        }
        catch
        {
            kmoniDto = new KmoniDefaultOptions();
        }

        KmoniOptions kmoniOptions = new(kmoniDto);
        kmoniOptions.PropertyChanged += (s, e)
            => File.WriteAllText(kmoniOptionsPath, JsonSerializer.Serialize(kmoniOptions.ToKmoniSerialisableOptions()));

        return kmoniOptions;
    }

    private static AuthenticatorDto GetAuthenticatorDto(string authenticatorPath)
    {
        AuthenticatorDto authenticatorDto;

        try
        {
            AuthenticatorDto? serialisedDto = JsonSerializer.Deserialize<AuthenticatorDto>(File.ReadAllText(authenticatorPath));
            authenticatorDto = serialisedDto is not null ? serialisedDto : new AuthenticatorDto();
        }
        catch
        {
            authenticatorDto = new AuthenticatorDto();
        }

        return authenticatorDto;
    }

    private static CultureInfo GetLanguage(string languagePath)
    {
        CultureInfo culture;
        try
        {
            string languageOption = File.ReadAllText(languagePath);
            culture = new CultureInfo(languageOption);
        }
        catch
        {
            culture = CultureInfo.InvariantCulture;
        }

        return culture;
    }

    /// <inheritdoc/>
    public override void OnFrameworkInitializationCompleted()
    {
        IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();


        string baseApi = config["BaseApi"]!;
        string baseTelegram = config["BaseTelegram"]!;

        string kmoniOptionsPath = config["KmoniOptionsPath"]!;
        string authenticatorPath = config["AuthenticatorPath"]!;
        string languagePath = config["LanguagePath"]!;

        //IConfigurationSection oAuthConfig = config.GetSection("oAuth");
        //string oAuthClientId = oAuthConfig["clientId"] ?? string.Empty;
        //string oAuthBaseUri = oAuthConfig["baseUri"] ?? string.Empty;
        //string oAuthHost = oAuthConfig["host"] ?? string.Empty;
        //HashSet<string> oAuthScopes = oAuthConfig.GetSection("scopes").Get<HashSet<string>>() ?? [];
        //IAuthenticator oAuth = new OAuth(oAuthClientId, oAuthBaseUri, oAuthHost, oAuthScopes);

        Lang.Resources.Culture = GetLanguage(languagePath);

        IServiceCollection collection = new ServiceCollection();

        _ = collection.AddSingleton(GetKmoniOptions(kmoniOptionsPath));
        _ = collection.AddSingleton(GetAuthenticatorDto(authenticatorPath));
        _ = collection.AddSingleton<OnAuthenticatorChanged>(v => File.WriteAllText(authenticatorPath, JsonSerializer.Serialize(v)));
        _ = collection.AddSingleton<OnLanguageChanged>(s => File.WriteAllText(languagePath, s.Name));
        _ = collection.AddSingleton<IApiCaller>(s => new ApiCaller(baseApi, s.GetRequiredService<AuthenticatorDto>()));
        _ = collection.AddSingleton<ITelegramRetriever>(s => new TelegramRetriever(baseTelegram, s.GetRequiredService<AuthenticatorDto>()));
        _ = collection.AddSingleton<StaticResources>();

        _ = collection.AddSingleton<MainWindowViewModel>();
        _ = collection.AddSingleton<RealtimePageViewModel>();
        _ = collection.AddSingleton<PastPageViewModel>();
        _ = collection.AddSingleton<SettingPageViewModel>();

        Service = collection.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainWindow
            {
                DataContext = Service.GetRequiredService<MainWindowViewModel>(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        DataAnnotationsValidationPlugin[] dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (DataAnnotationsValidationPlugin plugin in dataValidationPluginsToRemove)
        {
            _ = BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}