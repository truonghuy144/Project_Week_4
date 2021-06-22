using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
   public Transform levelContainer;
   public RectTransform menuContainer;
   public float transitionTime = 1f;
   private int screenWidth;

   private void Start()
   {
      InitLevelButton();
      screenWidth = Screen.width;
   }


   public void InitLevelButton()
   {
      int i = 1;
      foreach (Transform levelButton in levelContainer)
      {
         int currentIndex = i;
         Button button = levelButton.GetComponent<Button>();
         button.onClick.AddListener(() => OnLevelSelect(currentIndex));
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
      Debug.Log("We press the button of level " + index);
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
   
   private enum MenuType
   {
      MainMenu,
      Map1Menu
   }
}
