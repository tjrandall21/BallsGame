using Unity.Mathematics;
using UnityEngine;

public class TKProdj : Projectile
{

    SpriteRenderer spriteRenderer;



    public virtual void ProjectileInit(Vector3 moveDirection, float moveSpeed, float projectileDamage, Weapon weapon)
    {
        direction = moveDirection;
        speed = moveSpeed;
        damage = projectileDamage;
        parentWeapon = weapon;
    }

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
        // spin go here




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
