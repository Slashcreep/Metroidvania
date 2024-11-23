using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float enemyMovementSpeed;
    public float defaultEnemyMovementSpeed;
    bool isFacingRight;
    public Rigidbody2D rb;
    public Vector2 pushAmount;
    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyMovementSpeed = defaultEnemyMovementSpeed;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        rb.AddForce(Vector2.right * enemyMovementSpeed * Time.deltaTime);
        FlipEnemySprite();
        animator.SetFloat("xVelocity", Mathf.Abs(enemyMovementSpeed));
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Wall")
        {
            enemyMovementSpeed = -enemyMovementSpeed;
        }
    }

    void FlipEnemySprite()
    {
        transform.localScale = new Vector2(Mathf.Sign(enemyMovementSpeed), 1f);
    }
}
