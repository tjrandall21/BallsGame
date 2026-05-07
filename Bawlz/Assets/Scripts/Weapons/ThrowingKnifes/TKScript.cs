using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class TKScript : Weapon
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float arrowSpeed = 15;
    [SerializeField] int arrowCount = 1;
    [SerializeField] int arrowDamage = 10;

    SpriteRenderer spriteRenderer;

    protected override void OnAttack()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, transform.position, transform.rotation);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        float rotation = transform.eulerAngles.z * math.PI / 180.0f + math.PI / 2;
        projectile.ProjectileInit(new Vector3(math.cos(rotation), math.sin(rotation)), arrowSpeed, arrowDamage, this);
        projectileObject.layer = gameObject.layer;
        OnAttackEnd();
    }

    public override void OnWallCollision()
    {
        base.OnWallCollision();
        attackCooldown *= 0.98f;
    }
}
