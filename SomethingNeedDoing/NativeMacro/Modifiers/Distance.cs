﻿using SomethingNeedDoing.Core.Interfaces;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SomethingNeedDoing.NativeMacro.Modifiers;
/// <summary>
/// Modifier for specifying maximum distance for targeting and interaction.
/// </summary>
[GenericDoc(
    "Specify maximum distance for targeting and interaction",
    ["distance"],
    ["/target \"Zodiark\" <distance.3>", "/interact \"Y'shtola\" <distance.5.5>"]
)]
public class DistanceModifier(string text, float distance) : MacroModifierBase(text)
{
    private static readonly Regex Regex = new(@"(?<modifier><distance\.(?<distance>\d+(?:\.\d+)?)>)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    /// <summary>
    /// Gets the maximum distance in yalms.
    /// </summary>
    public float Distance { get; } = distance;

    /// <inheritdoc/>
    public static bool TryParse(ref string text, out IMacroModifier modifier)
    {
        var match = Regex.Match(text);
        if (!match.Success)
        {
            modifier = new DistanceModifier(string.Empty, 0);
            return false;
        }

        var group = match.Groups["modifier"];
        text = text.Remove(group.Index, group.Length);

        var distance = float.Parse(match.Groups["distance"].Value, CultureInfo.InvariantCulture);
        modifier = new DistanceModifier(group.Value, distance);
        return true;
    }
}
