#if UNITY_EDITOR && UNITY_2020_1_OR_NEWER
using System;
using Loot.Enum;
using UnityEngine;

namespace Loot.Editor
{
    public class DebugEntry
    {
        private static readonly Color IncreasedColor = Color.green;
        private static readonly Color DecreasedColor = Color.red;
        private static readonly Color ChangedColor = Color.yellow;
        private static readonly Color DefaultColor = Color.white;

        public string FinalPercentage;
        public Color FinalPercentageColor = DefaultColor;
        public bool IsDisabled;
        public bool IsExtension;
        public bool IsFilteredOut;
        public bool IsGuaranteed;
        public string Name;
        public Color NameColor = DefaultColor;

        public string Range;
        public string Maximum;
        public Color MaximumColor = DefaultColor;
        public string Separator = "~";
        public string Minimum;
        public Color MinimumColor = DefaultColor;
        
        public string RateOrWeight;
        public Color RateOrWeightColor = DefaultColor;

        public DebugEntry (HierarchyEntry hierarchyEntry, PercentageCalculation tableCalculation, int cachedTableWeight)
        {
            var dropEntry = hierarchyEntry.Drop;
            var rawDropEntry = hierarchyEntry.DropWithoutModifiers;
            
            SetName(dropEntry, hierarchyEntry.HierarchyLevel);
            NameColor = GetNameColor(dropEntry, rawDropEntry);
            
            var isExtension = dropEntry.IsExtensionDrop;
            SetRange(dropEntry, isExtension);
            if (!isExtension)
            {
                MinimumColor = GetColor(dropEntry.AmountRange.x, rawDropEntry.AmountRange.x);
                MaximumColor = GetColor(dropEntry.AmountRange.y, rawDropEntry.AmountRange.y);
            }
            
            IsFilteredOut = hierarchyEntry.IsFilteredOut;

            if (isExtension || IsFilteredOut)
                RateOrWeight = " -- ";
            else
            {
                if (tableCalculation == PercentageCalculation.Simple)
                {
                    RateOrWeight = $"{dropEntry.Percentage}";
                    RateOrWeightColor = GetColor(dropEntry.Percentage, rawDropEntry.Percentage);
                }
                else
                {
                    RateOrWeight = $"{dropEntry.Weight}";
                    RateOrWeightColor = GetColor(dropEntry.Weight, rawDropEntry.Weight);
                }
            }

            IsGuaranteed = dropEntry.IsGuaranteed;
            IsDisabled = dropEntry.IsDisabled;
            IsExtension = isExtension;
            
            if (isExtension || IsFilteredOut || IsDisabled)
            {
                FinalPercentage = "--.--";
            }
            else
            {
                var newDropEntryPercentage = dropEntry.GetPercentage(cachedTableWeight);
                var oldDropEntryPercentage = rawDropEntry.GetPercentage(cachedTableWeight);
                
                FinalPercentage = $"{newDropEntryPercentage}";

                FinalPercentageColor = GetColor(newDropEntryPercentage, oldDropEntryPercentage);
            }
        }

        private Color GetColor (float newValue, float oldValue)
        {
            if (Math.Abs(newValue - oldValue) < 0.001f)
                return DefaultColor;
            
            return newValue > oldValue ? IncreasedColor : DecreasedColor;
        }
        
        private Color GetColor (int newValue, int oldValue)
        {
            if (newValue == oldValue)
                return DefaultColor;

            return newValue > oldValue ? IncreasedColor : DecreasedColor;
        }

        private Color GetNameColor (Drop dropEntry, Drop rawDropEntry)
        {
            if (dropEntry.Entry == null && rawDropEntry.Entry == null)
                return DefaultColor;

            if (dropEntry.Entry != null && rawDropEntry.Entry == null)
                return ChangedColor;
            
            if (dropEntry.Entry == null && rawDropEntry.Entry != null)
                return ChangedColor;

            return dropEntry.Entry.name != rawDropEntry.Entry.name
                ? ChangedColor : DefaultColor;
        }

        private void SetRange (Drop dropEntry, bool isExtension)
        {
            if (isExtension)
            {
                Minimum = Maximum = Separator = "-";
                return;
            }
            
            Minimum = $"{dropEntry.AmountRange.x}";
            Maximum = $"{dropEntry.AmountRange.y}";
        }

        private void SetName (Drop dropEntry, int indentationLevel)
        {
            for (var i = 0; i < indentationLevel; i++)
                Name += "        ";

            Name += dropEntry.Entry != null
                ? dropEntry.Entry.name
                : "Empty";
        }
    }
}
#endif