using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public int width = 15;
    public int height = 5;
    public float cellSize = 1f;
    public GameObject cellPrefab;
    public GameObject unitPrefab;

    private GameObject[,] gridArray;

    private void Awake()
    {
        Instance = this;
    }

    public void GenerateGrid()
    {
        ClearGrid();
        gridArray = new GameObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x * cellSize, 0, y * cellSize);
                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity, transform);
                cell.name = $"Cell_{x}_{y}";
                var cellScript = cell.AddComponent<Cell>();
                cellScript.gridPosition = new Vector2Int(x, y);
                gridArray[x, y] = cell;
            }
        }
    }

    private void ClearGrid()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    public void PlaceUnitOnCell(Cell cell)
    {
        Vector3 position = cell.transform.position + new Vector3(0, 0.5f, 0);
        Instantiate(unitPrefab, position, Quaternion.identity);
        cell.isOccupied = true;
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
        if (GUILayout.Button("Generate Grid"))
        {
            gridManager.GenerateGrid();
        }
    }
}
#endif
