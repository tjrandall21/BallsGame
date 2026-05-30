using UnityEngine;

public class PersistentProjectile : Projectile
{
    [SerializeField] float defaultLifetime = 3f; 

    protected override void OnBallHit(BallController otherBall)
    {
        otherBall.AddVelocity((Vector2)direction.normalized * speed * 0.5f);
    }

    protected override void OnWeaponHit(Weapon otherWeapon)
    {
    }

    public override void ProjectileInit(Vector3 moveDirection, float moveSpeed, float projectileDamage, Weapon weapon)
    {
        if (lifetime <= 0f && defaultLifetime > 0f)
        {
            lifetime = defaultLifetime;
        }

        base.ProjectileInit(moveDirection, moveSpeed, projectileDamage, weapon);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        BallController ball = collision.collider.GetComponent<BallController>();
        if (ball != null)
        {
            OnBallHit(ball);
            return;
        }

        Weapon otherWeapon = collision.collider.GetComponent<Weapon>();
        if (otherWeapon != null)
        {
            OnWeaponHit(otherWeapon);
            return;
        }

        if (collision.contactCount > 0)
        {
            ContactPoint2D contact = collision.GetContact(0);
            Vector3 reflected = Vector3.Reflect(direction.normalized, contact.normal);
            direction = reflected.normalized;
            transform.position += (Vector3)direction * 0.01f;
        }
    }

    protected override void OnWallHit()
    {
    }
}