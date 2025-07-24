using System;
using System.Collections;
using System.Collections.Generic;
using NTools;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using EventHandler = NTools.EventHandler;

public class LogPanel : SerializedMonoBehaviour
{
    [Title("Settings")]
    [SerializeField]
    private int maxLogsAtSameTime = 5;

    [Title("References")]
    [SerializeField]
    private LogMessage logPrefab;

    [Title("Debug")]
    [ReadOnly]
    [SerializeField]
    private int currentLogsInScreen;

    private Task checkForQueuedMessageRoutine;

    [ReadOnly]
    [OdinSerialize]
    [ShowInInspector]
    private Queue<LogContext> logQueue = new();

    private void Awake()
        => checkForQueuedMessageRoutine = new Task(CheckForQueueMessage());

    private void Start()
        => EventHandler.RegisterEvent<LogContext>(GameEvents.CREATE_LOG, CreateMessage);

    public void ClearLogPanel()
    {
        checkForQueuedMessageRoutine.Pause();

        logQueue.Clear();

        checkForQueuedMessageRoutine.Unpause();
    }

    private IEnumerator CheckForQueueMessage()
    {
        while (true)
        {
            yield return new WaitForSeconds(.5f);

            if (logQueue.Count == 0)
            {
                // checkForQueuedMessageRoutine.Pause();
                continue;
            }

            if (currentLogsInScreen >= maxLogsAtSameTime)
                continue;

            var log = logQueue.Dequeue();
            CreateMessage(log);
        }
    }

    private void CreateMessage (LogContext ctx)
    {
        if (!CanSendLogToHUD())
        {
            QueueMessage(ctx);
            // checkForQueuedMessageRoutine.Unpause();
            return;
        }

        var title = ctx.Title;
        var description = ctx.Description;
        var timeInScreen = ctx.TimeInScreen;

        currentLogsInScreen++;
        var instance = Instantiate(logPrefab, transform, false);
        instance.Setup(title, description);

        StartCoroutine(DestroyAfterNSeconds(timeInScreen, instance));
    }

    private IEnumerator DestroyAfterNSeconds (float seconds, LogMessage instance)
    {
        yield return new WaitForSeconds(seconds);

        currentLogsInScreen--;
        Destroy(instance.gameObject);
    }

    private void QueueMessage (LogContext log)
        => logQueue.Enqueue(log);

    private bool CanSendLogToHUD()
        => currentLogsInScreen < maxLogsAtSameTime;
}