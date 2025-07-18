using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 10f;
    public int damage = 50;
    
    protected Transform target;
    protected bool hasTarget = false;
    
    public virtual void Initialize(Transform targetTransform, int projectileDamage)
    {
        target = targetTransform;
        damage = projectileDamage;
        hasTarget = true;
        
        // Поворачиваем снаряд в сторону цели
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
    
    protected virtual void Update()
    {
        if (!hasTarget || target == null)
        {
            Destroy(gameObject);
            return;
        }
        
        MoveToTarget();
    }
    
    protected virtual void MoveToTarget()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        
        if (distance < 0.1f)
        {
            OnHitTarget();
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
    
    protected virtual void OnHitTarget()
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        
        CreateHitEffect();
        Destroy(gameObject);
    }
    
    protected virtual void CreateHitEffect()
    {
        // Простой эффект попадания
        GameObject hitEffect = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        hitEffect.transform.position = transform.position;
        hitEffect.transform.localScale = Vector3.one * 0.5f;
        hitEffect.GetComponent<Renderer>().material.color = Color.yellow;
        
        // Убираем коллайдер и добавляем небольшой взрыв
        Destroy(hitEffect.GetComponent<Collider>());
        Destroy(hitEffect, 0.5f);
    }
}
