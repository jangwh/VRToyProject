using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject animalPrefab;
    public Transform animalGroup;
    public Animal lastAnimal;
    public AudioSource audioSource;
    public AudioClip[] audioClip;
    public int maxSpawnLevel;
    public bool isOver;

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
    }
        void Start()
    {
        // 게임 시작 세팅
        GameStart();
    }
    void Update()
    {
        if(isOver)
        {
            //게임오버
            Result();
        }
    }
    public void TouchDown()
    {
        if (lastAnimal == null) return;

        //lastAnimal.Drag();
    }

    public void TouchUp()
    {
        if (lastAnimal == null) return;

        lastAnimal.Drop();
        lastAnimal = null;
    }
    void GameStart()
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
    }
}
