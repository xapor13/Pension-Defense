using UnityEngine;

public class Cell : MonoBehaviour
{
    [Header("Cell Settings")]
    public Vector2Int gridPosition;
    public bool isOccupied = false;
    
    [Header("Visual")]
    public Color normalColor = Color.white;
    public Color hoverColor = Color.green;
    public Color occupiedColor = Color.red;
    public Color cannotAffordColor = Color.gray;

    private Renderer rend;
    private Color originalColor;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            originalColor = rend.material.color;
            normalColor = originalColor;
        }
    }

    private void OnMouseEnter()
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        
        if (!isOccupied)
        {
            UnitType selectedUnit = GameManager.Instance.selectedUnitType;
            
            // Проверяем, хватает ли ресурсов
            if (ResourceManager.Instance.CanAfford(selectedUnit))
            {
                rend.material.color = hoverColor;
            }
            else
            {
                rend.material.color = cannotAffordColor;
            }
        }
    }

    private void OnMouseExit()
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        
        if (!isOccupied)
        {
            rend.material.color = normalColor;
        }
    }

    private void OnMouseDown()
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        
        if (!isOccupied)
        {
            GridManager.Instance.PlaceUnitOnCell(this);
        }
    }
    
    public void SetOccupied(bool occupied)
    {
        isOccupied = occupied;
        
        if (rend != null)
        {
            rend.material.color = occupied ? occupiedColor : normalColor;
        }
    }
    
    public void SetColor(Color color)
    {
        if (rend != null)
        {
            rend.material.color = color;
        }
    }
    
    public void ResetColor()
    {
        if (rend != null)
        {
            rend.material.color = isOccupied ? occupiedColor : normalColor;
        }
    }
}