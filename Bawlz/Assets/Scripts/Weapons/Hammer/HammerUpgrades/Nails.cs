using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

[CreateAssetMenu(fileName = "Nails", menuName = "Weapon Upgrades/Hammer Upgrades/Nails")]
public class HammerNails : HammerUpgrade
{
    [SerializeField] List<GameObject> nailPrefabs;
    [SerializeField] float nailDamage = 1;
    [SerializeField] float nailLifeTime = 1;
    [SerializeField] float projectileCount = 3;
    [SerializeField] float nailSpeed = 8;
    [SerializeField] float nailSize = 0.4f;

    public override void OnBallHit(BallController otherBall)
    {
        for (int i = 0; i < projectileCount; i++)
        {
            float rotation = UnityEngine.Random.Range(-math.PI,math.PI);
            quaternion nailRotation = quaternion.Euler(0,0,rotation-math.PI/2);
            GameObject projectileObject = Instantiate(nailPrefabs[UnityEngine.Random.Range(0,nailPrefabs.Count)], parentWeapon.transform.position, nailRotation);
            projectileObject.transform.localScale = new Vector2(nailSize,nailSize);
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.lifetime = nailLifeTime;
            projectile.ProjectileInit(new Vector3(math.cos(rotation), math.sin(rotation)), nailSpeed, nailDamage, parentWeapon);
            projectileObject.layer = parentWeapon.gameObject.layer < 10 ? parentWeapon.gameObject.layer+4 : parentWeapon.gameObject.layer;
        }
        base.OnBallHit(otherBall);
    }

}