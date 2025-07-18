using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    
    [Header("Resources")]
    public int goldenTeeth = 200; // Стартовые зубы
    
    [Header("UI")]
    public Text goldenTeethText;
    public Text messageText;
    
    [Header("Unit Costs")]
    public int potatoShooterCost = 100;
    public int dentistSunflowerCost = 50;
    public int hawthornBushCost = 75;
    public int soapSlowerCost = 125;
    public int gasBombCost = 150;
    public int collectorCost = 125;
    public int firePotatoCost = 175;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        UpdateUI();
    }
    
    public void AddGoldenTeeth(int amount)
    {
        goldenTeeth += amount;
        UpdateUI();
        
        ShowMessage($"+{amount} зубов!", Color.yellow);
    }
    
    public bool SpendGoldenTeeth(int amount)
    {
        if (goldenTeeth >= amount)
        {
            goldenTeeth -= amount;
            UpdateUI();
            return true;
        }
        else
        {
            ShowMessage("Недостаточно золотых зубов!", Color.red);
            return false;
        }
    }
    
    public bool CanAfford(UnitType unitType)
    {
        int cost = GetUnitCost(unitType);
        return goldenTeeth >= cost;
    }
    
    public int GetUnitCost(UnitType unitType)
    {
        switch (unitType)
        {
            case UnitType.PotatoShooter: return potatoShooterCost;
            case UnitType.DentistSunflower: return dentistSunflowerCost;
            case UnitType.HawthornBush: return hawthornBushCost;
            case UnitType.SoapSlower: return soapSlowerCost;
            case UnitType.GasBomb: return gasBombCost;
            case UnitType.Collector: return collectorCost;
            case UnitType.FirePotato: return firePotatoCost;
            default: return 0;
        }
    }
    
    private void UpdateUI()
    {
        if (goldenTeethText != null)
        {
            goldenTeethText.text = $"Золотые зубы: {goldenTeeth}";
        }
    }
    
    private void ShowMessage(string message, Color color)
    {
        if (messageText != null)
        {
            messageText.text = message;
            messageText.color = color;
            
            // Очищаем сообщение через 2 секунды
            Invoke(nameof(ClearMessage), 2f);
        }
    }
    
    private void ClearMessage()
    {
        if (messageText != null)
        {
            messageText.text = "";
        }
    }
    
    public int GetGoldenTeeth()
    {
        return goldenTeeth;
    }
}