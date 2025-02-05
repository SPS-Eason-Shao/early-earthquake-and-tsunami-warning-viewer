﻿using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using EasonEetwViewer.Authentication;
using EasonEetwViewer.Dmdata.Caller.Interfaces;
using EasonEetwViewer.Dmdata.Dto.ApiPost;
using EasonEetwViewer.Dmdata.Dto.ApiResponse.Enum.WebSocket;
using EasonEetwViewer.Dmdata.Dto.ApiResponse.Record.Contract;
using EasonEetwViewer.Dmdata.Dto.ApiResponse.Record.WebSocket;
using EasonEetwViewer.Dmdata.Dto.ApiResponse.Response;
using EasonEetwViewer.KyoshinMonitor.Dto.Enum;
using EasonEetwViewer.Lang;
using EasonEetwViewer.Models;
using EasonEetwViewer.Services;
using EasonEetwViewer.Services.KmoniOptions;
using EasonEetwViewer.ViewModels.ViewModelBases;

namespace EasonEetwViewer.ViewModels;

internal partial class SettingPageViewModel(KmoniOptions kmoniOptions, WebSocketStartPost startPost, IWebSocketClient webSocketClient, OnLanguageChanged onLangChange,
    AuthenticatorDto authenticatorDto, IApiCaller apiCaller, ITelegramRetriever telegramRetriever, ITimeProvider timeProvider, OnAuthenticatorChanged onChange)
    : PageViewModelBase(authenticatorDto, apiCaller, telegramRetriever, timeProvider, onChange)
{
    #region languageSettings
    private readonly OnLanguageChanged LanguageChanged = onLangChange;

    [ObservableProperty]
    private CultureInfo _languageChoice = Resources.Culture;

    partial void OnLanguageChoiceChanged(CultureInfo value)
    {
        LanguageChanged(value);
    }

    internal IEnumerable<CultureInfo> LanguageChoices { get; init; } = [CultureInfo.InvariantCulture, new CultureInfo("ja-JP"), new CultureInfo("zh-CN")];
    #endregion

    #region kmoniSettings
    internal KmoniOptions KmoniOptions { get; init; } = kmoniOptions;
    internal IEnumerable<SensorType> SensorTypeChoices { get; init; } = Enum.GetValues<SensorType>();
    internal IEnumerable<MeasurementType> DataTypeChoices { get; init; } = Enum.GetValues<MeasurementType>();
    #endregion

    #region webSocketSettings
    private readonly IWebSocketClient _webSocketClient = webSocketClient;
    private readonly WebSocketStartPost _startPost = startPost;

    internal bool WebSocketConnected => _webSocketClient.IsWebSocketConnected;

    [RelayCommand]
    private async Task WebSocketButton()
    {
        OnPropertyChanged(nameof(WebSocketConnected));

        if (!WebSocketConnected)
        {
            WebSocketStart webSocket = await _apiCaller.PostWebSocketStartAsync(_startPost);
            await _webSocketClient.ConnectAsync(webSocket.WebSockerUrl.Url);
        }
        else
        {
            await _webSocketClient.DisconnectAsync();
        }

        OnPropertyChanged(nameof(WebSocketConnected));
    }

    [ObservableProperty]
    private ObservableCollection<WebSocketConnectionTemplate> _webSocketConnections = [];

    private async Task<int> GetAvaliableWebSocketConnections()
    {
        ContractList contractList = await _apiCaller.GetContractListAsync();
        IEnumerable<Contract> contracts = contractList.ItemList;
        return contracts.Sum(c => c.IsValid ? c.ConnectionCounts : 0);
    }

    private readonly WebSocketConnectionTemplate _emptyConnection
        = new(-1, () => SettingPageResources.WebSocketEmptyConnectionName, new(), (x) => Task.CompletedTask, false);

    [RelayCommand]
    private async Task WebSocketRefresh()
    {
        IList<WebSocketDetails> wsList = [];
        string currentCursorToken = string.Empty;

        // Cursor Token
        for (int i = 0; i < 5; ++i)
        {
            WebSocketList webSocketList = await _apiCaller.GetWebSocketListAsync(limit: 100, connectionStatus: ConnectionStatus.Open, cursorToken: currentCursorToken);
            wsList.AddRange(webSocketList.ItemList);

            if (webSocketList.NextToken is null)
            {
                break;
            }
            else
            {
                currentCursorToken = webSocketList.NextToken;
            }
        }

        // This filtering is due to undefined filtering behaviour in the API, just in case.
        wsList = wsList.Where(x => x.WebSocketStatus == ConnectionStatus.Open).ToList();

        ObservableCollection<WebSocketConnectionTemplate> currentConnections = [];
        currentConnections.AddRange(wsList.Select(x
            => new WebSocketConnectionTemplate(x.WebSocketId, () => x.ApplicationName ?? string.Empty, x.StartTime, _apiCaller.DeleteWebSocketAsync)));

        int avaliableConnection = await GetAvaliableWebSocketConnections();
        while (currentConnections.Count < avaliableConnection)
        {
            currentConnections.Add(_emptyConnection);
        }

        WebSocketConnections = currentConnections;
    }
    #endregion

    #region authSettings
    private protected override void OnAuthenticatorChanged()
    {
        base.OnAuthenticatorChanged();
        OnPropertyChanged(nameof(OAuthConnected));
        OnPropertyChanged(nameof(ApiKeyConfirmed));
        OnPropertyChanged(nameof(ApiKeyButtonEnabled));
    }

    internal bool OAuthConnected => AuthenticationStatus == AuthenticationStatus.OAuth;
    internal bool ApiKeyConfirmed => AuthenticationStatus == AuthenticationStatus.ApiKey;
    internal bool ApiKeyButtonEnabled => AuthenticationStatus == AuthenticationStatus.None;

    [RelayCommand]
    private async Task OAuthButton()
    {
        if (AuthenticationStatus == AuthenticationStatus.OAuth)
        {
            await UnsetAuthenticatorAsync();
        }
        else
        {
            await SetAuthenticatorToOAuthAsync();
        }
    }

    [ObservableProperty]
    private string _apiKeyText = string.Empty;

    [RelayCommand]
    private void ApiKeyButton() => SetAuthenticatorToApiKey(ApiKeyText);
    async partial void OnApiKeyTextChanged(string value)
    {
        if (AuthenticationStatus == AuthenticationStatus.ApiKey)
        {
            await UnsetAuthenticatorAsync();
        }
    }

    private void SetAuthenticatorToApiKey(string apiKey) => Authenticator = new ApiKey(apiKey);
    private async Task SetAuthenticatorToOAuthAsync()
    {
        Authenticator = new OAuth("CId.RRV95iuUV9FrYzeIN_BYM9Z35MJwwQen5DIwJ8JQXaTm",
            "https://manager.dmdata.jp/account/oauth2/v1/",
            "manager.dmdata.jp",
            [
            "contract.list",
            "eew.get.forecast",
            "gd.earthquake",
            "parameter.earthquake",
            "socket.close",
            "socket.list",
            "socket.start",
            "telegram.data",
            "telegram.get.earthquake",
            "telegram.list"
        ]);
        await ((OAuth)Authenticator).CheckAccessTokenAsync();
    }
    private async Task UnsetAuthenticatorAsync()
    {
        if (Authenticator is OAuth auth)
        {
            await auth.RevokeTokens();
        }

        Authenticator = new EmptyAuthenticator();
    }
    #endregion
}