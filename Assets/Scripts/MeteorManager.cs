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

    public float goldenMeteorSpawnChange = 0.2f;
    public GameObject goldenMeteorPrefab;

    public float spawnTime = 2f;
    private float timer = 0f;

    public int meteorToFinish = 5;
    public InGameManager inGameManager;
    public float speedIncrease = 1.2f;
    private int meteorControl = 0;
    
    [HideInInspector]
    public List<GameObject> aliveMeteor  = new List<GameObject>();
    
    [HideInInspector]
    public float minSpawnX = 0f, maxSpawnX = 1f, minSpawnY = 0f, maxSpawnY = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        timer = spawnTime;
        inGameManager.ChangeMeteorKillCount(meteorToFinish);
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

    public void OnMeteorKill(GameObject meteor)
    {
        meteorToFinish--;
        aliveMeteor.Remove(meteor);

        if (meteorToFinish <= 0)
        {
            if (GameManager.Instance != null)
            {
                int thisLevelIndex = GameManager.Instance.currentLevelIndex;
                int lastLevelCompleted = SaveManager.Instance.GetLevelCompleted();
                if (thisLevelIndex >= lastLevelCompleted)
                {
                    SaveManager.Instance.CompletedNextLevel();
                }
                
                inGameManager.OpenLevelComlpetedMenu();
            }
            else
            {
                inGameManager.ChangeMeteorKillCount(meteorToFinish);
            }
        }
    }
    
    private void SpawnNewMeteor()
    {
        float newX = Random.Range(minSpawnX, maxSpawnX);
        float newY = Random.Range(minSpawnY, maxSpawnY);
        
        Vector3 spawnPos = new Vector3(newX, newY, meteorSpawnDistance);

        float randomNumber = Random.Range(0f, 1f);

        GameObject gameObject = null;
        if (randomNumber < goldenMeteorSpawnChange)
        {
            // spawn golden meteor
            gameObject = Instantiate(goldenMeteorPrefab, spawnPos, goldenMeteorPrefab.transform.rotation);
        }
        else
        {
            // spawn normal meteor
            int meteorNumber = Random.Range(0, meteorPrefabs.Length);
            gameObject = Instantiate(meteorPrefabs[meteorNumber], spawnPos, meteorPrefabs[meteorNumber].transform.rotation);
        }
        
        gameObject.GetComponent<MeteorController>().IncreaseSpeed(meteorControl * speedIncrease);
        meteorControl++;
        aliveMeteor.Add(gameObject);
    }

    public void UpdateMeteors(List<GameObject> targetedMeteors)
    {
        foreach (GameObject meteor in aliveMeteor)
        {
            if (targetedMeteors.Contains(meteor))
            {
                //change material
                meteor.GetComponent<MeteorController>().SetTargetMaterial();
            }
            else
            {
                //reset it
                meteor.GetComponent<MeteorController>().ResetMaterial();
            }
        }
    }
}
