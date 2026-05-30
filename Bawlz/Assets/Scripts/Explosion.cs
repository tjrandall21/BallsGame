using System.Collections.Generic;
using UnityEngine;

public class Explosion : Weapon
{
    Animator animator = null;
    List<BallController> hitBalls = new List<BallController>();
    CircleCollider2D hitbox = null;

    protected override void Start()
    {
        base.Start();
        hitbox = GetComponent<CircleCollider2D>();
        DisableHitbox();
    }

    public virtual void ExplosionInit(float explosionDamage, Vector2 position, float knockback, float size=1)
    {
        transform.position = position;
        transform.localScale = new Vector3(size,size,1);
        damage = explosionDamage;
        knockbackSpeed = knockback;
        
    }
    
    public void EnableHitbox()
    {
        hitbox.enabled = true;
    }

    public void DisableHitbox()
    {
        hitbox.enabled = false;
    }

    public void EndExplosion()
    {
        Destroy(gameObject);
    }

    protected override void OnBallHit(BallController otherBall) 
    {
        if (!hitBalls.Contains(otherBall))
        {
            base.OnBallHit(otherBall);
            hitBalls.Add(otherBall);

            Vector2 direction = (otherBall.transform.position - transform.position).normalized;


            if (direction == Vector2.zero)
            {
                direction = Vector2.up; // <-- bug fix for balls getting "frozen" when blown up
            }
                

            otherBall.AddVelocity(direction * knockbackSpeed);
        }
    }
}
