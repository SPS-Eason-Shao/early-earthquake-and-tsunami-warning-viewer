﻿using System.Text.Json.Serialization;

namespace EasonEetwViewer.Dmdata.Telegram.Dtos.EewInformation.Enum.Accuracy;
[JsonConverter(typeof(JsonStringEnumConverter<EpicentreDepth>))]
public enum EpicentreDepth
{
    [JsonStringEnumMemberName("0")]
    Unknown = 0,
    [JsonStringEnumMemberName("1")]
    LevelIpf1Plum = 1,
    [JsonStringEnumMemberName("2")]
    Ipf2 = 2,
    [JsonStringEnumMemberName("3")]
    Ipf3Or4 = 3,
    [JsonStringEnumMemberName("4")]
    Ipf5OrMore = 4,
    [JsonStringEnumMemberName("5")]
    [Obsolete("No longer used after 2023/09/26 14:00 JST.", true)]
    Bosai4OrLess = 5,
    [JsonStringEnumMemberName("6")]
    [Obsolete("No longer used after 2023/09/26 14:00 JST.", true)]
    Bosai5OrMoreHinet = 6,
    [JsonStringEnumMemberName("7")]
    [Obsolete("No longer used after 2023/09/26 14:00 JST.", true)]
    EposSea = 7,
    [JsonStringEnumMemberName("8")]
    [Obsolete("No longer used after 2023/09/26 14:00 JST.", true)]
    EposLand = 8,
    [JsonStringEnumMemberName("9")]
    Final = 9
}
