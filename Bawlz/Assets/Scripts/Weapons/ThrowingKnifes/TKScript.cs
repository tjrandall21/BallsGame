using Unity.Mathematics;
using UnityEngine;

public class TKScript : Weapon
{
    [SerializeField, Tooltip("The throwing knife projectile prefab")] GameObject projectilePrefab;
    [SerializeField, Tooltip("Speed the knife is thrown at")] float knifeSpeed = 15;
    [SerializeField, Tooltip("Damage the knife deals on hit")] int knifeDamage = 10;

    protected override void OnAttack()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, transform.position, transform.rotation);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        float rotation = transform.eulerAngles.z * math.PI / 180.0f + math.PI / 2;
        projectile.ProjectileInit(new Vector3(math.cos(rotation), math.sin(rotation)), knifeSpeed, knifeDamage, this);
        projectileObject.layer = gameObject.layer;
        OnAttackEnd();
    }

    public override void OnWallCollision()
    {
        base.OnWallCollision();
    }

    protected override void OnWeaponHit(Weapon otherWeapon)
    {
        FXManager.Instance.PlayWeaponHit(otherWeapon.transform.position);
        base.OnWeaponHit(otherWeapon);
        attackCooldown *= 0.98f;
    }

    protected override void OnBallHit(BallController otherBall)
    {
        FXManager.Instance.PlayPlayerHit(otherBall.transform.position);
        base.OnBallHit(otherBall);
    }
}