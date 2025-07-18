using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [Header("Grid Settings")]
    public int width = 15;
    public int height = 5;
    public float cellSize = 1f;
    
    [Header("Prefabs")]
    public GameObject cellPrefab;
    public GameObject[] unitPrefabs; // Префабы юнитов в том же порядке, что и enum UnitType

    private GameObject[,] gridArray;
    private Cell[,] cellScripts;

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
        if (gridArray == null)
        {
            GenerateGrid();
        }
    }

    public void GenerateGrid()
    {
        ClearGrid();
        gridArray = new GameObject[width, height];
        cellScripts = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x * cellSize, 0, y * cellSize);
                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity, transform);
                cell.name = $"Cell_{x}_{y}";
                
                Cell cellScript = cell.GetComponent<Cell>();
                if (cellScript == null)
                {
                    cellScript = cell.AddComponent<Cell>();
                }
                
                cellScript.gridPosition = new Vector2Int(x, y);
                gridArray[x, y] = cell;
                cellScripts[x, y] = cellScript;
            }
        }
    }

    private void ClearGrid()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        
        gridArray = null;
        cellScripts = null;
    }

    public void PlaceUnitOnCell(Cell cell)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        
        UnitType selectedUnit = GameManager.Instance.selectedUnitType;
        int cost = ResourceManager.Instance.GetUnitCost(selectedUnit);
        
        // Проверяем, хватает ли ресурсов
        if (!ResourceManager.Instance.CanAfford(selectedUnit))
        {
            return;
        }
        
        // Тратим ресурсы
        if (!ResourceManager.Instance.SpendGoldenTeeth(cost))
        {
            return;
        }
        
        // Размещаем юнит
        GameObject unitPrefab = GetUnitPrefab(selectedUnit);
        if (unitPrefab != null)
        {
            Vector3 position = cell.transform.position + new Vector3(0, 0.5f, 0);
            GameObject unit = Instantiate(unitPrefab, position, Quaternion.identity);
            
            // Устанавливаем тип юнита
            Unit unitScript = unit.GetComponent<Unit>();
            if (unitScript != null)
            {
                unitScript.unitType = selectedUnit;
            }
            
            cell.isOccupied = true;
            cell.GetComponent<Renderer>().material.color = Color.red; // Показываем, что клетка занята
        }
    }
    
    private GameObject GetUnitPrefab(UnitType unitType)
    {
        int index = (int)unitType;
        if (index >= 0 && index < unitPrefabs.Length && unitPrefabs[index] != null)
        {
            return unitPrefabs[index];
        }
        
        // Если префаб не найден, создаем временный объект
        Debug.LogWarning($"Префаб для {unitType} не найден! Создаем временный объект.");
        return CreateTemporaryUnitPrefab(unitType);
    }
    
    private GameObject CreateTemporaryUnitPrefab(UnitType unitType)
    {
        GameObject tempUnit = GameObject.CreatePrimitive(PrimitiveType.Cube);
        tempUnit.transform.localScale = Vector3.one * 0.8f;
        
        // Добавляем соответствующий скрипт юнита
        switch (unitType)
        {
            case UnitType.PotatoShooter:
                tempUnit.AddComponent<PotatoShooter>();
                tempUnit.GetComponent<Renderer>().material.color = Color.green;
                break;
            case UnitType.DentistSunflower:
                tempUnit.AddComponent<DentistSunflower>();
                tempUnit.GetComponent<Renderer>().material.color = Color.yellow;
                break;
            case UnitType.HawthornBush:
                tempUnit.AddComponent<HawthornBush>();
                tempUnit.GetComponent<Renderer>().material.color = Color.magenta;
                break;
            case UnitType.SoapSlower:
                tempUnit.AddComponent<Unit>(); // Базовый класс для простоты
                tempUnit.GetComponent<Renderer>().material.color = Color.cyan;
                break;
            case UnitType.GasBomb:
                tempUnit.AddComponent<GasBomb>();
                tempUnit.GetComponent<Renderer>().material.color = Color.gray;
                break;
            case UnitType.Collector:
                tempUnit.AddComponent<Collector>();
                tempUnit.GetComponent<Renderer>().material.color = Color.black;
                break;
            case UnitType.FirePotato:
                tempUnit.AddComponent<FirePotato>();
                tempUnit.GetComponent<Renderer>().material.color = Color.red;
                break;
        }
        
        return tempUnit;
    }
    
    public Cell GetCell(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return cellScripts[x, y];
        }
        return null;
    }
    
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * cellSize, 0, y * cellSize);
    }
    
    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x / cellSize);
        int y = Mathf.RoundToInt(worldPosition.z / cellSize);
        return new Vector2Int(x, y);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridManager gridManager = (GridManager)target;
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("Generate Grid"))
        {
            gridManager.GenerateGrid();
        }
        
        if (GUILayout.Button("Clear Grid"))
        {
            for (int i = gridManager.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(gridManager.transform.GetChild(i).gameObject);
            }
        }
    }
}
#endif