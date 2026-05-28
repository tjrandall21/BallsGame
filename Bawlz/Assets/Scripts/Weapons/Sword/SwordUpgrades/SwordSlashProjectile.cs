using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "SwordSlashProjectile", menuName = "Weapon Upgrades/Sword Upgrades/SlashProjectile")]
public class SwordSlashProjectile : SwordUpgrade
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileDamage = 4;
    [SerializeField] float projectileLifeTime = 1;
    [SerializeField] float projectileSpeed = 8;
    [SerializeField] float projectileSize = 0.4f;
    [SerializeField] float slashCooldown = 1.7f;
    float slashTimer = 0;

    public override void Init(BallController ball, Weapon weapon)
    {
        base.Init(ball, weapon);
        slashTimer = slashCooldown;
    }

    public override void Update()
    {
        base.Update();
        slashTimer -= Time.deltaTime;
        if (slashTimer <= 0)
        {
            SpawnProjectile();
            slashTimer = slashCooldown;
        }
    }

    void SpawnProjectile()
    {
        float rotation = parentWeapon.transform.eulerAngles.z * math.PI / 180.0f + math.PI * 0.5f;
        quaternion slashRotation = quaternion.Euler(0,0,rotation-math.PI/2);
        GameObject projectileObject = Instantiate(projectilePrefab, parentWeapon.transform.position, slashRotation);
        projectileObject.transform.localScale = new Vector2(projectileSize,projectileSize);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.lifetime = projectileLifeTime;
        projectile.ProjectileInit(new Vector3(math.cos(rotation), math.sin(rotation)), projectileSpeed, projectileDamage, parentWeapon);
        projectileObject.layer = parentWeapon.gameObject.layer < 10 ? parentWeapon.gameObject.layer+4 : parentWeapon.gameObject.layer;
    }

}
