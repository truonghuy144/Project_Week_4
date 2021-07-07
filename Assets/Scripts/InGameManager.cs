using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour
{
    
    public Image healthBarFill;
    public float healthBarChangeTime = 10f;

    public GameObject pauseMenu;
    public GameObject deathMenu;
    public GameObject levelCompletedMenu;

    public Text meteorToKillText;
    
    public void ChangeHealthBar(int maxHealth, int currentHealth)
    {
        if(currentHealth < 0)
            return;

        if (currentHealth == 0)
        {
            Invoke("OpenDeathMenu", healthBarChangeTime);
        }
        
        float healthPercent = currentHealth / (float) maxHealth;
        StartCoroutine(SmoothHealthBarChange(healthPercent));
    }
    
    private IEnumerator SmoothHealthBarChange(float newFillAmount)
    {
        float elapsed = 0f;
        float oldFillAmount = healthBarFill.fillAmount;

        while (elapsed <= healthBarChangeTime)
        {
            elapsed += Time.deltaTime;
            float currentFillAmount = Mathf.Lerp(oldFillAmount, newFillAmount, elapsed / healthBarChangeTime);
            healthBarFill.fillAmount = currentFillAmount;
            yield return null;
        }
    }

    
    public void OnMenuButtonClicked()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void OnQuitButtonClicked()
    {
        Debug.Log("Quit App");
        //save game
        
        Application.Quit();
    }

    public void OnPauseButtonClicked()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void OnContinueButtonClicked()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void OnRestartButtonClicked()
    {
        //save Game

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void OpenDeathMenu()
    {
        Time.timeScale = 0f;
        deathMenu.SetActive(true);
    }

    public void ChangeMeteorKillCount(int toKill)
    {
        meteorToKillText.text = "Meteor to kill: " + toKill.ToString();
    }

    public void OpenLevelComlpetedMenu()
    {
        Time.timeScale = 0f;
        levelCompletedMenu.SetActive(true);
    }
    
}
