using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public GameObject SpawnAnimal(int index, Vector3 position)
    {
        return LeanPool.Spawn(AniPrefab[index], position, Quaternion.identity);
    }
}