using UnityEngine;
using System.Collections;

public enum EnemyType
{
    Regular,        // Обычный пенсионер
    WithWalker,     // Пенсионер с ходунками
    Armored         // Пенсионер-броневик
}

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public EnemyType enemyType;
    public float speed = 2f;
    public int maxHealth = 100;
    public int currentHealth;
    public int goldenTeethReward = 10;
    
    [Header("Movement")]
    public Vector3 targetPosition;
    
    [Header("Visual")]
    public GameObject bloodEffect;
    public GameObject deathEffect;
    
    private bool isDead = false;
    private bool isBurning = false;
    private Renderer rend;
    
    // Системы статусов
    private float burnDamagePerSecond = 0f;
    private float burnTimeLeft = 0f;
    
    private void Start()
    {
        currentHealth = maxHealth;
        rend = GetComponent<Renderer>();
        
        // Устанавливаем параметры в зависимости от типа врага
        SetupEnemyType();
    }
    
    private void SetupEnemyType()
    {
        switch (enemyType)
        {
            case EnemyType.Regular:
                maxHealth = 100;
                speed = 2f;
                goldenTeethReward = 10;
                if (rend != null) rend.material.color = Color.gray;
                break;
                
            case EnemyType.WithWalker:
                maxHealth = 200;
                speed = 1f;
                goldenTeethReward = 20;
                if (rend != null) rend.material.color = Color.blue;
                break;
                
            case EnemyType.Armored:
                maxHealth = 400;
                speed = 1.5f;
                goldenTeethReward = 40;
                if (rend != null) rend.material.color = Color.black;
                break;
        }
        
        currentHealth = maxHealth;
    }
    
    private void Update()
    {
        if (isDead) return;
        
        // Движение к цели
        MoveToTarget();
        
        // Обработка горения
        if (isBurning)
        {
            ProcessBurn();
        }
        
        // Проверка достижения цели
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            ReachTarget();
        }
    }
    
    private void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
    
    private void ProcessBurn()
    {
        burnTimeLeft -= Time.deltaTime;
        
        if (burnTimeLeft <= 0f)
        {
            StopBurn();
        }
        else
        {
            // Наносим урон от горения каждую секунду
            if (Time.time % 1f < Time.deltaTime)
            {
                TakeDamage(Mathf.RoundToInt(burnDamagePerSecond));
            }
        }
    }
    
    private void StopBurn()
    {
        isBurning = false;
        burnTimeLeft = 0f;
        burnDamagePerSecond = 0f;
        
        // Возвращаем нормальный цвет
        if (rend != null)
        {
            SetupEnemyType(); // Восстанавливаем оригинальный цвет
        }
    }
    
    public void TakeDamage(int damage)
    {
        if (isDead) return;
        
        currentHealth -= damage;
        
        // Эффект получения урона
        CreateBloodEffect();
        
        // Визуальная индикация урона
        if (rend != null)
        {
            StartCoroutine(DamageFlash());
        }
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void ApplyBurn(float duration, float damagePerSecond)
    {
        isBurning = true;
        burnTimeLeft = duration;
        burnDamagePerSecond = damagePerSecond;
        
        // Визуальный эффект горения
        if (rend != null)
        {
            rend.material.color = Color.Lerp(rend.material.color, Color.red, 0.5f);
        }
    }
    
    private void CreateBloodEffect()
    {
        if (bloodEffect != null)
        {
            Instantiate(bloodEffect, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
        else
        {
            // Простой эффект крови
            GameObject blood = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            blood.transform.position = transform.position + Vector3.up * 0.5f;
            blood.transform.localScale = Vector3.one * 0.2f;
            blood.GetComponent<Renderer>().material.color = Color.red;
            Destroy(blood.GetComponent<Collider>());
            Destroy(blood, 1f);
        }
    }
    
    private System.Collections.IEnumerator DamageFlash()
    {
        Color originalColor = rend.material.color;
        rend.material.color = Color.white;
        
        yield return new WaitForSeconds(0.1f);
        
        rend.material.color = originalColor;
    }
    
    private void Die()
    {
        if (isDead) return;
        
        isDead = true;
        
        // Даем награду игроку
        ResourceManager.Instance.AddGoldenTeeth(goldenTeethReward);
        
        // Эффект смерти
        CreateDeathEffect();
        
        // Уничтожаем врага
        Destroy(gameObject);
    }
    
    private void CreateDeathEffect()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        else
        {
            // Простой эффект смерти
            GameObject death = GameObject.CreatePrimitive(PrimitiveType.Cube);
            death.transform.position = transform.position;
            death.transform.localScale = Vector3.one * 0.5f;
            death.GetComponent<Renderer>().material.color = Color.black;
            Destroy(death.GetComponent<Collider>());
            Destroy(death, 2f);
        }
    }
    
    private void ReachTarget()
    {
        // Враг достиг цели - игрок проиграл
        GameManager.Instance.OnEnemyReachedTarget();
        Destroy(gameObject);
    }
    
    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
    }
}