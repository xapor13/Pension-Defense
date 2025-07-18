using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoShooter : Unit
{
    [Header("Potato Shooter Settings")]
    public int damage = 50;
    
    protected override void PerformAction()
    {
        Enemy target = FindClosestEnemy();
        if (target != null)
        {
            ShootAt(target);
        }
    }
    
    private void ShootAt(Enemy target)
    {
        if (projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            Projectile projScript = projectile.GetComponent<Projectile>();
            if (projScript != null)
            {
                projScript.Initialize(target.transform, damage);
            }
        }
    }
}
