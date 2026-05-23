using UnityEngine;

[CreateAssetMenu(fileName = "ExplosiveArrowUpgrade", menuName = "Weapon Upgrades/Bow Upgrades/ExplosiveArrowUpgrade")]
public class ExplosiveArrowUpgrade : BowUpgrade
{
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] float explosionDamage = 10;
    [SerializeField] float explosionKnockback = 30;
    [SerializeField] float explosionSize = 1;

    public override void OnArrowDestroyed(BowProjectile arrow)
    {
        GameObject explosionObject = Instantiate(explosionPrefab);
        explosionObject.layer = arrow.gameObject.layer+4;
        Explosion explosion = explosionObject.GetComponent<Explosion>();
        explosion.ExplosionInit(explosionDamage,arrow.transform.position,explosionKnockback,explosionSize);
        base.OnArrowDestroyed(arrow);
    }
}
