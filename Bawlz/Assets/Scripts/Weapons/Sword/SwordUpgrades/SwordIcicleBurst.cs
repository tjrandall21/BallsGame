using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "IceSword", menuName = "Weapon Upgrades/Sword Upgrades/Ice Sword")]
public class SwordIcicleBurst : SwordUpgrade
{
    [SerializeField] List<GameObject> iciclePrefabs;
    [SerializeField] float icicleDamage = 4;
    [SerializeField] float icicleLifeTime = 1;
    [SerializeField] float projectileCount = 3;
    [SerializeField] float icicleSpeed = 8;
    [SerializeField] float icicleSize = 0.4f;

    public override void OnBallHit(BallController otherBall)
    {
        for (int i = 0; i < projectileCount; i++)
        {
            float rotation = UnityEngine.Random.Range(-math.PI,math.PI);
            quaternion icicleRotation = quaternion.Euler(0,0,rotation-math.PI/2);
            GameObject projectileObject = Instantiate(iciclePrefabs[UnityEngine.Random.Range(0,iciclePrefabs.Count)], parentWeapon.transform.position, icicleRotation);
            projectileObject.transform.localScale = new Vector2(icicleSize,icicleSize);
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.lifetime = icicleLifeTime;
            projectile.ProjectileInit(new Vector3(math.cos(rotation), math.sin(rotation)), icicleSpeed, icicleDamage, parentWeapon);
            projectileObject.layer = parentWeapon.gameObject.layer < 10 ? parentWeapon.gameObject.layer+4 : parentWeapon.gameObject.layer;
        }
        base.OnBallHit(otherBall);
    }

}
