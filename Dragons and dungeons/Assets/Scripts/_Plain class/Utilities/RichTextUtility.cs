using UnityEngine;

public class RichTextUtility
{
    public static string GetRichTextWithColor (string text, Color color)
    {
        var hexColor = ColorUtility.ToHtmlStringRGBA(color);
        
        return $"<color=#{hexColor}>{text}</color>";
    }
}