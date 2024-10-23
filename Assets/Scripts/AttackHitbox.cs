using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public float damage;
    [SerializeField] float parentLocation;
    [SerializeField] float enemyLocation;
    [SerializeField] float shieldLocation;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        {
            parentLocation = gameObject.transform.parent.gameObject.transform.position.x;
            if (collider.tag == "Shield")
            {
                shieldLocation = this.transform.position.x;
            }
            if (collider.tag == "Enemy")
            {
                enemyLocation = this.transform.position.x;
                if (collider.tag == "Shield" && Mathf.Abs(parentLocation - shieldLocation) < Mathf.Abs(parentLocation - enemyLocation))
                {
                    EnemyMovement _rb = collider.GetComponentInParent<EnemyMovement>();
                    _rb.rb.AddForce(_rb.pushAmount, ForceMode2D.Impulse);
                    Debug.Log("PUSH");
                    return;
                }

                if (collider.GetComponentInParent<Health>() != null)
                {
                    Health health = collider.GetComponentInParent<Health>();
                    health.Damage(damage);
                }
            }
        }
    }
}
