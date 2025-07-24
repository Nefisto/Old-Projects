using System;
using System.Globalization;
using UnityEngine;

public static partial class Helper
{
    public static class ColorHelper
    {
        public static Color32 FromHex (string hex)
        {
            if (hex.Length < 6)
                throw new FormatException("Needs a string with a length of at least 6");

            var r = hex[..2];
            var g = hex[2..4];
            var b = hex[4..6];

            var alpha = hex.Length >= 8 ? hex.Substring(6, 2) : "FF";

            return new Color32(byte.Parse(r, NumberStyles.HexNumber),
                byte.Parse(g, NumberStyles.HexNumber),
                byte.Parse(b, NumberStyles.HexNumber),
                byte.Parse(alpha, NumberStyles.HexNumber));
        }

        public static Color32 SetAlpha (Color color, float targetAlpha)
        {
            color.a = targetAlpha;
            return color;
        }
    }
}