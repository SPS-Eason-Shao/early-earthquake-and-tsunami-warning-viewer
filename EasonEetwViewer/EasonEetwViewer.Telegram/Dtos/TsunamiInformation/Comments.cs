﻿using System.Text.Json.Serialization;
using EasonEetwViewer.Telegram.Dtos.TelegramBase;

namespace EasonEetwViewer.Telegram.Dtos.TsunamiInformation;
public record Comments
{
    [JsonPropertyName("free")]
    public string? FreeText { get; init; }
    [JsonPropertyName("warning")]
    public AdditionalComment? Warning { get; init; }
}
