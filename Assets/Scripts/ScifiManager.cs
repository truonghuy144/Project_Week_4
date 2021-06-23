using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScifiManager : MonoBehaviour
{
    #region Singleton

    public static ScifiManager Instance;

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
    
    public GameObject[] scifiPrefabs;

    public float scifiSpawnDistance ;

    public float spawnTime = 10f;
    private float timer = 0f;
    
    [HideInInspector]
    public float minX, maxX, minY, maxY;
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
            //spawn Sci-fi element
            SpawnNewScifi();
            timer = 0f;
        }
    }
    
    private void SpawnNewScifi()
    {
        float newX = Random.Range(minX, maxX);
        float newY = Random.Range(minY, maxY);
        
        Vector3 spawnPos = new Vector3(newX, newY, scifiSpawnDistance);

        Instantiate(scifiPrefabs[Random.Range(0, scifiPrefabs.Length)], spawnPos, Quaternion.identity);    
    }
}
