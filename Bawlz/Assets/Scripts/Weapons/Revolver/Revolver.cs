using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Revolver : Weapon
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float bulletSpeed = 15f;
    [SerializeField] public float bulletDamage = 10f;
    [SerializeField] int maxAmmo = 6;
    [SerializeField] float reloadTime = 2f;
    [SerializeField] int extraProjectiles = 0;
    [SerializeField] float spreadIncreasePerBullet = 10f;

    int currentAmmo;
    bool isReloading = false;

    protected override void Start()
    {
        base.Start();
        currentAmmo = maxAmmo;
    }

    protected override void OnAttack()
    {
        if (isReloading) return;
        if (currentAmmo <= 0)
        {
            StartCoroutine(ReloadCoroutine());
            return;
        }

        float halfSpreadRadians = extraProjectiles * spreadIncreasePerBullet * (math.PI / 180.0f) * 0.5f;

        for (int i = 0; i < 1 + extraProjectiles; i++)
        {
            float rotation = transform.eulerAngles.z * math.PI / 180.0f + math.PI * 0.5f;
            if (extraProjectiles > 0)
            {
                float spreadRatio = (i / (float)extraProjectiles - 0.5f) * 2f;
                rotation += spreadRatio * halfSpreadRadians;
            }

            quaternion projRot = quaternion.Euler(0, 0, rotation - math.PI / 2);
            GameObject projectileObject = Instantiate(projectilePrefab, transform.position, projRot);
            Projectile proj = projectileObject.GetComponent<Projectile>();

            if (proj != null)
            {
                proj.ProjectileInit(new Vector3(math.cos(rotation), math.sin(rotation)), bulletSpeed, bulletDamage, this);
            }
               
            projectileObject.layer = gameObject.layer < 10 ? gameObject.layer + 4 : gameObject.layer;

            FXManager.Instance.PlayWeaponHit(transform.position, this);
        }

        currentAmmo--;
        if (currentAmmo <= 0)
            StartCoroutine(ReloadCoroutine());

        OnAttackEnd();
    }

    IEnumerator ReloadCoroutine()
    {
        if (isReloading)
        {
            yield break;
        } 
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        attackTimer = attackCooldown;
    }

    public int GetCurrentAmmo() => currentAmmo;
    public bool IsReloading() => isReloading;

    public override void OnWallCollision()
    {
        base.OnWallCollision();
        attackCooldown *= 0.98f;
    }
}