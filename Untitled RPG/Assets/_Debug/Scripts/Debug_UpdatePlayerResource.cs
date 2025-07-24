using UnityEngine;
using UnityEngine.UI;

public class Debug_UpdatePlayerResource : MonoBehaviour
{
    public Slider healthSlider;
    public Slider manaSlider;

    public void UpdatePlayerLife()
    {
        ServiceLocator
            .BattleContext
            .Player
            .HealthResource
            .SetToPercentage(healthSlider.value);
    }

    public void UpdatePlayerMana()
    {
        ServiceLocator
            .BattleContext
            .Player
            .ManaResource
            .SetToPercentage(manaSlider.value);
    }
}