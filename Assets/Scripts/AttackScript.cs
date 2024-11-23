using System.Collections;
using System.Threading;
using TarodevController;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AttackScript : MonoBehaviour
{
    PlayerController attackMoves;
    private GameObject attackHitbox1;
    private GameObject attackHitbox2;
    private GameObject attackHitbox3;
    private GameObject attackHitbox4;
    private GameObject attackHitbox5;
    private GameObject attackHitbox6;
    private GameObject attackHitbox7;
    private GameObject attackHitbox8;
    private GameObject attackHitbox9;
    private GameObject attackHitbox10;
    private GameObject attackHitbox11;
    private GameObject attackHitbox12;
    private GameObject attackHitbox13;

    [Header("Hitbox Origin Point and Size")]
    [SerializeField] Transform attackPoint1;
    [SerializeField] Vector2 attackSize1;
    [SerializeField] Transform attackPoint2;
    [SerializeField] Vector2 attackSize2;
    [SerializeField] Transform attackPoint3;
    [SerializeField] Vector2 attackSize3;
    [SerializeField] Transform attackPoint4;
    [SerializeField] Vector2 attackSize4;
    [SerializeField] Transform attackPoint5;
    [SerializeField] Vector2 attackSize5;
    [SerializeField] Transform attackPoint6;
    [SerializeField] Vector2 attackSize6;
    [SerializeField] Transform attackPoint7;
    [SerializeField] float attackSize7;
    [SerializeField] Transform attackPoint8;
    [SerializeField] float attackSize8;
    [SerializeField] Transform attackPoint9;
    [SerializeField] float attackSize9;
    [SerializeField] Transform attackPoint10;
    [SerializeField] float attackSize10;


    public LayerMask enemyLayers;
    float attackTimer;
    [SerializeField] float attackLinger = 0.5f;
    [SerializeField] float attackCooldown;
    [SerializeField] float attackRate = 2f;
    [SerializeField] float nextAttackTime = 0f;
    [SerializeField] float damage;


    private void Start()
    {
        attackMoves = GetComponent<PlayerController>();
        attackHitbox1 = transform.GetChild(0).gameObject;
        attackHitbox2 = transform.GetChild(1).gameObject;
        attackHitbox3 = transform.GetChild(2).gameObject;
        attackHitbox4 = transform.GetChild(3).gameObject;
        attackHitbox5 = transform.GetChild(4).gameObject;
        attackHitbox6 = transform.GetChild(5).gameObject;
        attackHitbox7 = transform.GetChild(6).gameObject;
        attackHitbox8 = transform.GetChild(7).gameObject;
        attackHitbox9 = transform.GetChild(8).gameObject;
        attackHitbox10 = transform.GetChild(9).gameObject;
        attackHitbox11 = transform.GetChild(10).gameObject;
        attackHitbox12 = transform.GetChild(11).gameObject;
        attackHitbox13 = transform.GetChild(12).gameObject;
        attackHitbox1.SetActive(false);
        attackHitbox2.SetActive(false);
        attackHitbox3.SetActive(false);
        attackHitbox4.SetActive(false);
        attackHitbox5.SetActive(false);
        attackHitbox6.SetActive(false);
        attackHitbox7.SetActive(false);
        attackHitbox8.SetActive(false);
        attackHitbox9.SetActive(false);
        attackHitbox10.SetActive(false);
        attackHitbox11.SetActive(false);
        attackHitbox12.SetActive(false);
        attackHitbox13.SetActive(false);


    }

    private void Update()
    {
        RightBlock();
        UpBlock();
        LeftBlock();
        if (attackMoves._grounded)
        {
            attackHitbox7.SetActive(false);
            attackHitbox8.SetActive(false);
            attackHitbox9.SetActive(false);
            attackHitbox10.SetActive(false);
        }

        if (attackMoves.isBlocking)
        {
            attackHitbox1.SetActive(false);
            attackHitbox2.SetActive(false);
            attackHitbox3.SetActive(false);
            attackHitbox4.SetActive(false);
            attackHitbox5.SetActive(false);
            attackHitbox6.SetActive(false);
        }
        if (attackMoves.isUpwardSlashRight || attackMoves.isUpwardSlashLeft ||
            attackMoves.isSideSlashRight || attackMoves.isSideSlashLeft ||
            attackMoves.isDownwardSlashRight || attackMoves.isDownwardSlashLeft ||
            attackMoves.isAirSlashCCRight || attackMoves.isAirSlashCCLeft ||
            attackMoves.isAirSlashCRight || attackMoves.isAirSlashCLeft)
        {
            attackTimer = Time.time + attackLinger;
        }
        if (Time.time >= attackTimer)
        {
            attackHitbox1.SetActive(false);
            attackHitbox2.SetActive(false);
            attackHitbox3.SetActive(false);
            attackHitbox4.SetActive(false);
            attackHitbox5.SetActive(false);
            attackHitbox6.SetActive(false);
            attackHitbox7.SetActive(false);
            attackHitbox8.SetActive(false);
        }
        if (Time.time >= nextAttackTime)
        {
            attackMoves.isAttacking = false;
            if (attackMoves.isUpwardSlashRight)
            {
                nextAttackTime = Time.time + 1f / attackRate;
                UpwardSlashRight();
            }
            if (attackMoves.isUpwardSlashLeft)
            {
                nextAttackTime = Time.time + 1f / attackRate;
                UpwardSlashLeft();
            }
            if (attackMoves.isDownwardSlashRight)
            {
                nextAttackTime = Time.time + 1f / attackRate;
                DownwardSlashRight();
            }
            if (attackMoves.isDownwardSlashLeft)
            {
                nextAttackTime = Time.time + 1f / attackRate;
                DownwardSlashLeft();
            }
            if (attackMoves.isSideSlashRight)
            {
                nextAttackTime = Time.time + 1f / attackRate;
                SideSlashRight();
            }
            if (attackMoves.isSideSlashLeft)
            {
                nextAttackTime = Time.time + 1f / attackRate;
                SideSlashLeft();
            }
            if (attackMoves.isAirSlashCCRight)
            {
                nextAttackTime = Time.time + 1f / attackRate;
                AirSlashCCRight();
            }
            if (attackMoves.isAirSlashCCLeft)
            {
                nextAttackTime = Time.time + 1f / attackRate;
                AirSlashCCLeft();
            }
            if (attackMoves.isAirSlashCRight)
            {
                nextAttackTime = Time.time + 1f / attackRate;
                AirSlashCRight();
            }
            if (attackMoves.isAirSlashCLeft)
            {
                nextAttackTime = Time.time + 1f / attackRate;
                AirSlashCLeft();
            }
        }
    }

    #region Grounded Attacks
    private void UpwardSlashRight()
    {
        if (attackMoves.isUpwardSlashRight && !attackMoves.isAttacking)
        {
            attackMoves.isAttacking = true;
            attackHitbox1.SetActive(true);
            attackMoves.isUpwardSlashRight = false;
            Debug.Log("UP ATTACKED RIGHT");
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint1.position, attackSize1, 0f, enemyLayers);
            if (hitEnemies.Length > 0)
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
                            if (hitEnemies[i].tag == "Enemy")
                            {
                                doDamage = false;
                                continue;
                            }
                            if (hitEnemies[i].tag == "Shield" && hitEnemies[j].tag == "Enemy" && Vector2.Distance(gameObject.transform.position, hitEnemies[i].transform.position) < Vector2.Distance(gameObject.transform.position, hitEnemies[j].transform.position))
                            {
                                doDamage = false;
                                /*if (hitEnemies[i].GetComponentInParent<EnemyMovement>() != null)
                                {
                                    EnemyMovement enemy = hitEnemies[i].GetComponentInParent<EnemyMovement>();
                                    enemy.rb.AddForce(enemy.pushAmount, ForceMode2D.Impulse);
                                }*/
                                Debug.Log(hitEnemies[i].transform.parent.name + " has blocked damage");

                            }
                            else if (hitEnemies[i].tag == "Shield" && hitEnemies[j].tag == "Enemy" && Vector2.Distance(gameObject.transform.position, hitEnemies[i].transform.position) > Vector2.Distance(gameObject.transform.position, hitEnemies[j].transform.position))
                            {
                                continue;
                            }
                        }
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
    }

    private void UpwardSlashLeft()
    {
        if (attackMoves.isUpwardSlashLeft && !attackMoves.isAttacking)
        {
            attackMoves.isAttacking = true;
            attackHitbox2.SetActive(true);
            attackMoves.isUpwardSlashLeft = false;
            Debug.Log("UP ATTACKED LEFT");
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint2.position, attackSize2, 0f, enemyLayers);
            if (hitEnemies.Length > 0)
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
                            if (hitEnemies[i].tag == "Enemy")
                            {
                                doDamage = false;
                                continue;
                            }
                            if (hitEnemies[i].tag == "Shield" && hitEnemies[j].tag == "Enemy" && Vector2.Distance(gameObject.transform.position, hitEnemies[i].transform.position) < Vector2.Distance(gameObject.transform.position, hitEnemies[j].transform.position))
                            {
                                doDamage = false;
                                Debug.Log(hitEnemies[i].transform.parent.name + " has blocked damage");

                            }
                            else if (hitEnemies[i].tag == "Shield" && hitEnemies[j].tag == "Enemy" && Vector2.Distance(gameObject.transform.position, hitEnemies[i].transform.position) > Vector2.Distance(gameObject.transform.position, hitEnemies[j].transform.position))
                            {
                                continue;
                            }
                        }
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
    }

    private void DownwardSlashRight()
    {
        if (attackMoves.isDownwardSlashRight && !attackMoves.isAttacking)
        {
            attackHitbox3.SetActive(true);
            attackMoves.isAttacking = true;
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint3.position, attackSize3, 0f, enemyLayers);
            if (hitEnemies.Length > 0)
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
                            if (hitEnemies[i].tag == "Enemy")
                            {
                                doDamage = false;
                                continue;
                            }
                            if (hitEnemies[i].tag == "Shield" && hitEnemies[j].tag == "Enemy" && Vector2.Distance(gameObject.transform.position, hitEnemies[i].transform.position) < Vector2.Distance(gameObject.transform.position, hitEnemies[j].transform.position))
                            {
                                doDamage = false;
                                Debug.Log(hitEnemies[i].transform.parent.name + " has blocked damage");

                            }
                            else if (hitEnemies[i].tag == "Shield" && hitEnemies[j].tag == "Enemy" && Vector2.Distance(gameObject.transform.position, hitEnemies[i].transform.position) > Vector2.Distance(gameObject.transform.position, hitEnemies[j].transform.position))
                            {
                                continue;
                            }
                        }
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
            attackMoves.isDownwardSlashRight = false;
            Debug.Log("DOWN ATTACKED");
        }
    }
    private void DownwardSlashLeft()
    {
        if (attackMoves.isDownwardSlashLeft && !attackMoves.isAttacking)
        {
            attackHitbox4.SetActive(true);
            attackMoves.isAttacking = true;
            attackMoves.isDownwardSlashLeft = false;
            Debug.Log("DOWN ATTACKED LEFT");
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint4.position, attackSize4, 0f, enemyLayers);
            if (hitEnemies.Length > 0)
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
                            if (hitEnemies[i].tag == "Enemy")
                            {
                                doDamage = false;
                                continue;
                            }
                            if (hitEnemies[i].tag == "Shield" && hitEnemies[j].tag == "Enemy" && Vector2.Distance(gameObject.transform.position, hitEnemies[i].transform.position) < Vector2.Distance(gameObject.transform.position, hitEnemies[j].transform.position))
                            {
                                doDamage = false;
                                Debug.Log(hitEnemies[i].transform.parent.name + " has blocked damage");

                            }
                            else if (hitEnemies[i].tag == "Shield" && hitEnemies[j].tag == "Enemy" && Vector2.Distance(gameObject.transform.position, hitEnemies[i].transform.position) > Vector2.Distance(gameObject.transform.position, hitEnemies[j].transform.position))
                            {
                                continue;
                            }
                        }
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
    }

    private void SideSlashRight()
    {
        if (attackMoves.isSideSlashRight && !attackMoves.isAttacking)
        {
            attackHitbox5.SetActive(true);
            attackMoves.isAttacking = true;
            attackMoves.isSideSlashRight = false;
            Debug.Log("SIDE ATTACKED");
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint5.position, attackSize5, 0f, enemyLayers);
            if (hitEnemies.Length > 0)
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
                            if (hitEnemies[i].tag == "Enemy")
                            {
                                doDamage = false;
                                continue;
                            }
                            if (hitEnemies[i].tag == "Shield" && hitEnemies[j].tag == "Enemy" && Vector2.Distance(gameObject.transform.position, hitEnemies[i].transform.position) < Vector2.Distance(gameObject.transform.position, hitEnemies[j].transform.position))
                            {
                                doDamage = false;
                                Debug.Log(hitEnemies[i].transform.parent.name + " has blocked damage");

                            }
                            else if (hitEnemies[i].tag == "Shield" && hitEnemies[j].tag == "Enemy" && Vector2.Distance(gameObject.transform.position, hitEnemies[i].transform.position) > Vector2.Distance(gameObject.transform.position, hitEnemies[j].transform.position))
                            {
                                continue;
                            }
                        }
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
    }

    private void SideSlashLeft()
    {
        if (attackMoves.isSideSlashLeft && !attackMoves.isAttacking)
        {
            attackHitbox6.SetActive(true);
            attackMoves.isAttacking = true;
            attackMoves.isSideSlashLeft = false;
            Debug.Log("SIDE ATTACKED");
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint6.position, attackSize6, 0f, enemyLayers);
            if (hitEnemies.Length > 0)
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
                            if (hitEnemies[i].tag == "Enemy")
                            {
                                doDamage = false;
                                continue;
                            }
                            if (hitEnemies[i].tag == "Shield" && hitEnemies[j].tag == "Enemy" && Vector2.Distance(gameObject.transform.position, hitEnemies[i].transform.position) < Vector2.Distance(gameObject.transform.position, hitEnemies[j].transform.position))
                            {
                                doDamage = false;
                                Debug.Log(hitEnemies[i].transform.parent.name + " has blocked damage");

                            }
                            else if (hitEnemies[i].tag == "Shield" && hitEnemies[j].tag == "Enemy" && Vector2.Distance(gameObject.transform.position, hitEnemies[i].transform.position) > Vector2.Distance(gameObject.transform.position, hitEnemies[j].transform.position))
                            {
                                continue;
                            }
                        }
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
    }
    #endregion

    #region Air Attacks
    private void AirSlashCCRight()
    {
        if (attackMoves.isAirSlashCCRight && !attackMoves.isAttacking)
        {
            attackHitbox7.SetActive(true);
            attackMoves.isAttacking = true;
            attackMoves.isAirSlashCCRight = false;
            Debug.Log("AIR SLASH CC RIGHT");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint7.position, attackSize7, enemyLayers);
            if (hitEnemies.Length > 0)
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
                            if (hitEnemies[i].tag == "Enemy")
                            {
                                doDamage = false;
                                continue;
                            }
                            if (hitEnemies[i].tag == "Shield" && hitEnemies[j].tag == "Enemy" && Vector2.Distance(gameObject.transform.position, hitEnemies[i].transform.position) < Vector2.Distance(gameObject.transform.position, hitEnemies[j].transform.position))
                            {
                                doDamage = false;
                                Debug.Log(hitEnemies[i].transform.parent.name + " has blocked damage");

                            }
                            else if (hitEnemies[i].tag == "Shield" && hitEnemies[j].tag == "Enemy" && Vector2.Distance(gameObject.transform.position, hitEnemies[i].transform.position) > Vector2.Distance(gameObject.transform.position, hitEnemies[j].transform.position))
                            {
                                continue;
                            }
                        }
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
    }
    private void AirSlashCCLeft()
    {
        if (attackMoves.isAirSlashCCLeft && !attackMoves.isAttacking)
        {
            attackHitbox8.SetActive(true);
            attackMoves.isAttacking = true;
            attackMoves.isAirSlashCCLeft = false;
            Debug.Log("AIR SLASH CC LEFT");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint8.position, attackSize8, enemyLayers);
            if (hitEnemies.Length > 0)
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
                            if (hitEnemies[i].tag == "Enemy")
                            {
                                doDamage = false;
                                continue;
                            }
                            if (hitEnemies[i].tag == "Shield" && hitEnemies[j].tag == "Enemy" && Vector2.Distance(gameObject.transform.position, hitEnemies[i].transform.position) < Vector2.Distance(gameObject.transform.position, hitEnemies[j].transform.position))
                            {
                                doDamage = false;
                                Debug.Log(hitEnemies[i].transform.parent.name + " has blocked damage");

                            }
                            else if (hitEnemies[i].tag == "Shield" && hitEnemies[j].tag == "Enemy" && Vector2.Distance(gameObject.transform.position, hitEnemies[i].transform.position) > Vector2.Distance(gameObject.transform.position, hitEnemies[j].transform.position))
                            {
                                continue;
                            }
                        }
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
    }

    private void AirSlashCRight()
    {
        if (attackMoves.isAirSlashCRight && !attackMoves.isAttacking)
        {
            attackHitbox9.SetActive(true);
            attackMoves.isAttacking = true;
            attackMoves.isAirSlashCRight = false;
            Debug.Log("AIR SLASH C RIGHT");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint9.position, attackSize9, enemyLayers);
            if (hitEnemies.Length > 0)
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
                            if (hitEnemies[i].tag == "Enemy")
                            {
                                doDamage = false;
                                continue;
                            }
                            if (hitEnemies[i].tag == "Shield" && hitEnemies[j].tag == "Enemy" && Vector2.Distance(gameObject.transform.position, hitEnemies[i].transform.position) < Vector2.Distance(gameObject.transform.position, hitEnemies[j].transform.position))
                            {
                                doDamage = false;
                                Debug.Log(hitEnemies[i].transform.parent.name + " has blocked damage");

                            }
                            else if (hitEnemies[i].tag == "Shield" && hitEnemies[j].tag == "Enemy" && Vector2.Distance(gameObject.transform.position, hitEnemies[i].transform.position) > Vector2.Distance(gameObject.transform.position, hitEnemies[j].transform.position))
                            {
                                continue;
                            }
                        }
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
    }
    private void AirSlashCLeft()
    {
        if (attackMoves.isAirSlashCLeft && !attackMoves.isAttacking)
        {
            attackHitbox10.SetActive(true);
            attackMoves.isAttacking = true;
            attackMoves.isAirSlashCLeft = false;
            Debug.Log("AIR SLASH C LEFT");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint10.position, attackSize10, enemyLayers);
            if (hitEnemies.Length > 0)
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
                            if (hitEnemies[i].tag == "Enemy")
                            {
                                doDamage = false;
                                continue;
                            }
                            if (hitEnemies[i].tag == "Shield" && hitEnemies[j].tag == "Enemy" && Vector2.Distance(gameObject.transform.position, hitEnemies[i].transform.position) < Vector2.Distance(gameObject.transform.position, hitEnemies[j].transform.position))
                            {
                                doDamage = false;
                                Debug.Log(hitEnemies[i].transform.parent.name + " has blocked damage");

                            }
                            else if (hitEnemies[i].tag == "Shield" && hitEnemies[j].tag == "Enemy" && Vector2.Distance(gameObject.transform.position, hitEnemies[i].transform.position) > Vector2.Distance(gameObject.transform.position, hitEnemies[j].transform.position))
                            {
                                continue;
                            }
                        }
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
    }
    #endregion

    #region Blocking
    private void RightBlock()
    {
        if (!attackMoves.isLeftBlocking && !attackMoves.isUpBlocking)
        {
            attackHitbox11.SetActive(attackMoves.isRightBlocking);
        }
        if (!attackMoves.isRightBlocking)
        {
            attackHitbox11.SetActive(false);
        }
    }
    private void UpBlock()
    {
        if (!attackMoves.isLeftBlocking && !attackMoves.isRightBlocking)
        {
            attackHitbox12.SetActive(attackMoves.isUpBlocking);
        }
        if (!attackMoves.isUpBlocking)
        {
            attackHitbox12.SetActive(false);
        }
    }
    private void LeftBlock()
    {
        if (!attackMoves.isRightBlocking && !attackMoves.isUpBlocking)
        {
            attackHitbox13.SetActive(attackMoves.isLeftBlocking);
        }
        if (!attackMoves.isLeftBlocking)
        {
            attackHitbox13.SetActive(false);
        }
    }
    #endregion

    [Header("GIZMOS SETTINGS")]
    [SerializeField] Transform gizmosPos1;
    [SerializeField] Transform gizmosPos2;
    [SerializeField] Vector2 gizmoSize1;
    [SerializeField] float gizmoSize2;

    private void OnDrawGizmosSelected()
    {
        if (gizmosPos1 == null || gizmosPos2 == null)
            return;

        Gizmos.DrawWireCube(gizmosPos1.position, gizmoSize1);
        Gizmos.DrawWireSphere(gizmosPos2.position, gizmoSize2);

    }
}
