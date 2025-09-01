using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Lean.Pool;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Instance { get; private set; }
    public GameObject[] AniPrefab;

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
        PrewarmPool(AniPrefab[0], 50);
        PrewarmPool(AniPrefab[1], 50);
        PrewarmPool(AniPrefab[2], 50);
        PrewarmPool(AniPrefab[3], 50);
        PrewarmPool(AniPrefab[4], 50);
        PrewarmPool(AniPrefab[5], 50);
        PrewarmPool(AniPrefab[6], 50);
        PrewarmPool(AniPrefab[7], 50);
        PrewarmPool(AniPrefab[8], 50);
    }
    void PrewarmPool(GameObject gameObject, int count)
    {
        for (int i = 0; i < count; i++)
        {
            var obj = LeanPool.Spawn(gameObject, Vector3.zero, Quaternion.identity);
            obj.gameObject.SetActive(false);
            LeanPool.Despawn(obj);
        }
    }
    public GameObject SpawnAnimal00(Vector3 position)
    {
        return LeanPool.Spawn(AniPrefab[0], position, Quaternion.identity);
    }
    public GameObject SpawnAnimal01(Vector3 position)
    {
        return LeanPool.Spawn(AniPrefab[1], position, Quaternion.identity);
    }
    public GameObject SpawnAnimal02(Vector3 position)
    {
        return LeanPool.Spawn(AniPrefab[2], position, Quaternion.identity);
    }
    public GameObject SpawnAnimal03(Vector3 position)
    {
        return LeanPool.Spawn(AniPrefab[3], position, Quaternion.identity);
    }
    public GameObject SpawnAnimal04(Vector3 position)
    {
        return LeanPool.Spawn(AniPrefab[4], position, Quaternion.identity);
    }
    public GameObject SpawnAnimal05(Vector3 position)
    {
        return LeanPool.Spawn(AniPrefab[5], position, Quaternion.identity);
    }
    public GameObject SpawnAnimal06(Vector3 position)
    {
        return LeanPool.Spawn(AniPrefab[6], position, Quaternion.identity);
    }
    public GameObject SpawnAnimal07(Vector3 position)
    {
        return LeanPool.Spawn(AniPrefab[7], position, Quaternion.identity);
    }
    public GameObject SpawnAnimal08(Vector3 position)
    {
        return LeanPool.Spawn(AniPrefab[8], position, Quaternion.identity);
    }
}