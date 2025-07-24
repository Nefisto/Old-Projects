using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class CheatInputs : MonoBehaviour
{
    public KeyCode rechargeKey = KeyCode.R;

    [ReadOnly]
    public Player player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player")?.GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(rechargeKey))
        {
            if (player == null)
            {
                Debug.LogWarning("Player ref isn't set on cheat menu");
                return;
            }
            
            player.UpdateFuel(1000);
        }
    }
}