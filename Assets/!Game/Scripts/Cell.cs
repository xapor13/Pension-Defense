using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2Int gridPosition;
    public bool isOccupied = false;

    private Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    private void OnMouseEnter()
    {
        if (!isOccupied)
            rend.material.color = Color.green;
    }

    private void OnMouseDown()
    {
        if (!isOccupied)
        {
            GridManager.Instance.PlaceUnitOnCell(this);
        }
    }
}
