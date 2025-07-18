using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
   [Header("Fire Settings")]
    public float burnDuration = 3f;
    
    public void Initialize(Transform targetTransform, int projectileDamage, float burnTime)
    {
        base.Initialize(targetTransform, projectileDamage);
        burnDuration = burnTime;
    }
    
    protected override void OnHitTarget()
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            enemy.ApplyBurn(burnDuration, damage / 3); // Горение наносит 1/3 от урона каждую секунду
        }
        
        CreateFireEffect();
        Destroy(gameObject);
    }
    
    protected override void CreateFireEffect()
    {
        // Эффект огня
        GameObject fireEffect = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        fireEffect.transform.position = transform.position;
        fireEffect.transform.localScale = Vector3.one * 0.7f;
        fireEffect.GetComponent<Renderer>().material.color = Color.red;
        
        // Убираем коллайдер
        Destroy(fireEffect.GetComponent<Collider>());
        
        // Анимация огня
        StartCoroutine(FireAnimation(fireEffect));
    }
    
    private System.Collections.IEnumerator FireAnimation(GameObject fireEffect)
    {
        float time = 0f;
        Vector3 originalScale = fireEffect.transform.localScale;
        
        while (time < 1f)
        {
            time += Time.deltaTime * 2f;
            fireEffect.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, time);
            fireEffect.GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.yellow, time);
            yield return null;
        }
        
        Destroy(fireEffect);
    }
}
