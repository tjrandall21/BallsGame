using UnityEngine;

[CreateAssetMenu(fileName = "GasRoundUpgrade", menuName = "Weapon Upgrades/GasRoundUpgrade")]
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
}