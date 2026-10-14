using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI enemiesText;
    [SerializeField] private GameObject gameCompletePanel;

    private void OnEnable()
    {
        waveManager.OnWaveStarted += UpdateWaveText;
        waveManager.OnEnemiesCountChanged += UpdateEnemiesText;
        waveManager.OnAllWavesCompleted += ShowGameComplete;
    }

    private void OnDisable()
    {
        waveManager.OnWaveStarted -= UpdateWaveText;
        waveManager.OnEnemiesCountChanged -= UpdateEnemiesText;
        waveManager.OnAllWavesCompleted -= ShowGameComplete;
    }

    private void Start()
    {
        gameCompletePanel.SetActive(false);
    }

    private void UpdateWaveText(int currentWave, int totalInWave)
    {
        waveText.text = $"Wave: {currentWave}";
    }

    private void UpdateEnemiesText(int aliveEnemies)
    {
        enemiesText.text = $"Enemies: {aliveEnemies}";
    }

    private void ShowGameComplete()
    {
        gameCompletePanel.SetActive(true);
    }
}