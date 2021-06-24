using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] spaceshipPrefabs;
    public Texture2D[] spaceShipTexture;

    
    public int currentSpaceshipIndex = 0;
    public GameObject currentSpaceship => spaceshipPrefabs[currentSpaceshipIndex];
    public int CurrentSpaceshipIndex => currentSpaceshipIndex;
    

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        spaceShipTexture = new Texture2D[spaceshipPrefabs.Length];
        for (int i = 0; i < spaceshipPrefabs.Length; i++)
        {
            GameObject prefab = spaceshipPrefabs[i];
            Texture2D texture = AssetPreview.GetAssetPreview(prefab);
            spaceShipTexture[i] = texture;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void ChangeCurrentSpaceship(int index)
    {
        currentSpaceshipIndex = index;
    }
}
