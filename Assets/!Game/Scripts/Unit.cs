using UnityEngine;

public enum UnitType
{
    PotatoShooter,      // Картошкастрел
    DentistSunflower,   // Стоматолог-солнцедел
    HawthornBush,       // Кустик боярышника
    SoapSlower,         // Коммуналка-мыло
    GasBomb,            // Газовый баллон
    Collector,          // Коллектор
    FirePotato          // Огненная картошка
}

public abstract class Unit : MonoBehaviour
{
    [Header("Unit Settings")]
    public UnitType unitType;
    public int cost;
    public float cooldown;
    public float range;
    
    protected float lastActionTime;
    protected bool canAct = true;
    
    [Header("Visual")]
    public GameObject projectilePrefab;
    
    protected virtual void Start()
    {
        lastActionTime = Time.time;
    }
    
    protected virtual void Update()
    {
        if (canAct && Time.time - lastActionTime >= cooldown)
        {
            PerformAction();
            lastActionTime = Time.time;
        }
    }
    
    protected abstract void PerformAction();
    
    protected Enemy FindClosestEnemy()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Enemy closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        
        foreach (Enemy enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < range && distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }
        
        return closestEnemy;
    }
    
    protected Enemy[] FindEnemiesInRange()
    {
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();
        System.Collections.Generic.List<Enemy> enemiesInRange = new System.Collections.Generic.List<Enemy>();
        
        foreach (Enemy enemy in allEnemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) <= range)
            {
                enemiesInRange.Add(enemy);
            }
        }
        
        return enemiesInRange.ToArray();
    }
}