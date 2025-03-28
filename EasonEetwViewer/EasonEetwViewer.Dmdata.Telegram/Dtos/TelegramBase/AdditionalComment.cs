﻿using System.Text.Json.Serialization;

namespace EasonEetwViewer.Dmdata.Telegram.Dtos.TelegramBase;
public record AdditionalComment
{
    [JsonPropertyName("text")]
    public required string Text { get; init; }
    [JsonPropertyName("codes")]
    public required IEnumerable<string> Codes { get; init; }
}
