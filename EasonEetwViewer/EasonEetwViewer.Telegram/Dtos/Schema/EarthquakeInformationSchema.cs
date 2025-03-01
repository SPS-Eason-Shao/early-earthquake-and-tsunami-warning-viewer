﻿using System.Text.Json.Serialization;
using EasonEetwViewer.Telegram.Dtos.EarthquakeInformation;
using EasonEetwViewer.Telegram.Dtos.TelegramBase;

namespace EasonEetwViewer.Telegram.Dtos.Schema;
public record EarthquakeInformationSchema : Head
{
    [JsonPropertyName("body")]
    public required Body Body { get; init; }
}