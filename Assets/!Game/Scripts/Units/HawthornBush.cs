using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HawthornBush : Unit
{
    [Header("Hawthorn Settings")]
    public float slowAmount = 0.5f;
    public float slowDuration = 2f;
    
    protected override void PerformAction()
    {
        Enemy[] enemies = FindEnemiesInRange();
        foreach (Enemy enemy in enemies)
        {
            SlowEnemy(enemy);
        }
    }
    
    private void SlowEnemy(Enemy enemy)
    {
        // Применяем замедление
        enemy.speed *= slowAmount;
        
        // Возвращаем скорость через время
        Invoke(nameof(RestoreSpeed), slowDuration);
    }
    
    private void RestoreSpeed()
    {
        // Находим всех врагов и возвращаем скорость
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            if (enemy.speed < 2f) // Если скорость была замедлена
            {
                enemy.speed = 2f; // Возвращаем базовую скорость
            }
        }
    }
}
