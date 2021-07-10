using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public GameObject[] spaceshipPrefabs;
    public Texture2D[] spaceShipTexture;
    public Image[] textureImage;

    
    public int currentSpaceshipIndex = 0;
    public GameObject currentSpaceship => spaceshipPrefabs[currentSpaceshipIndex];
    public int CurrentSpaceshipIndex => currentSpaceshipIndex;
    public int currentLevelIndex = 0;
    

    public static GameManager Instance;

    

    [ContextMenu("SpaceshipTexture")]
    public void SpaceshipTexture()
    {
        spaceShipTexture = new Texture2D[spaceshipPrefabs.Length];
#if UNITY_EDITOR
        for (int i = 0; i < spaceshipPrefabs.Length; i++)
        {
            GameObject prefab = spaceshipPrefabs[i];
            Texture2D texture = AssetPreview.GetAssetPreview(prefab);
            spaceShipTexture[i] = texture;
        }
#endif
    }

        
    
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        #if UNITY_EDITOR
        SpaceshipTexture();
        #endif
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeCurrentSpaceship(int index)
    {
        currentSpaceshipIndex = index;
    }

    
}
