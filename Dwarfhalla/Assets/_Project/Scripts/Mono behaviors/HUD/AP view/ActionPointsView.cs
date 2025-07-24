using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class ActionPointsView : MonoBehaviour
{
    private const string ActionPointViewTemplate = "AP {0} / {1}";

    [TitleGroup("References")]
    [SerializeField]
    private TMP_Text apLabel;

    private ActionPoints actionPoints;

    private void Awake()
    {
        GameEntryPoints.OnFinishedSetup += _ => actionPoints = ServiceLocator.GameContext.PlayerData.ActionPoints;
        ServiceLocator.ActionPointsView = this;
    }

    public IEnumerator RefreshVisual()
    {
        apLabel.text = string.Format(ActionPointViewTemplate, actionPoints.CurrentPoints, actionPoints.MaxPoints);
        yield break;
    }
}