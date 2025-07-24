using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;

public class FloatTextManager : SerializedMonoBehaviour, IFloatText
{
    [TitleGroup("Settings")]
    [SerializeField]
    private float timeBetweenFloatTexts = 0.5f;

    [TitleGroup("References")]
    [SerializeField]
    private FloatTextPooler pooler;

    [TitleGroup("Debug")]
    [ReadOnly]
    [OdinSerialize]
    private List<NTask> runningTasks = new();

    [TitleGroup("Debug")]
    [ReadOnly]
    [OdinSerialize]
    private Dictionary<Transform, PriorityList<FloatTextSettings>> targetToFloatTexts = new();

    private void Awake()
    {
        targetToFloatTexts = new Dictionary<Transform, PriorityList<FloatTextSettings>>();

        ServiceLocator.FloatText = this;

        GameEvents.OnPause += PauseFloatTexts;
        GameEvents.OnUnpause += UnpauseFloatTexts;
    }

    [Button(ButtonStyle.FoldoutButton)]
    [DisableInEditorMode]
    public void AddCustomFloatText (FloatTextSettings floatTextSettings, int priority = 5)
    {
        var currentTarget = floatTextSettings.targetTransform;

        targetToFloatTexts.TryAdd(currentTarget, new PriorityList<FloatTextSettings>());

        if (targetToFloatTexts[currentTarget].Count == 0)
        {
            targetToFloatTexts[currentTarget].Add(priority, floatTextSettings);

            var task = new NTask(ShowText(currentTarget));
            task.OnFinished += _ => runningTasks.Remove(task);
            runningTasks.Add(task);
        }
        else
        {
            targetToFloatTexts[currentTarget].Add(priority, floatTextSettings);
        }
    }

    private void UnpauseFloatTexts()
    {
        foreach (var runningTask in runningTasks)
            runningTask.Unpause();
    }

    private void PauseFloatTexts()
    {
        foreach (var runningTask in runningTasks)
            runningTask.Pause();
    }

    private IEnumerator ShowText (Transform target)
    {
        while (targetToFloatTexts[target].Count != 0)
        {
            var settings = targetToFloatTexts[target].First();
            var text = pooler.GetFloatText();

            SetupText(text, settings);

            var task = new NTask(MoveTextToUp(settings, text));
            task.OnFinished += _ => runningTasks.Remove(task);
            runningTasks.Add(task);

            yield return new WaitForSeconds(timeBetweenFloatTexts);

            targetToFloatTexts[target].Remove(settings);
        }
    }

    private static IEnumerator MoveTextToUp (FloatTextSettings floatTextSettings, TMP_Text text)
    {
        var counter = 0f;
        while (counter < floatTextSettings.timeOnScreen)
        {
            text.transform.Translate(Vector3.up * (floatTextSettings.upSpeed * Time.deltaTime));
            yield return null;
            counter += Time.deltaTime;
        }

        text.gameObject.SetActive(false);
    }

    private static void SetupText (TMP_Text text, FloatTextSettings floatTextSettings)
    {
        text.gameObject.SetActive(true);
        text.text = floatTextSettings.message.ToLower();
        text.color = floatTextSettings.textColor;
        text.transform.position = floatTextSettings.targetTransform.position;
    }
}