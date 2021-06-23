using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeteorManager : MonoBehaviour
{
    #region Singleton

    public static MeteorManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    #endregion
    
    public GameObject[] meteorPrefabs;

    public float meteorSpawnDistance ;

    public float spawnTime = 2f;
    private float timer = 0f;
    
    [HideInInspector]
    public float minSpawnX = -2f, maxSpawnX = 2f, minSpawnY = -1f, maxSpawnY = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        timer = spawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnTime)
        {
            //spawnMeteor
            SpawnNewMeteor();
            timer = 0f;
        }
    }

    private void SpawnNewMeteor()
    {
        float newX = Random.Range(minSpawnX, maxSpawnX);
        float newY = Random.Range(minSpawnY, maxSpawnY);
        
        Vector3 spawnPos = new Vector3(newX, newY, meteorSpawnDistance);

        Instantiate(meteorPrefabs[Random.Range(0, meteorPrefabs.Length)], spawnPos, meteorPrefabs[Random.Range(0,meteorPrefabs.Length)].transform.rotation);    
    }
}
