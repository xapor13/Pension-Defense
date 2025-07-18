using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public Vector3 targetPosition;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
    }
}
