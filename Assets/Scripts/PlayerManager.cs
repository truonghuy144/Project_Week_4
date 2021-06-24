using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
   private PlayerController activePlayerController;

   private void Awake()
   {
      if (GameManager.Instance == null)
      {
         //For debugging
         Debug.Log("Debugging or testing");
         GetFirstActiveController();
      }
      else
      {
         //In real game
         SetCurrentSpaceShip();
      }
      
   }

   public void SetCurrentSpaceShip()
   {
      int currentSpaceshipIndex = GameManager.Instance.CurrentSpaceshipIndex;

      int i = 0;
      foreach (Transform spaceship in this.transform)
      {
         int currentIndex = i;
         if (currentIndex == currentSpaceshipIndex)
         {
            //activate it
            spaceship.gameObject.SetActive(true);
            activePlayerController = spaceship.GetComponent<PlayerController>();
         }
         else
         {
            //deactivate it
            spaceship.gameObject.SetActive(false);
         }

         i++;
      }
   }

   private void GetFirstActiveController()
   {
      foreach (Transform spaceship in this.transform)
      {
         activePlayerController = spaceship.GetComponent<PlayerController>();
         return;
      }
   }
   
}
