using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasBomb : Unit
{
    [Header("Gas Bomb Settings")]
    public int explosionDamage = 100;
    public float explosionRadius = 3f;
    public GameObject explosionEffect;
    
    protected override void PerformAction()
    {
        Enemy[] enemies = FindEnemiesInRange();
        if (enemies.Length > 0)
        {
            Explode();
        }
    }
    
    private void Explode()
    {
        // Создаем эффект взрыва
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        
        // Наносим урон всем врагам в радиусе
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider col in colliders)
        {
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(explosionDamage);
            }
        }
        
        // Уничтожаем газовый баллон после взрыва
        Destroy(gameObject);
    }
}
