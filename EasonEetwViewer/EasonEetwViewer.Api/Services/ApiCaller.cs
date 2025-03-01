﻿using System.Collections.Specialized;
using System.Text;
using System.Text.Json;
using System.Web;
using EasonEetwViewer.Api.Abstractions;
using EasonEetwViewer.Api.Dtos;
using EasonEetwViewer.Api.Dtos.Enum.WebSocket;
using EasonEetwViewer.Api.Dtos.Response;
using EasonEetwViewer.Api.Extensions;
using EasonEetwViewer.Api.Logging;
using EasonEetwViewer.Authentication.Abstractions;
using EasonEetwViewer.Dtos.Enum;
using Microsoft.Extensions.Logging;

namespace EasonEetwViewer.Api.Services;
/// <summary>
/// The default implmenetation for <see cref="IApiCaller"/>.
/// </summary>
internal sealed class ApiCaller : IApiCaller
{
    private readonly HttpClient _client;
    private readonly ILogger<ApiCaller> _logger;
    private readonly IAuthenticationHelper _authenticator;
    private readonly JsonSerializerOptions _options;
    public ApiCaller(string baseApi, ILogger<ApiCaller> logger, IAuthenticationHelper authenticator, JsonSerializerOptions jsonSerializerOptions)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(baseApi, nameof(baseApi));

        _client = new()
        {
            BaseAddress = new(baseApi)
        };
        _authenticator = authenticator;
        _options = jsonSerializerOptions;
        _logger = logger;
        _logger.Instantiated();
    }
    /// <inheridoc/>
    public async Task<ContractList?> GetContractListAsync()
    {
        using HttpRequestMessage request = new(HttpMethod.Get, "contract");
        request.Headers.Authorization = await _authenticator.GetAuthenticationHeaderAsync();
        using HttpResponseMessage response = await _client.SendAsync(request);

        _ = response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        ContractList contractList = JsonSerializer.Deserialize<ContractList>(responseBody, _options) ?? throw new Exception();
        return contractList;
    }
    /// <inheridoc/>
    public async Task<WebSocketList?> GetWebSocketListAsync(int? id = null, ConnectionStatus? connectionStatus = null, string? cursorToken = null, int? limit = null)
    {
        NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
        if (id is not null)
        {
            queryString.Add("id", id.ToString());
        }

        if (connectionStatus is not null)
        {
            queryString.Add("status", ((ConnectionStatus)connectionStatus).ToUriString());
        }

        if (cursorToken is not null)
        {
            queryString.Add("cursorToken", cursorToken);
        }

        if (limit is not null)
        {
            queryString.Add("limit", limit.ToString());
        }

        using HttpRequestMessage request = new(HttpMethod.Get, $"socket?{queryString}");
        request.Headers.Authorization = await _authenticator.GetAuthenticationHeaderAsync();
        using HttpResponseMessage response = await _client.SendAsync(request);

        _ = response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        WebSocketList webSocketList = JsonSerializer.Deserialize<WebSocketList>(responseBody, _options) ?? throw new Exception();
        return webSocketList;
    }
    /// <inheridoc/>
    public async Task<WebSocketStart?> PostWebSocketStartAsync(WebSocketStartPost postData)
    {
        string postDataJson = JsonSerializer.Serialize(postData);
        StringContent content = new(postDataJson, Encoding.UTF8, "application/json");
        using HttpRequestMessage request = new(HttpMethod.Post, "socket");
        request.Headers.Authorization = await _authenticator.GetAuthenticationHeaderAsync();
        request.Content = content;
        using HttpResponseMessage response = await _client.SendAsync(request);

        _ = response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        WebSocketStart startResponse = JsonSerializer.Deserialize<WebSocketStart>(responseBody, _options) ?? throw new Exception();
        return startResponse;
    }
    /// <inheridoc/>
    public async Task DeleteWebSocketAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(id, nameof(id));

        using HttpRequestMessage request = new(HttpMethod.Delete, $"socket/{id}");
        request.Headers.Authorization = await _authenticator.GetAuthenticationHeaderAsync();
        using HttpResponseMessage response = await _client.SendAsync(request);

        _ = response.EnsureSuccessStatusCode();
        return;
    }

    /// <inheridoc/>
    public async Task<EarthquakeParameter?> GetEarthquakeParameterAsync()
    {
        using HttpRequestMessage request = new(HttpMethod.Get, "parameter/earthquake/station");
        request.Headers.Authorization = await _authenticator.GetAuthenticationHeaderAsync();
        using HttpResponseMessage response = await _client.SendAsync(request);

        _ = response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        EarthquakeParameter earthquakeParameter = JsonSerializer.Deserialize<EarthquakeParameter>(responseBody, _options) ?? throw new Exception();
        return earthquakeParameter;
    }
    /// <inheridoc/>
    public async Task<GdEarthquakeList?> GetPastEarthquakeListAsync(string? hypocentreCode = null, Intensity? maxInt = null, DateOnly? date = null, int? limit = null, string? cursorToken = null)
    {
        NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
        if (hypocentreCode is not null)
        {
            queryString.Add("hypocenter", hypocentreCode);
        }

        if (maxInt is not null)
        {
            queryString.Add("maxInt", ((Intensity)maxInt).ToUriString());
        }

        if (date is not null)
        {
            queryString.Add("date", ((DateOnly)date).ToString("yyyy-MM-dd"));
        }

        if (limit is not null)
        {
            queryString.Add("limit", limit.ToString());
        }

        if (cursorToken is not null)
        {
            queryString.Add("cursorToken", cursorToken);
        }

        using HttpRequestMessage request = new(HttpMethod.Get, $"gd/earthquake?{queryString}");
        request.Headers.Authorization = await _authenticator.GetAuthenticationHeaderAsync();
        using HttpResponseMessage response = await _client.SendAsync(request);

        _ = response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        GdEarthquakeList pastEarthquakeList = JsonSerializer.Deserialize<GdEarthquakeList>(responseBody, _options) ?? throw new Exception();
        return pastEarthquakeList;
    }
    /// <inheridoc/>
    public async Task<GdEarthquakeEvent?> GetPathEarthquakeEventAsync(string eventId)
    {
        using HttpRequestMessage request = new(HttpMethod.Get, $"gd/earthquake/{eventId}");
        request.Headers.Authorization = await _authenticator.GetAuthenticationHeaderAsync();
        using HttpResponseMessage response = await _client.SendAsync(request);

        _ = response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        GdEarthquakeEvent pastEarthquakeEvent = JsonSerializer.Deserialize<GdEarthquakeEvent>(responseBody, _options) ?? throw new Exception();
        return pastEarthquakeEvent;
    }
}
