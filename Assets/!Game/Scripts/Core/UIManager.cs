using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [Header("Unit Selection Buttons")]
    public Button potatoShooterButton;
    public Button dentistSunflowerButton;
    public Button hawthornBushButton;
    public Button soapSlowerButton;
    public Button gasBombButton;
    public Button collectorButton;
    public Button firePotatoButton;
    
    [Header("Cost Labels")]
    public Text potatoShooterCost;
    public Text dentistSunflowerCost;
    public Text hawthornBushCost;
    public Text soapSlowerCost;
    public Text gasBombCost;
    public Text collectorCost;
    public Text firePotatoCost;
    
    [Header("Selection Visual")]
    public Color selectedColor = Color.yellow;
    public Color normalColor = Color.white;
    public Color cannotAffordColor = Color.red;
    
    private Button currentSelectedButton;
    
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
        SetupButtons();
        UpdateCostLabels();
        SelectButton(potatoShooterButton, UnitType.PotatoShooter);
    }
    
    private void Update()
    {
        UpdateButtonStates();
    }
    
    private void SetupButtons()
    {
        if (potatoShooterButton != null)
            potatoShooterButton.onClick.AddListener(() => SelectButton(potatoShooterButton, UnitType.PotatoShooter));
        
        if (dentistSunflowerButton != null)
            dentistSunflowerButton.onClick.AddListener(() => SelectButton(dentistSunflowerButton, UnitType.DentistSunflower));
        
        if (hawthornBushButton != null)
            hawthornBushButton.onClick.AddListener(() => SelectButton(hawthornBushButton, UnitType.HawthornBush));
        
        if (soapSlowerButton != null)
            soapSlowerButton.onClick.AddListener(() => SelectButton(soapSlowerButton, UnitType.SoapSlower));
        
        if (gasBombButton != null)
            gasBombButton.onClick.AddListener(() => SelectButton(gasBombButton, UnitType.GasBomb));
        
        if (collectorButton != null)
            collectorButton.onClick.AddListener(() => SelectButton(collectorButton, UnitType.Collector));
        
        if (firePotatoButton != null)
            firePotatoButton.onClick.AddListener(() => SelectButton(firePotatoButton, UnitType.FirePotato));
    }
    
    private void SelectButton(Button button, UnitType unitType)
    {
        // Сброс предыдущего выбора
        if (currentSelectedButton != null)
        {
            var colors = currentSelectedButton.colors;
            colors.normalColor = normalColor;
            currentSelectedButton.colors = colors;
        }
        
        // Выбор нового юнита
        currentSelectedButton = button;
        GameManager.Instance.SelectUnit(unitType);
        
        // Подсвечиваем выбранную кнопку
        var selectedColors = button.colors;
        selectedColors.normalColor = selectedColor;
        button.colors = selectedColors;
    }
    
    private void UpdateButtonStates()
    {
        UpdateButtonAffordability(potatoShooterButton, UnitType.PotatoShooter);
        UpdateButtonAffordability(dentistSunflowerButton, UnitType.DentistSunflower);
        UpdateButtonAffordability(hawthornBushButton, UnitType.HawthornBush);
        UpdateButtonAffordability(soapSlowerButton, UnitType.SoapSlower);
        UpdateButtonAffordability(gasBombButton, UnitType.GasBomb);
        UpdateButtonAffordability(collectorButton, UnitType.Collector);
        UpdateButtonAffordability(firePotatoButton, UnitType.FirePotato);
    }
    
    private void UpdateButtonAffordability(Button button, UnitType unitType)
    {
        if (button == null) return;
        
        bool canAfford = ResourceManager.Instance.CanAfford(unitType);
        button.interactable = canAfford;
        
        // Если это не выбранная кнопка, меняем цвет в зависимости от доступности
        if (button != currentSelectedButton)
        {
            var colors = button.colors;
            colors.normalColor = canAfford ? normalColor : cannotAffordColor;
            button.colors = colors;
        }
    }
    
    private void UpdateCostLabels()
    {
        if (potatoShooterCost != null)
            potatoShooterCost.text = ResourceManager.Instance.GetUnitCost(UnitType.PotatoShooter).ToString();
        
        if (dentistSunflowerCost != null)
            dentistSunflowerCost.text = ResourceManager.Instance.GetUnitCost(UnitType.DentistSunflower).ToString();
        
        if (hawthornBushCost != null)
            hawthornBushCost.text = ResourceManager.Instance.GetUnitCost(UnitType.HawthornBush).ToString();
        
        if (soapSlowerCost != null)
            soapSlowerCost.text = ResourceManager.Instance.GetUnitCost(UnitType.SoapSlower).ToString();
        
        if (gasBombCost != null)
            gasBombCost.text = ResourceManager.Instance.GetUnitCost(UnitType.GasBomb).ToString();
        
        if (collectorCost != null)
            collectorCost.text = ResourceManager.Instance.GetUnitCost(UnitType.Collector).ToString();
        
        if (firePotatoCost != null)
            firePotatoCost.text = ResourceManager.Instance.GetUnitCost(UnitType.FirePotato).ToString();
    }
    
    public void ShowUnitInfo(UnitType unitType)
    {
        string info = GetUnitDescription(unitType);
        // Здесь можно показать информацию о юните в отдельном окне
        Debug.Log($"Информация о юните: {info}");
    }
    
    private string GetUnitDescription(UnitType unitType)
    {
        switch (unitType)
        {
            case UnitType.PotatoShooter:
                return "Картошкастрел - стреляет картошкой по врагам";
            case UnitType.DentistSunflower:
                return "Стоматолог-солнцедел - производит золотые зубы";
            case UnitType.HawthornBush:
                return "Кустик боярышника - замедляет врагов";
            case UnitType.SoapSlower:
                return "Коммуналка-мыло - замедляет врагов с уроном";
            case UnitType.GasBomb:
                return "Газовый баллон - взрывается в области 3x3";
            case UnitType.Collector:
                return "Коллектор - проглатывает пенсионеров";
            case UnitType.FirePotato:
                return "Огненная картошка - поджигает врагов";
            default:
                return "Неизвестный юнит";
        }
    }
}