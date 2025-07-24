using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class InfoCardPanel : PersistentLazySingletonMonoBehaviour<InfoCardPanel>
{
    private const string InfoCardLocation = "Info card canvas";

    [Title("Control")]
    [SerializeField]
    private InfoCardController infoCard;
    
    private IEnumerator Start()
    {
        yield return LoadInfoCard();
    }

    public void ShowInfoCard (InfoCardContext ctx)
    {
        StartCoroutine(Show());
        
        IEnumerator Show()
        {
            if (infoCard == null)
                yield return LoadInfoCard();
            
            infoCard.ShowInfoCard(ctx);
        }
    }

    private IEnumerator LoadInfoCard()
    {
        var handle = Addressables.InstantiateAsync(InfoCardLocation);

        yield return handle;

        if (handle.Status != AsyncOperationStatus.Succeeded)
            throw new Exception($"Didnt found this location on addressables: - {InfoCardLocation} -");

        infoCard = handle.Result.GetComponentInChildren<InfoCardController>();
    }
}