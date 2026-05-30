using UnityEngine;

public class HammerProdj : Projectile
{
    [SerializeField] GameObject persistentProjectilePrefab;
    [SerializeField] float persistentLifetime = 3f;
    [SerializeField] float spawnSpeedMultiplier = 1f;
    [SerializeField] float spawnDamageMultiplier = 1f;
    [SerializeField] float spawnOffset = 0.25f; // small offset so spawned projectile doesn't immediately overlap the hit ball

    protected override void OnBallHit(BallController otherBall)
    {
        Vector3 dirNorm = direction.normalized;
        otherBall.AddVelocity((Vector2)dirNorm * speed * 0.5f);

        if (persistentProjectilePrefab != null)
        {
            Vector3 spawnPos = transform.position + dirNorm * spawnOffset;
            GameObject proj = Instantiate(persistentProjectilePrefab, spawnPos, Quaternion.identity);
            proj.layer = gameObject.layer;

            PersistentProjectile p = proj.GetComponent<PersistentProjectile>();
            if (p != null)
            {
                p.lifetime = persistentLifetime;
                p.ProjectileInit(dirNorm, speed * spawnSpeedMultiplier, damage * spawnDamageMultiplier, parentWeapon);
            }
            else
            {
                Projectile generic = proj.GetComponent<Projectile>();
                if (generic != null)
                {
                    generic.lifetime = persistentLifetime;
                    generic.ProjectileInit(dirNorm, speed * spawnSpeedMultiplier, damage * spawnDamageMultiplier, parentWeapon);
                }
            }
        }

        Destroy(gameObject);
    }

    protected override void OnWeaponHit(Weapon otherWeapon)
    {
        base.OnWeaponHit(otherWeapon);
    }

    protected override void OnWallHit()
    {
        base.OnWallHit();
    }
}