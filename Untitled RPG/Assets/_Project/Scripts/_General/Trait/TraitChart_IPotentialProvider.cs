public partial class TraitChart
{
    public int StrengthPotential => StrengthSector.CompletedPoints;
    public int VitalityPotential => VitalitySector.CompletedPoints;
    public int DexterityPotential => DexteritySector.CompletedPoints;
    public int IntelligencePotential => IntelligenceSector.CompletedPoints;
}