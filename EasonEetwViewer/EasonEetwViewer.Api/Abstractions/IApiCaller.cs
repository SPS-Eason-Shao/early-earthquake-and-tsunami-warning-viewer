﻿using EasonEetwViewer.Dtos.ApiPost;
using EasonEetwViewer.Dtos.ApiResponse.Response;
using EasonEetwViewer.Dtos.Enum;
using EasonEetwViewer.Dtos.Enum.WebSocket;

namespace EasonEetwViewer.Api.Abstractions;
public interface IApiCaller
{
    public Task<ContractList> GetContractListAsync();
    public Task<WebSocketList> GetWebSocketListAsync(int? id = null, ConnectionStatus? connectionStatus = null, string? cursorToken = null, int? limit = null);
    public Task<WebSocketStart> PostWebSocketStartAsync(WebSocketStartPost postData);
    public Task DeleteWebSocketAsync(int id);
    public Task<EarthquakeParameter> GetEarthquakeParameterAsync();
    public Task<GdEarthquakeList> GetPastEarthquakeListAsync(string? hypocentreCode = null, Intensity? maxInt = null, DateOnly? date = null, int? limit = null, string? cursorToken = null);
    public Task<GdEarthquakeEvent> GetPathEarthquakeEventAsync(string eventId);
}
