using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movedir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movedir * (moveSpeed * Time.deltaTime));
    }

    public void MoveTo(Vector2 targetPosition)
    {
        movedir = targetPosition;
    }
}