using System.Collections;
using UnityEngine;

[System.Serializable]
public class EnemyWave
{
    public GameObject[] enemyPrefabs; // Массив префабов врагов
    public int count;                 // Кол-во врагов в волне
    public float spawnInterval;       // Интервал между спавном врагов
}

public class EnemySpawner : MonoBehaviour
{
    public EnemyWave[] waves;
    public float timeBetweenWaves = 5f;
    public int minLane = 0;
    public int maxLane = 4;
    public float enemyHeightOffset = 0.5f;

    private int currentWaveIndex = 0;

    private void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        while (currentWaveIndex < waves.Length)
        {
            EnemyWave wave = waves[currentWaveIndex];

            for (int i = 0; i < wave.count; i++)
            {
                int randomLane = Random.Range(minLane, maxLane + 1);

                // Выбираем случайный префаб из массива
                if (wave.enemyPrefabs.Length == 0)
                {
                    Debug.LogWarning("EnemyWave: Нет префабов врагов в волне!");
                    yield break;
                }
                GameObject enemyPrefab = wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Length)];

                SpawnEnemy(enemyPrefab, randomLane);

                yield return new WaitForSeconds(wave.spawnInterval);
            }

            currentWaveIndex++;
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab, int lane)
    {
        Vector3 spawnPos = new Vector3(
            GridManager.Instance.width * GridManager.Instance.cellSize + 1f,
            enemyHeightOffset,
            lane * GridManager.Instance.cellSize);

        GameObject enemyGO = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        Enemy enemyScript = enemyGO.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            Vector3 targetPos = new Vector3(-1f, enemyHeightOffset, lane * GridManager.Instance.cellSize);
            enemyScript.SetTarget(targetPos);
        }
    }
}
