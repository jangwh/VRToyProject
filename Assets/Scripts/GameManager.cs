using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject ClawMachine;
    public GameObject XRController;
    public Transform XRControllerPos;
    public GameObject animalPrefab;
    public Transform animalGroup;
    public Animal lastAnimal;
    public AudioSource audioSource;
    public AudioClip[] audioClip;
    public int maxSpawnLevel;
    public bool isOver;
    public int score;
    int bestScore;

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
        if (!PlayerPrefs.HasKey("BestScore"))
        {
            PlayerPrefs.SetInt("BestScore", 0);
        }
    }
    void Update()
    {
        if(isOver)
        {
            //게임오버
            Result();
        }
        UIManager.Instance.scoreText.text = $"Score : {score.ToString()}";
    }
    public void TouchDown()
    {
        if (lastAnimal == null) return;

        lastAnimal.Drag();
    }

    public void TouchUp()
    {
        if (lastAnimal == null) return;

        lastAnimal.Drop();
        lastAnimal = null;
    }
    public void GameStart()
    {
        Invoke("NextAnimal", 1.5f);
    }
    Animal GetAnimal()
    {
        int ran = Random.Range(0, maxSpawnLevel);
        GameObject instant = ObjectManager.Instance.SpawnAnimal(ran, animalGroup.position);
        audioSource.PlayOneShot(audioClip[2]);
        Animal instantAnimal = instant.GetComponent<Animal>();
        return instantAnimal;
    }
    void NextAnimal()
    {
        if (isOver)
            return;

        Animal newAnimal = GetAnimal();
        lastAnimal = newAnimal;
        score++;
        // 다음 동물 생성을 기다리는 코루틴
        StartCoroutine(WaitNext());
    }
    IEnumerator WaitNext()
    {
        // 현재 동물 드랍될 때까지 대기
        while (lastAnimal != null)
        {
            yield return null; // 한 프레임 쉼
        }

        yield return new WaitForSeconds(2f);

        // 다음 동물 생성 호출
        NextAnimal();
    }
    public void Result()
    {
        if (!isOver) return;
        // 게임 오버 및 결산
        //isOver = true;
        audioSource.Stop();
        audioSource.PlayOneShot(audioClip[1]);

        StartCoroutine("ResultRoutine");
    }

    IEnumerator ResultRoutine()
    {
        // 남아있는 동글들을 순차적으로 숨김
        Lean.Pool.LeanPool.DespawnAll();
        yield return new WaitForSeconds(0.05f);
        isOver = false;
        UIManager.Instance.GameRetry.gameObject.SetActive(true);

        bestScore = Mathf.Max(score, PlayerPrefs.GetInt("BestScore"));
        PlayerPrefs.SetInt("BestScore", bestScore);
    }
}
