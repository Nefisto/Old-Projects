using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private Button startGameButton;
    
    [TitleGroup("References")]
    [SerializeField]
    private Button creditsOpenButton;

    [TitleGroup("References")]
    [SerializeField]
    private Button creditsCloseButton;

    [TitleGroup("References")]
    [SerializeField]
    private Transform creditsScreen;

    [TitleGroup("References")]
    [SerializeField]
    private Transform startMenuScreen;

    [BankRef]
    public List<string> banks = new();
    
    private void Awake()
    {
        creditsOpenButton.onClick.AddListener(() =>
        {
            creditsScreen.gameObject.SetActive(true);
            startMenuScreen.gameObject.SetActive(false);
        });
        creditsCloseButton.onClick.AddListener(() =>
        {
            creditsScreen.gameObject.SetActive(false);
            startMenuScreen.gameObject.SetActive(true);
        });
        
        startGameButton.onClick.AddListener(StartGameHandle);
    }

    private void StartGameHandle()
    {
        StartCoroutine(Routine());
        
        IEnumerator Routine()
        {
            yield return ServiceLocator.ScreenFading.FadeIn(new ScreenFading.Settings() { duration = 1.5f });
            SceneManager.LoadScene(1);
        }
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        
        foreach (var bank in banks)
        {
            RuntimeManager.LoadBank(bank, true);
        }

        yield return new WaitUntil(() => RuntimeManager.HaveAllBanksLoaded);
        yield return new WaitUntil(() => !RuntimeManager.AnySampleDataLoading());
        yield return ServiceLocator.ScreenFading.FadeOut(new ScreenFading.Settings() { duration = 1.5f });
    }
}