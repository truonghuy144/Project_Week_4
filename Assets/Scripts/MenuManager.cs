using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
   public Transform levelContainer;
   public RectTransform menuContainer;
   public float transitionTime = 1f;
   private int screenWidth;

   public Transform shopButtonsParent;
   private GameObject currentSpaceshipPreview = null;
   public float rotationSpeed = 10f;

   public Text goldText;

   private void Start()
   {
      InitLevelButton();
      screenWidth = Screen.width;
      InitShopButton();
      UpdateSpaceshipPreview();
      UpdateGoldText();
   }

   private void Update()
   {
      if (currentSpaceshipPreview != null)
      {
         currentSpaceshipPreview.transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
      }
   }

   private void UpdateSpaceshipPreview()
   {
      if (currentSpaceshipPreview != null)
      {
         Destroy(currentSpaceshipPreview);
      }

      GameObject newSpaceshipPrefab = GameManager.Instance.currentSpaceship;
      Vector3 startRotationVector = new Vector3(0f,360f,0f); 
      currentSpaceshipPreview = Instantiate(newSpaceshipPrefab, Vector3.zero, Quaternion.Euler(startRotationVector));

   }
   
   private void InitShopButton()
   {
      int i = 0;
      foreach (Transform shopButton in shopButtonsParent)
      {
         int currentIndex = i;
         
         //create sprites
         Texture2D texture = GameManager.Instance.spaceShipTexture[currentIndex];
         Rect newRect = new Rect(0f,0f,texture.width,texture.height);
         Sprite newSprite = Sprite.Create(texture,newRect,new Vector2(0.5f,0.5f));
         shopButton.GetComponent<Image>().sprite = newSprite;

         //Onclick Event
         Button button = shopButton.GetComponent<Button>();
         button.onClick.AddListener(() => OnShopButtonClicked(currentIndex));
         i++;
      }
   }

   private void OnShopButtonClicked(int index)
   {
      GameManager.Instance.ChangeCurrentSpaceship(index);
      UpdateSpaceshipPreview();
   }
   
   public void InitLevelButton()
   {
      int lastLevelCompleted = SaveManager.Instance.GetLevelCompleted();
      
      int i = 0;
      foreach (Transform levelButton in levelContainer)
      {
         int currentIndex = i;
         Button button = levelButton.GetComponent<Button>();
         if (currentIndex <= lastLevelCompleted)
         {
            //complete level
            button.onClick.AddListener(() => OnLevelSelect(currentIndex));
            button.image.color = Color.white;
         }
         else if(currentIndex == lastLevelCompleted + 1)
         {
            // the current level to be completed
            button.onClick.AddListener(() => OnLevelSelect(currentIndex));
            button.image.color = Color.blue;
         }
         else
         {
            //not complete
            button.interactable = false;
            button.image.color = Color.gray;
         }
         
         
         i++;
      }
   }

   private void ChangeMenu(MenuType menuType)
   {
      Vector3 menuPos;
      if (menuType == MenuType.Map1Menu)
      {
         menuPos = new Vector3(-screenWidth, 0f,0f);
      }
      else if (menuType == MenuType.ShopMenu)
      {
         menuPos = new Vector3(screenWidth, 0f,0f);
      }
      //default
      else
      {
         menuPos = Vector3.zero;
      }
      
      StopAllCoroutines();
      StartCoroutine(changeMenuAnimation(menuPos));
   }

   private IEnumerator changeMenuAnimation(Vector3 newPos)
   {
      float elapsed = 0f;
      Vector3 oldPos = menuContainer.anchoredPosition3D;

      while (elapsed <= transitionTime)
      {
         elapsed += Time.deltaTime;
         Vector3 currentPos = Vector3.Lerp(oldPos, newPos, elapsed / transitionTime);
         menuContainer.anchoredPosition3D = currentPos;
         yield return null;
      }
   }
   private void OnLevelSelect(int index)
   {
      GameManager.Instance.currentLevelIndex = index;
      
      int LevelIndex = index + 1;
      string sceneName = "Level" + LevelIndex.ToString();
      SceneManager.LoadScene(sceneName);
   }
   
   public void OnStartButtonClicked()
   {
      Debug.Log("Start Button Clicked");
      ChangeMenu(MenuType.Map1Menu);
   }

   public void OnMainMenuClicked()
   {
      Debug.Log("Main Button Clicked");
      ChangeMenu(MenuType.MainMenu);
   }

   public void OnNextMapButtonClicked()
   {
      Debug.Log("Next Map Clicked");
   }

   public void OnShopButtonClicked()
   {
      ChangeMenu(MenuType.ShopMenu);
   }

   private void UpdateGoldText()
   {
      goldText.text = SaveManager.Instance.getGold().ToString();
   }
   
   private enum MenuType
   {
      MainMenu,
      Map1Menu,
      ShopMenu
   }
}
