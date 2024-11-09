﻿using System.Text.Json.Serialization;

namespace EasonEetwViewer.Dto.Http.Response.Enum;

/// <summary>
/// Represents the status of the WebSocket connection.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<ConnectionStatus>))]
public enum ConnectionStatus
{
    /// <summary>
    /// Unknown. Default value.
    /// </summary>
    Unknown = 0,
    /// <summary>
    /// The value <c>waiting</c>, representing awaiting connection.
    /// </summary>
    [JsonStringEnumMemberName("waiting")]
    Waiting = 1,
    /// <summary>
    /// The value <c>open</c>, representing an active connection.
    /// </summary>
    [JsonStringEnumMemberName("open")]
    Open = 2,
    /// <summary>
    /// The value <c>closed</c>, representing a closed connection.
    /// </summary>
    [JsonStringEnumMemberName("closed")]
    Closed = 3
}