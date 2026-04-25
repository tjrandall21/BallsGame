using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] float damage = 10;
    public float Damage {get{return damage;}}
    [SerializeField] float knockbackSpeed = 0;
    [SerializeField] float attackCooldown = 0;
    float attackTimer = 0;
    Rigidbody2D rb = null;
    BallController parent = null;
    
    List<uint> activeCollisions = new List<uint>();

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        parent = GetComponentInParent<BallController>();
    }

    void Update()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                OnAttack();
            }
        }

    }

    uint GetID()
    {
        if (parent == null)
        {
            return 0;
        }
        return parent.BallID;
    }

    protected virtual void OnBallHit(BallController otherBall)
    {
        Debug.Log("Ball Collision");
        parent.FlipRotation();
        otherBall.OnHit(this);
    }
    protected virtual void OnWeaponHit(Weapon otherWeapon)
    {
        Debug.Log("Weapon Collision");
        parent.FlipRotation();
    }

    protected virtual void OnAttack()
    {
        
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        BallController ball = collision.collider.GetComponent<BallController>();
        if (ball != null)
        {
            uint newID = ball.BallID;
            foreach (uint id in activeCollisions)
            {
                if (newID == id)
                {
                    return;
                }
            }
            OnBallHit(ball);
            return;
        }
        Weapon otherWeapon = collision.collider.GetComponent<Weapon>();
        if (otherWeapon != null)
        {
            uint newID = otherWeapon.GetID();
            foreach (uint id in activeCollisions)
            {
                if (newID == id)
                {
                    return;
                }
            }
            OnWeaponHit(otherWeapon);
            return;
        }
    }

}
