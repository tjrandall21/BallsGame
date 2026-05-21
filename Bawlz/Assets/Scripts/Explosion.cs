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
        if (!hitBalls.Contains(otherBall)) // make sure the explosion can't hit the same ball twice
        {
            base.OnBallHit(otherBall);
            hitBalls.Add(otherBall);
            Vector2 direction = (transform.position - otherBall.transform.position).normalized * -1;
            otherBall.SetVelocity(direction * knockbackSpeed);
        }
    }
}
