using System.Collections;
using UnityEngine;

public class DatabaseLoader : MonoBehaviour
{
    [SerializeField]
    private Database database;
    
    private void Awake() => GameEntryPoints.OnLoadAssets += LoadAssetsHandle;

    private IEnumerator LoadAssetsHandle (object arg)
    {
        ServiceLocator.Database = database;
        yield break;
    }
}