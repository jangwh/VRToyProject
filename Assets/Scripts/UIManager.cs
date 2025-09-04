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
    public Image scoreImage;
    public Text scoreText;
    public Image bestScoreImage;
    public Text bestScoreText;

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
        scoreImage.gameObject.SetActive(true);
        bestScoreImage.gameObject.SetActive(true);
        GameManager.Instance.ClawMachine.SetActive(true);
        GameManager.Instance.GameStart();
        GameManager.Instance.XRController.transform.position = GameManager.Instance.XRControllerPos.position;
        bestScoreText.text = $"Best Score : {PlayerPrefs.GetInt("BestScore").ToString()}";
    }
    public void OnGameRetry()
    {
        SceneManager.LoadScene(0);
        GameStart.gameObject.SetActive(false);
        GameRetry.gameObject.SetActive(false);
        GameEnd.gameObject.SetActive(false);
        scoreImage.gameObject.SetActive(false);
        bestScoreImage.gameObject.SetActive(false);
        GameManager.Instance.XRController.transform.position = GameManager.Instance.XRControllerPos.position;
        GameManager.Instance.score = 0;
    }
    public void OnGameEnd()
    {
        Application.Quit();
    }
}
