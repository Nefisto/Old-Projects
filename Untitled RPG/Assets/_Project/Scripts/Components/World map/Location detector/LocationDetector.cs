using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

[DisallowMultipleComponent]
public class LocationDetector : MonoBehaviour, ILocationDetector
{
    [TitleGroup("References")]
    [SerializeField]
    private CircleCollider2D playerCollider;

    private void Awake()
    {
        playerCollider = GetComponent<CircleCollider2D>();
        ServiceLocator.LocationDetector = this;
    }

    [Button]
    public LocationContext GetLocationContext()
    {
        var detectedLocations = Physics2D.OverlapCircleAll(playerCollider.transform.position, playerCollider.radius,
            LayerMask.GetMask("Location"));

        Assert.AreNotEqual(0, detectedLocations.Length);

        return GetLocationWithMoreContactWithPlayer(detectedLocations)
            .GetLocationContext();
    }

    private Location GetLocationWithMoreContactWithPlayer (Collider2D[] detectedLocations)
    {
        var biggerArea = (area: 0f, collider: new Collider2D());
        foreach (var detectedLocation in detectedLocations)
        {
            var intersectionBound = new Bounds();

            intersectionBound.SetMinMax(
                Vector3.Max(playerCollider.bounds.min, detectedLocation.bounds.min),
                Vector3.Min(playerCollider.bounds.max, detectedLocation.bounds.max));

            var area = intersectionBound.size.x * intersectionBound.size.y;
            if (area > biggerArea.area)
                biggerArea = (area, detectedLocation);
        }

        return biggerArea
            .collider
            .GetComponent<Location>();
    }
}