using System;
using Random = UnityEngine.Random;

[Serializable]
public class MagickaResource
{
    public int red;
    public int black;
    public int white;

    public MagickaResource(int red = 0, int black = 0, int white = 0)
    {
        this.red = red;
        this.black = black;
        this.white = white;
    }

    public bool CanAfford (Magicka magicka)
        => CanAfford(magicka.Cost);
    
    public bool CanAfford (MagickaResource cost)
        => red >= cost.red
           && black >= cost.black
           && white >= cost.white;

    public MagickaResource Reduce (Magicka magicka) => Reduce(magicka.Cost);
    public MagickaResource Reduce (MagickaResource cost) => new(red - cost.red, black - cost.black, white - cost.white);

    public MagickaResource Generate (int amount, out int[] generatedAmount)
    {
        generatedAmount = new int[3];

        for (var i = 0; i < amount; i++)
        {
            var resource = Random.Range(0, 3);
            generatedAmount[resource]++;
        }

        return new MagickaResource(red + generatedAmount[0], black + generatedAmount[1], white + generatedAmount[2]);
    }
    
    public void Reset()
    {
        red = black = white = 0;
    }
}