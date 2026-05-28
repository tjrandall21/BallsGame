using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "MinionRoundUpgrade", menuName = "Weapon Upgrades/Cannon Upgrades/MinionRoundUpgrade")]
public class MinionRoundUpgrade : CannonUpgrade
{
    [SerializeField] GameObject minionPrefab;
    public GameObject MinionPrefab => minionPrefab;

    private class TrackedMinion
    {
        public BallController ball;
        public Vector3 lastPosition;
    }

    private List<TrackedMinion> _activeMinions = new List<TrackedMinion>();

    public void TrackMinion(BallController minion, Vector3 position)
    {
        _activeMinions.Add(new TrackedMinion
        {
            ball = minion,
            lastPosition = position
        });
    }

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
            parentWeapon.transform.rotation
        );

        minionObject.layer = parentWeapon.gameObject.layer + 4;

        BallController minion = minionObject.GetComponent<BallController>();
        if (minion != null)
            TrackMinion(minion, minionObject.transform.position);

        Projectile projectile = minionObject.GetComponent<Projectile>();
        if (projectile != null)
            projectile.ProjectileInit(shotDirection, cannon.ProjSpeed, cannon.ProjDamage, cannon);
    }

    public override void Update()
    {
        Cannon cannon = parentWeapon as Cannon;
        if (cannon == null) return;

        for (int i = _activeMinions.Count - 1; i >= 0; i--)
        {
            TrackedMinion tracked = _activeMinions[i];

            if (tracked.ball == null)
            {
                cannon.OnMinionDeath(tracked.lastPosition);
                _activeMinions.RemoveAt(i);
            }
            else
            {
                tracked.lastPosition = tracked.ball.transform.position;
            }
        }
    }
}