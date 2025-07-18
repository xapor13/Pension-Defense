using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    Paused,
    GameOver,
    Victory
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Game State")]
    public GameState currentState = GameState.Playing;
    
    [Header("UI Panels")]
    public GameObject gameOverPanel;
    public GameObject victoryPanel;
    public GameObject pausePanel;
    public GameObject unitSelectionPanel;
    
    [Header("UI Text")]
    public Text gameOverText;
    public Text victoryText;
    public Text waveText;
    
    [Header("Game Settings")]
    public int totalWaves = 10;
    private int currentWave = 0;
    private bool gameEnded = false;
    
    [Header("Unit Selection")]
    public UnitType selectedUnitType = UnitType.PotatoShooter;
    public GameObject[] unitPrefabs; // Префабы юнитов в том же порядке, что и enum
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        currentState = GameState.Playing;
        UpdateWaveUI();
        
        // Убеждаемся, что все панели скрыты в начале
        HideAllPanels();
    }
    
    private void Update()
    {
        HandleInput();
        CheckVictoryCondition();
    }
    
    private void HandleInput()
    {
        if (currentState != GameState.Playing) return;
        
        // Пауза
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        
        // Быстрый выбор юнитов
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectUnit(UnitType.PotatoShooter);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectUnit(UnitType.DentistSunflower);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectUnit(UnitType.HawthornBush);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectUnit(UnitType.SoapSlower);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectUnit(UnitType.GasBomb);
        if (Input.GetKeyDown(KeyCode.Alpha6)) SelectUnit(UnitType.Collector);
        if (Input.GetKeyDown(KeyCode.Alpha7)) SelectUnit(UnitType.FirePotato);
    }
    
    public void SelectUnit(UnitType unitType)
    {
        selectedUnitType = unitType;
        Debug.Log($"Выбран юнит: {unitType}");
    }
    
    public void OnEnemyReachedTarget()
    {
        if (gameEnded) return;
        
        GameOver("Пенсионер добрался до почты! Поражение!");
    }
    
    public void OnWaveCompleted()
    {
        currentWave++;
        UpdateWaveUI();
        
        if (currentWave >= totalWaves)
        {
            Victory();
        }
    }
    
    private void CheckVictoryCondition()
    {
        // Проверяем, завершились ли все волны и не осталось ли врагов
        if (currentWave >= totalWaves && FindObjectsOfType<Enemy>().Length == 0)
        {
            if (currentState == GameState.Playing)
            {
                Victory();
            }
        }
    }
    
    private void GameOver(string reason)
    {
        if (gameEnded) return;
        
        gameEnded = true;
        currentState = GameState.GameOver;
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        
        if (gameOverText != null)
        {
            gameOverText.text = reason;
        }
        
        Time.timeScale = 0f; // Останавливаем игру
    }
    
    private void Victory()
    {
        if (gameEnded) return;
        
        gameEnded = true;
        currentState = GameState.Victory;
        
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
        
        if (victoryText != null)
        {
            victoryText.text = "Почта защищена! Победа!";
        }
        
        Time.timeScale = 0f; // Останавливаем игру
    }
    
    public void TogglePause()
    {
        if (currentState == GameState.Playing)
        {
            currentState = GameState.Paused;
            Time.timeScale = 0f;
            
            if (pausePanel != null)
            {
                pausePanel.SetActive(true);
            }
        }
        else if (currentState == GameState.Paused)
        {
            currentState = GameState.Playing;
            Time.timeScale = 1f;
            
            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }
        }
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    
    private void HideAllPanels()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (unitSelectionPanel != null) unitSelectionPanel.SetActive(true);
    }
    
    private void UpdateWaveUI()
    {
        if (waveText != null)
        {
            waveText.text = $"Волна: {currentWave + 1}/{totalWaves}";
        }
    }
    
    public GameObject GetUnitPrefab(UnitType unitType)
    {
        int index = (int)unitType;
        if (index >= 0 && index < unitPrefabs.Length)
        {
            return unitPrefabs[index];
        }
        return null;
    }
    
    public bool IsGamePlaying()
    {
        return currentState == GameState.Playing;
    }
}