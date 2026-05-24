using UnityEngine;

[CreateAssetMenu(fileName = "GasRoundUpgrade", menuName = "Weapon Upgrades/Cannon Upgrades/GasRoundUpgrade")]
public class GasRound : CannonUpgrade
{
    [SerializeField] GameObject gasPrefab;
    [SerializeField] float gasSize = 1f;

    public override void OnProjectileDestroyed(Projectile projectile)
    {
        GameObject gasObject = Instantiate(gasPrefab, projectile.transform.position, Quaternion.identity);
        gasObject.layer = projectile.gameObject.layer;
        gasObject.transform.localScale = Vector3.one * gasSize;
        base.OnProjectileDestroyed(projectile);
    }

    public override void OnMinionDeath(Vector3 position)
    {
        GameObject gasObject = Instantiate(gasPrefab, position, Quaternion.identity);
        gasObject.layer = parentWeapon.gameObject.layer + 4;
        gasObject.transform.localScale = Vector3.one * gasSize;
        base.OnMinionDeath(position);
    }
}