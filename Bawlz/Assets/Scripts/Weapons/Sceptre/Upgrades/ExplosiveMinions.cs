using UnityEngine;

[CreateAssetMenu(fileName = "ExplosiveMinionsUpgrade", menuName = "Ball Upgrades/Minion Upgrades/ExplosiveMinionsUpgrade")]
public class ExplosiveMinions : Upgrade
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
        float size = minion.tag == "Super Minion" ? explosionSize * 1.5f : explosionSize;
        float damage = minion.tag == "Super Minion" ? explosionDamage * 2 : explosionDamage;
        explosion.ExplosionInit(damage,minion.transform.position,explosionKnockback,size);
    }
}
