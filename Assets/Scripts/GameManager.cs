using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
    using UnityEditor;    
#endif
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public GameObject[] spaceshipPrefabs;
    public Texture2D[] spaceShipTexture;

    
    public int currentSpaceshipIndex = 0;
    public GameObject currentSpaceship => spaceshipPrefabs[currentSpaceshipIndex];
    public int CurrentSpaceshipIndex => currentSpaceshipIndex;
    

    public static GameManager Instance;

    void Start()
    {   
        #if UNITY_EDITOR
        SpaceshipTexture();
        #endif
    }
#if UNITY_EDITOR
    [ContextMenu("SpaceshipTexture")]
    public void SpaceshipTexture()
    {
        spaceShipTexture = new Texture2D[spaceshipPrefabs.Length];
        for (int i = 0; i < spaceshipPrefabs.Length; i++)
        {
            GameObject prefab = spaceshipPrefabs[i];
            Texture2D texture = AssetPreview.GetAssetPreview(prefab);
            spaceShipTexture[i] = texture;
        }
    }
#endif
        
    
    
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
