using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "MinionRoundUpgrade", menuName = "Weapon Upgrades/Cannon Upgrades/MinionRoundUpgrade")]
public class MinionRoundUpgrade : CannonUpgrade
{
    [SerializeField] GameObject minionPrefab;
    [SerializeField] float minionHealth = 1;

    public float MinionHealth => minionHealth;

    public GameObject MinionPrefab => minionPrefab;

    public override void OnAttack()
    {
        if (parentWeapon == null) return;

        Cannon cannon = parentWeapon as Cannon;
        if (cannon == null) return;

        foreach (WeaponUpgrade upgrade in cannon.WeaponUpgrades)
        {
            if (upgrade is GrapeShotUpgrade) return;
        }

        cannon.suppressBaseShot = true;

        float rotation = parentWeapon.transform.eulerAngles.z * math.PI / 180f + math.PI / 2f;
        Vector3 shotDirection = new Vector3(math.cos(rotation), math.sin(rotation), 0f);

        GameObject minionObject = Instantiate(
            minionPrefab,
            parentWeapon.transform.position,
            quaternion.identity
        );
        minionObject.layer = parentWeapon.gameObject.layer + 4;
        minionObject.tag = "Cannon Minion";

        BallController minion = minionObject.GetComponent<BallController>();
        if (minion != null)
        {
            minion.maxHealth = minionHealth;
            minion.contactDamage = cannon.ProjDamage;
            minion.Init(new List<Upgrade>(),parentBall.playerNum,rotation,parentBall.sprite.sprite);
            GameManager.Instance.GetMainBallByNumber(parentBall.playerNum).OnBallSpawned(minion);
        }

        Projectile projectile = minionObject.GetComponent<Projectile>();
        if (projectile != null)
            projectile.ProjectileInit(shotDirection, cannon.ProjSpeed, cannon.ProjDamage, cannon);
    }
}