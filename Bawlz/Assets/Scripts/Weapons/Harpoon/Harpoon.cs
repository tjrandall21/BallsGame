using UnityEngine;
using Unity.Mathematics;

public class Harpoon : Weapon
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 30;
    [SerializeField] int projectileDamage = 4;

    protected override void OnAttack()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, transform.position, transform.rotation);
        HarpoonProjectile projectile = projectileObject.GetComponent<HarpoonProjectile>();
        float rotation = transform.eulerAngles.z * math.PI / 180.0f + math.PI / 2;
        projectile.ProjectileInit(new Vector3(math.cos(rotation), math.sin(rotation)), projectileSpeed, projectileDamage, this);
        projectileObject.layer = gameObject.layer;
        OnAttackEnd();
    }

    
}
