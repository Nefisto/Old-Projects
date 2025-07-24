using Sirenix.OdinInspector;
using UnityEngine;

public class Information : MonoBehaviour
{
    [Multiline(5)]
    [HideLabel]
    [SerializeField]
    private string message;
}