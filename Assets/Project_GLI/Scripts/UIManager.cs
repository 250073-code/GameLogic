using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Counters and Timer")]
    public Text scoreText;
    public Text ammoText;
    public Text enemyCountText;
    public Text timeText;


    [Header("Health and Panels")]
    public Slider healthBar;
    public Image damagePanel;
    public GameObject tutorialPanel;


    [Header("End Game UI")]
    public GameObject winPanel;
    public GameObject losePanel;
    public Text winText;
    public Text loseText;

    private int _score = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if(winPanel) winPanel.SetActive(false);
        if(losePanel) losePanel.SetActive(false);
    }

    void Start()
    {
        if (tutorialPanel != null) tutorialPanel.SetActive(true);

        Time.timeScale = 0f; // Stop game while showing tutorial

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Hide red panel
        if (damagePanel != null)
        {
            Color c = damagePanel.color;
            c.a = 0f;
            damagePanel.color = c;
        }

        UpdateScore(0);
        UpdateEnemyCount(0);
        UpdateTimeDisplay(0);
    }

    public void CloseTutorial()
    {
        if (tutorialPanel != null) tutorialPanel.SetActive(false);

        Time.timeScale = 1f; // Return time to normal
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UpdateScore(int amount)
    {
        _score += amount;
        if (scoreText != null) scoreText.text = "Score: " + _score;
    }

    public void UpdateAmmo(int current, int max)
    {
        if (ammoText != null)
        {
            ammoText.text = current + " / " + max;
            ammoText.color = (current == 0) ? Color.red : Color.white;
        }
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    public void UpdateEnemyCount(int count)
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = "Enemies: " + count;
        }
    }

    public void UpdateTimeDisplay(float timeInSeconds)
    {
        if (timeText != null)
        {
            // Convert seconds to 00:00 format
            float minutes = Mathf.FloorToInt(timeInSeconds / 60);
            float seconds = Mathf.FloorToInt(timeInSeconds % 60);
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void ShowDamageFlash()
    {
        if (damagePanel != null)
        {
            StopAllCoroutines();
            StartCoroutine(FlashRoutine());
        }
    }

    public void ShowWin() 
    {
        winPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine(FlickerText(winText));
    }

    public void ShowLose() 
    {
        StopAllCoroutines();

        if (damagePanel != null)
        {
            Color c = damagePanel.color;
            c.a = 0f;
            damagePanel.color = c;
        }

        losePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine(FlickerText(loseText));
        StopCoroutine(FlashRoutine());
        Time.timeScale = 0f; // Stop game
    }   

    // Coroutine for flickering
    IEnumerator FlickerText(Text targetText)
    {
        if (targetText == null) yield break;
        while (true)
        {
            targetText.enabled = !targetText.enabled;
            yield return new WaitForSecondsRealtime(0.5f); 
        }
    }

    IEnumerator FlashRoutine()
    {
        Color c = damagePanel.color;
        c.a = 0.5f; // How red the screen will be (0.5 = 50%)
        damagePanel.color = c;

        while (c.a > 0f)
        {
            c.a -= Time.deltaTime * 2f;
            damagePanel.color = c;
            yield return null;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Return time to normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}