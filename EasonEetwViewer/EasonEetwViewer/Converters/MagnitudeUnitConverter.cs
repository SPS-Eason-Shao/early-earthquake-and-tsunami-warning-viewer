﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Data.Converters;
using EasonEetwViewer.HttpRequest.DmdataComponent;
using EasonEetwViewer.HttpRequest.DmdataComponent.Enum;
using EasonEetwViewer.Lang;

namespace EasonEetwViewer.Converters;
internal class MagnitudeUnitConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is Magnitude magnitude
            ? magnitude.Condition is MagnitudeCondition
                ? Resources.EarthquakeMagnitudeUnitDefault
                : magnitude.Unit switch
                {
                    MagnitudeUnit.JmaMagnitude => Resources.EarthquakeMagnitudeUnitJma,
                    MagnitudeUnit.NormalMagnitude => Resources.EarthquakeMagnitudeUnitMoment,
                    MagnitudeUnit.Unknown or _ => Resources.EarthquakeMagnitudeUnitDefault
                }
            : value is null
                ? Resources.EarthquakeMagnitudeUnitDefault
                : null;
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}
