using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
/*
public class AttackManager : MonoBehaviour
{
    private PlayerController _pc;
    [SerializeField] private GameObject _player;
    [SerializeField] Transform attackPoint1;
    [SerializeField] Vector2 attackSize;
    [SerializeField] float damage;
    public LayerMask enemyLayers;


    private void Awake()
    {
        _pc = _player.GetComponent<PlayerController>();
        
    }

    private void FixedUpdate()
    {
        if (_pc.isUpwardSlashRight)
        {
            AttackUpwardSlashRight();
            _pc.isUpwardSlashRight = false;
        }
    }

    void AttackUpwardSlashRight()
    {

        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint1.position, attackSize, 0f, enemyLayers);
        if (hitEnemies.Length > 0 )
        {

            for (int i = 0; i < hitEnemies.Length; i++)
            {
                Debug.Log(hitEnemies[i].name);
                bool doDamage = true;
                for (int j = 0; j < hitEnemies.Length; j++)
                {
                    if (hitEnemies[i].transform.parent != hitEnemies[j].transform.parent && j != i)
                    {
                        continue;
                    }
                    if (hitEnemies[i].transform.parent == hitEnemies[j].transform.parent && j != i)
                    {
                        if (hitEnemies[i].tag == "Shield" && hitEnemies[j].tag == "Enemy" && Mathf.Abs(_player.transform.position.x - hitEnemies[i].transform.position.x) < Mathf.Abs(_player.transform.position.x - hitEnemies[j].transform.position.x))
                        {
                            doDamage = false;
                        }
                        else if (hitEnemies[i].tag == "Shield" && hitEnemies[j].tag == "Enemy" && Mathf.Abs(_player.transform.position.x - hitEnemies[i].transform.position.x) > Mathf.Abs(_player.transform.position.x - hitEnemies[j].transform.position.x))
                        {
                            continue;
                        }
                    }
                }
                if (!doDamage)
                {
                    Debug.Log(hitEnemies[i].transform.parent.name + " has blocked damage");
                }
                if (doDamage)
                {
                    if (hitEnemies[i].GetComponentInParent<Health>() != null)
                    {
                        Health health = hitEnemies[i].GetComponentInParent<Health>();
                        health.Damage(damage);
                        Debug.Log(hitEnemies[i].transform.parent.name + " has been hit");
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint1 == null)
            return;

        Gizmos.DrawWireCube(attackPoint1.position, attackSize);
    }
}
*/