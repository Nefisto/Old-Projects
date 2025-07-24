using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NTools;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Serialization;

public partial class Player
{
    [TabGroup(TabNameFieldOfView)]
    [SerializeField]
    private bool enableDetection = true;

    [TabGroup(TabNameFieldOfView)]
    [SerializeField]
    private bool debugFieldOfView;

    [TabGroup(TabNameFieldOfView)]
    [Title("Status")]
    [Range(2f, 20f)]
    [SerializeField]
    private float visionRadius = 1f;

    [TabGroup(TabNameFieldOfView)]
    [Range(1f, 180f)]
    [SerializeField]
    private float viewAngle = 30f;

    [TabGroup(TabNameFieldOfView)]
    [Tooltip("Layer that will block the view")]
    [SerializeField]
    private LayerMask obstructLayer;

    [TabGroup(TabNameFieldOfView)]
    [Title("Debug")]
    [ReadOnly]
    [SerializeField]
    private float currentViewAngle;

    [TabGroup(TabNameFieldOfView)]
    [ReadOnly]
    [SerializeField]
    private List<Enemy> enemiesInView = new List<Enemy>();

    #region API

    public Enemy GetNearestEnemy()
    {
        if (enemiesInView.Count == 0)
            return null;

        // Find the near enemy
        var playerPosition = transform.position;
        var nearEnemy = enemiesInView[0];
        var nearDistance = (nearEnemy.transform.position - playerPosition).sqrMagnitude;
        for (var i = 1; i < enemiesInView.Count; i++)
        {
            var currentDistance = (enemiesInView[i].transform.position - playerPosition).sqrMagnitude;
            if (currentDistance < nearDistance)
            {
                nearEnemy = enemiesInView[i];
                nearDistance = currentDistance;
            }
        }

        return nearEnemy;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Periodically scan for enemies in cone view
    /// </summary>
    private void SetupEnemyDetection()
    {
        new Task(ScanForEnemies(.2f));
    }

    private IEnumerator ScanForEnemies(float sleepTime)
    {
        while (true)
        {
            // Simulate pause
            if (!enableDetection)
            {
                yield return new WaitForSeconds(1f);
                continue;
            }
                    
            yield return new WaitForSeconds(sleepTime);

            // Enemies that exist in actual list but not here will fade out
            // Enemies that exist here but not on original will fade in
            // Enemies that exist in both will do nothing
            var temporaryList = new List<Enemy>();
            
            // If enemy is in range
            var enemiesInRange = Physics.OverlapSphere(transform.position, visionRadius, EnemyLayer);
            
            foreach (var enemy in enemiesInRange)
            {
                // If it's inside the cone
                var directionToTarget = (enemy.transform.position - transform.position).normalized;
                if (Vector3.Angle(FlashlightPosition.forward, directionToTarget) <= (currentViewAngle * .5f))
                {
                    var distanceToTarget = Vector3.Distance(transform.position, enemy.transform.position);
                    // If its visible by player
                    if (!Physics.Raycast(transform.position, directionToTarget, out _, distanceToTarget, obstructLayer))
                    {
                        temporaryList.Add(enemy.GetComponent<Enemy>());
                    }
                }
            }

            var newEnemies = temporaryList.Except(enemiesInView).ToList();
            var oldEnemies = enemiesInView.Except(temporaryList).ToList();

            newEnemies.ForEach(enemy => enemy.AddDetector(new Detector(gameObject, true)));
            oldEnemies.ForEach(enemy => enemy.RemoveDetector(gameObject));

            enemiesInView = temporaryList.ToList();
        }
    }
    
    private void UpdateAngle (float percent)
    {
        DOTween.To(() => currentViewAngle, x => currentViewAngle = x, viewAngle * percent, .5f);
    }
    
    // Will give a point in unit circle to simulate the arc
    // JUST TO SIMPLIFY THE DEBUG
    private Vector3 AngleDirection (float degreeAngle, bool isGlobal = false)
    {
        if (!isGlobal)
        {
            var offSet = FlashlightPosition == null
                ? transform.rotation.eulerAngles.y
                : FlashlightPosition.rotation.eulerAngles.y;
            
            degreeAngle += offSet; // Add the Yaw to make degree relative to player
        }
        
        return new Vector3(Mathf.Sin(degreeAngle * Mathf.Deg2Rad), 0f, Mathf.Cos(degreeAngle * Mathf.Deg2Rad));
    }

    #endregion

    #region Debug

#if UNITY_EDITOR
    private void DebugFoV()
    {
        var originalColor = Handles.color;
        
        var playerPosition = transform.position;
        Handles.DrawWireDisc(playerPosition, Vector3.up, visionRadius, 3f);

        // Show max angle when not playing
        var actualAngle = Application.isPlaying
            ? currentViewAngle : viewAngle;
        var pointA = AngleDirection(-actualAngle * .5f) * visionRadius;
        var pointB = AngleDirection(actualAngle * .5f) * visionRadius;

        Handles.DrawAAPolyLine(Texture2D.whiteTexture, playerPosition, playerPosition + pointA);
        Handles.DrawAAPolyLine(Texture2D.whiteTexture, playerPosition, playerPosition + pointB);

        Handles.color = originalColor;
    }
#endif
    
    #endregion
    
    private const string TabNameFieldOfView = "Field of view";
}