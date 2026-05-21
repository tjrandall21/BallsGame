using UnityEngine;

[CreateAssetMenu(fileName = "ExplosiveminionsUpgrade", menuName = "Weapon Upgrades/Sceptre Upgrades/ExplosiveMinionsUpgrade")]
public class ExplosiveMinions : SceptreUpgrade
{
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] float explosionDamage = 10;
    [SerializeField] float explosionKnockback = 30;
    [SerializeField] float explosionSize = 1;

    public override void OnMinionDeath(BallController minion)
    {
        base.OnMinionDeath(minion);
        GameObject explosionObject = Instantiate(explosionPrefab);
        explosionObject.layer = minion.gameObject.layer;
        Explosion explosion = explosionObject.GetComponent<Explosion>();
        explosion.ExplosionInit(explosionDamage,minion.transform.position,explosionKnockback,explosionSize);
    }
}
