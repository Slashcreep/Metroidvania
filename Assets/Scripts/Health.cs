using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHp;
    public float currentHp;
    public float timeToBeHit;
    public float immunityTime;
    private void Start()
    {
        currentHp = maxHp;
    }

    public void Damage(float amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative damage");
        }
        if (Time.time >= timeToBeHit)
        {
            timeToBeHit = Time.time + immunityTime;
            currentHp -= amount;
            Debug.Log("I'm Hit");
        }
        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (currentHp <= 0)
        {
            GameObject.Destroy(gameObject);
            Debug.Log("DEAD");
        }
    }
}
