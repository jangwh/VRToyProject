using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public Button GameStart;
    public Button GameRetry;
    public Button GameEnd;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            DestroyImmediate(this);
            return;
        }
        GameStart.gameObject.SetActive(true);
        GameRetry.gameObject.SetActive(false);
        GameEnd.gameObject.SetActive(true);
    }

    public void OnGameStart()
    {
        GameStart.gameObject.SetActive(false);
        GameRetry.gameObject.SetActive(false);
        GameEnd.gameObject.SetActive(false);
    }
    public void OnGameRetry()
    {
        SceneManager.LoadScene(0);
        GameStart.gameObject.SetActive(false);
        GameRetry.gameObject.SetActive(false);
        GameEnd.gameObject.SetActive(false);
    }
    public void OnGameEnd()
    {
        Application.Quit();
    }
}
