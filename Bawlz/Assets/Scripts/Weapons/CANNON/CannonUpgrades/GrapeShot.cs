using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "GrapeShotUpgrade", menuName = "Weapon Upgrades/GrapeShotUpgrade")]
public class GrapeShot : CannonUpgrade
{
    [SerializeField, Tooltip("Number of projectiles in the spread")]
    int projectileCount = 5;

    [SerializeField, Tooltip("Total angle of the spread in degrees")]
    float spreadAngle = 45f;

    public override void OnAttack()
    {
        if (parentWeapon == null) return;

        Cannon cannon = parentWeapon as Cannon;
        if (cannon == null) return;
        float baseRotation = parentWeapon.transform.eulerAngles.z * math.PI / 180f + math.PI / 2f;
        Vector3 baseDirection = new Vector3(math.cos(baseRotation), math.sin(baseRotation), 0f);

        for (int i = 0; i < projectileCount; i++)
        {
            float angleOffset = UnityEngine.Random.Range(-spreadAngle / 2f, spreadAngle / 2f);
            Vector3 spreadDirection = Quaternion.Euler(0f, 0f, angleOffset) * baseDirection;

            GameObject projectileObject = Instantiate(
                cannon.ProjectilePrefab,
                parentWeapon.transform.position,
                parentWeapon.transform.rotation
            );

            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.ProjectileInit(spreadDirection, cannon.ProjSpeed, cannon.ProjDamage, parentWeapon);
            projectileObject.layer = parentWeapon.gameObject.layer + 4;
        }
    }
}
