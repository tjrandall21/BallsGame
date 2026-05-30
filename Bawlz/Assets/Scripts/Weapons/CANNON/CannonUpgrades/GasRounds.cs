using UnityEngine;

[CreateAssetMenu(fileName = "GasRoundUpgrade", menuName = "Weapon Upgrades/Cannon Upgrades/GasRoundUpgrade")]
public class GasRound : CannonUpgrade
{
    [SerializeField] GameObject gasPrefab;
    [SerializeField] float gasSize = 1f;
    [SerializeField] float gasDamage = 3f;
    [SerializeField] float gasKnockback = 10f;
    [SerializeField] float grapeShotGasSize = 0.5f;

    public override void OnProjectileDestroyed(Projectile projectile)
    {
        GameObject gasObject = Instantiate(gasPrefab);
        gasObject.layer = projectile.gameObject.layer;
        gasObject.GetComponent<Explosion>().ExplosionInit(gasDamage,projectile.transform.position,gasKnockback,gasSize);
        base.OnProjectileDestroyed(projectile);
    }

    public override void OnMinionDeath(BallController minion)
    {
        if (minion.tag == "Cannon Minion")
        {
            GameObject gasObject = Instantiate(gasPrefab);
            gasObject.layer = parentWeapon.gameObject.layer + 4;
            gasObject.GetComponent<Explosion>().ExplosionInit(gasDamage,minion.transform.position,gasKnockback,gasSize);
        }
        else if (minion.tag == "Grape Minion")
        {
            GameObject gasObject = Instantiate(gasPrefab);
            gasObject.layer = parentWeapon.gameObject.layer + 4;
            gasObject.GetComponent<Explosion>().ExplosionInit(gasDamage,minion.transform.position,gasKnockback,grapeShotGasSize);
        }
        base.OnMinionDeath(minion);
    }
}