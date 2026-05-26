using UnityEngine;

public class Projectile : Weapon
{
    public Vector3 direction = Vector3.zero;
    public float speed = 0;
    public Weapon parentWeapon = null;
    public float lifetime = 0;
    float lifeTimer = 0;

    public virtual void ProjectileInit(Vector3 moveDirection, float moveSpeed, float projectileDamage, Weapon weapon)
    {
        direction = moveDirection;
        speed = moveSpeed;
        damage = projectileDamage;
        parentWeapon = weapon;
        lifeTimer = lifetime;
    }

    protected override void Update()
    {
        base.Update();
        transform.position += direction * speed * Time.deltaTime;
        if (lifetime > 0)
        {
            if (lifeTimer <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                lifeTimer -= Time.deltaTime;
            }
        }
    }

    protected override void OnBallHit(BallController otherBall)
    {
        base.OnBallHit(otherBall);
        otherBall.AddVelocity(direction * speed * 0.5f);
        Destroy(gameObject);
    }

    protected override void OnWeaponHit(Weapon otherWeapon)
    {
        base.OnWeaponHit(otherWeapon);
        Destroy(gameObject);
    }

    protected override void OnWallHit()
    {
        base.OnWallHit();
        Destroy(gameObject);
    }
}