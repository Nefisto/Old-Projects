using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Collectable : MonoBehaviour
{
    [Title("Settings")]
    [MinMaxSlider(0f, 100f)]
    [SerializeField]
    private Vector2 refuelAmount;

    private void OnTriggerEnter (Collider other)
    {
        var fuel = Random.Range(refuelAmount.x, refuelAmount.y);
        other.GetComponent<Player>().UpdateFuel(fuel);
        Destroy(gameObject);
    }
}