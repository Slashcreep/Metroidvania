using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapAttack : MonoBehaviour
{
    EnemyMovement movement;
    [SerializeField] float leapDistance;
    [SerializeField] float leapHeight;
    [SerializeField] float attackCooldown;
    [SerializeField] float attackTimer;
    Vector2 leap;
    void Awake()
    {
        movement = GetComponentInParent<EnemyMovement>();
    }

    void Update()
    {
        leap = new Vector2(leapDistance * Mathf.Sign(movement.enemyMovementSpeed), leapHeight);
        if (Time.time >= attackTimer) { Debug.Log("IM READY"); }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && Time.time >= attackTimer)
        {
            Debug.Log("Player Detected");
            StartCoroutine(LeapToPlayer());
            attackTimer = Time.time + attackCooldown;
        }
    }
    IEnumerator LeapToPlayer()
    {
        movement.enemyMovementSpeed = movement.rb.velocity.x;
        yield return new WaitForSeconds(1);
        movement.rb.AddForce(leap, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        movement.enemyMovementSpeed = movement.defaultEnemyMovementSpeed;
    }
}
