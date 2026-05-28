using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "GrapeShotUpgrade", menuName = "Weapon Upgrades/Cannon Upgrades/GrapeShotUpgrade")]
public class GrapeShotUpgrade : CannonUpgrade
{
    [SerializeField] GameObject grapeProjectilePrefab;
    [SerializeField] int projectileCount = 5;
    [SerializeField] float spreadAngle = 45f;
    [SerializeField] float spawnJitter = 0.1f;
    [SerializeField] float speedVariance = 0.2f;
    [SerializeField] float damageMultiplier = 0.5f;
    [SerializeField] float minionHealthMultiplier = 0.3f;
    

    public override void OnAttack()
    {
        if (parentWeapon == null) return;

        Cannon cannon = parentWeapon as Cannon;
        if (cannon == null) return;

        cannon.suppressBaseShot = true;

        GameObject prefabToUse = grapeProjectilePrefab;
        MinionRoundUpgrade minionRoundUpgrade = null;
        string tagToUse = "Grape Shot";
        foreach (WeaponUpgrade upgrade in cannon.WeaponUpgrades)
        {
            if (upgrade is MinionRoundUpgrade minionRound)
            {
                prefabToUse = minionRound.MinionPrefab;
                minionRoundUpgrade = minionRound;
                tagToUse = "Grape Minion";
                break;
            }
        }

        float baseRotation = parentWeapon.transform.eulerAngles.z * math.PI / 180f + math.PI / 2f;
        Vector3 baseDirection = new Vector3(math.cos(baseRotation), math.sin(baseRotation), 0f);

        for (int i = 0; i < projectileCount; i++)
        {
            float angleOffset = UnityEngine.Random.Range(-spreadAngle / 2f, spreadAngle / 2f);
            Vector3 spreadDirection = Quaternion.Euler(0f, 0f, angleOffset) * baseDirection;
            Vector3 spawnPos = parentWeapon.transform.position + (Vector3)UnityEngine.Random.insideUnitCircle * spawnJitter;
            float randomSpeed = cannon.ProjSpeed * UnityEngine.Random.Range(1f - speedVariance, 1f + speedVariance);

            GameObject projectileObject = Instantiate(
                prefabToUse,
                spawnPos,
                parentWeapon.transform.rotation
            );
            projectileObject.tag = tagToUse;

            CannonProdj projectile = projectileObject.GetComponent<CannonProdj>();
            if (projectile != null)
            {
                projectile.ProjectileInit(spreadDirection, randomSpeed, cannon.ProjDamage * damageMultiplier, cannon);
            }
            else if (minionRoundUpgrade != null)
            {
                BallController minion = projectileObject.GetComponent<BallController>();
                minion.maxHealth = minionRoundUpgrade.MinionHealth*minionHealthMultiplier;
                minion.contactDamage = cannon.ProjDamage*damageMultiplier;
                minion.Init(new List<Upgrade>(), parentBall.playerNum,spreadDirection.z,parentBall.sprite.sprite);
                GameManager.Instance.GetMainBallByNumber(parentBall.playerNum).OnBallSpawned(minion);
            }
            else
            {
                Projectile proj = projectileObject.GetComponent<Projectile>();
                if (proj != null)
                    proj.ProjectileInit(spreadDirection, randomSpeed, cannon.ProjDamage * damageMultiplier, cannon);
            }
            projectileObject.layer = parentWeapon.gameObject.layer + 4;
        }
    }
}