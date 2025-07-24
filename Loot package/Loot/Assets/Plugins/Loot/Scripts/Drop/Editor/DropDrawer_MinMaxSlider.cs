#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Loot.Editor
{
    public partial class DropDrawer
    {
        private void AmountVariationDraw (Rect rect, SerializedProperty property, GUIContent label)
        {
            var firstLineRect = EditorGUI.PrefixLabel(rect, new GUIContent("Amount range"));
            var secondLine = new Rect(new Vector2(firstLineRect.x, firstLineRect.y + EditorGUIUtility.singleLineHeight),
                new Vector2(firstLineRect.size.x, EditorGUIUtility.singleLineHeight));

            var firstLineSplit = SplitRect(firstLineRect, 5);
            var secondLineSplit = SplitRect(secondLine, 3);
            secondLineSplit = ChangeRectProportionsGivingMoreSpaceToMidOne(secondLineSplit);

            EditorGUI.BeginChangeCheck();
            var value = amountRange.vector2IntValue;
            var limits = amountLimit.vector2IntValue;

            if (!hasAlreadyUpdatedLimit.boolValue)
            {
                var newLimit = new Vector2Int
                {
                    x = limits.x > value.x ? value.x : 0,
                    y = limits.y < value.y ? value.y : 10
                };
                amountLimit.vector2IntValue = newLimit;
                limits = newLimit;
                hasAlreadyUpdatedLimit.boolValue = true;
            }

            // Aesthetics only
            firstLineSplit[1].position += new Vector2(10f, 0f);
            firstLineSplit[3].position -= new Vector2(10f, 0f);
            EditorGUI.LabelField(firstLineSplit[2], "~", new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                normal = new GUIStyleState
                {
                    textColor = Color.white
                }
            });

            value.x = EditorGUI.DelayedIntField(firstLineSplit[1], value.x);
            value.y = EditorGUI.DelayedIntField(firstLineSplit[3], value.y);

            limits.x = EditorGUI.DelayedIntField(secondLineSplit[0], limits.x);
            limits.y = EditorGUI.DelayedIntField(secondLineSplit[2], limits.y);

            float minValue = value.x;
            float maxValue = value.y;
            EditorGUI.MinMaxSlider(secondLineSplit[1], ref minValue, ref maxValue, limits.x, limits.y);

            if (minValue < limits.x)
                minValue = limits.x;

            if (maxValue > limits.y)
                maxValue = limits.y;

            if (minValue > maxValue)
                minValue = maxValue;

            if (maxValue < minValue)
                maxValue = minValue;

            if (limits.x >= limits.y)
                limits.x = limits.y - 1;

            if (!EditorGUI.EndChangeCheck())
                return;

            amountRange.vector2IntValue = new Vector2Int((int)minValue, (int)maxValue);
            amountLimit.vector2IntValue = new Vector2Int(limits.x, limits.y);
        }

        private static Rect[] SplitRect (Rect rectToSplit, int n)
        {
            var rects = new Rect[n];

            for (var i = 0; i < n; i++)
                rects[i] = new Rect(rectToSplit.position.x + i * rectToSplit.width / n,
                    rectToSplit.position.y, rectToSplit.width / n, EditorGUIUtility.singleLineHeight);

            return rects;
        }

        private static Rect[] ChangeRectProportionsGivingMoreSpaceToMidOne (Rect[] rects)
        {
            var padding = (int)rects[0].width - 40;
            const int space = 5;

            rects[0].width -= padding + space;
            rects[2].width -= padding + space;

            rects[1].x -= padding;
            rects[1].width += padding * 2;

            rects[2].x += padding + space;

            return rects;
        }
    }
}
#endif