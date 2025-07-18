using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : Unit
{
    [Header("Collector Settings")]
    public int maxCapacity = 3;
    private int currentCapacity = 0;
    
    protected override void PerformAction()
    {
        if (currentCapacity < maxCapacity)
        {
            Enemy target = FindClosestEnemy();
            if (target != null)
            {
                SwallowEnemy(target);
            }
        }
    }
    
    private void SwallowEnemy(Enemy enemy)
    {
        // Увеличиваем счетчик проглоченных врагов
        currentCapacity++;
        
        // Уничтожаем врага
        Destroy(enemy.gameObject);
        
        // Визуальный эффект поглощения
        transform.localScale += Vector3.one * 0.1f;
        
        // Если достигли максимума, коллектор больше не работает
        if (currentCapacity >= maxCapacity)
        {
            canAct = false;
            GetComponent<Renderer>().material.color = Color.red;
        }
    }
}
