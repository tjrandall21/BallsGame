using UnityEngine;

public class Projectile : Weapon
{
    public Vector3 direction = Vector3.zero;
    public float speed = 0;
    public Weapon parentWeapon = null;

    public virtual void ProjectileInit(Vector3 moveDirection, float moveSpeed, float projectileDamage, Weapon weapon)
    {
        direction = moveDirection;
        speed = moveSpeed;
        damage = projectileDamage;
        parentWeapon = weapon;
    }

    protected override void Update()
    {
        base.Update();
        transform.position += direction * speed * Time.deltaTime;
    }

    protected override void OnBallHit(BallController otherBall)
    {
        base.OnBallHit(otherBall);
        otherBall.AddVelocity(direction * speed);
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