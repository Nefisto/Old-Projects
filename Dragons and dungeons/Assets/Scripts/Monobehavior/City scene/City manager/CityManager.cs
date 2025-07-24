using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class CityManager : MonoBehaviour
{
    [Title("Control")]
    [SerializeField]
    private AssetReference cityScene;

    [SerializeField]
    private FadeBackground background;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        GameEvents.City.RaiseStart();
    }

    private void OnEnable()
        => GameEvents.City.OnStart += StartCityListener;

    private void OnDisable()
        => GameEvents.City.OnStart -= StartCityListener;

    private void StartCityListener()
    {
        StartCoroutine(_Start());
        
        IEnumerator _Start()
        {
            yield return background.FadeOutRoutine();
        }
    }

    public void ChangeScene()
    {
        StartCoroutine(_ChangeScene());
        
        IEnumerator _ChangeScene()
        {
            yield return background.FadeInRoutine();

            yield return cityScene.LoadSceneAsync();
        }
    }
}