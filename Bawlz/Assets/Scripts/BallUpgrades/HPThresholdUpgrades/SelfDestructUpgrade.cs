using UnityEngine;

[CreateAssetMenu(fileName = "SelfDestructUpgrade", menuName = "Ball Upgrades/SelfDestructUpgrade")]
public class SelfDestructUpgrade : Upgrade
{
    [SerializeField] GameObject explosionPrefab;
    public float explosionDamage = 200;
    public float knockback = 10;
    public float explosionSize = 10;

    public override void OnBallDeath()
    {
        base.OnBallDeath();
        GameObject explosionObject = Instantiate(explosionPrefab, parentBall.transform.position, Quaternion.identity);
        explosionObject.layer = parentBall.gameObject.layer; 
        Explosion explosion = explosionObject.GetComponent<Explosion>();
        explosion.ExplosionInit(explosionDamage, parentBall.transform.position, knockback, explosionSize);
    }
}
