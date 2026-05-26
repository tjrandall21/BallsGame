using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "IceBurstUpgrade", menuName = "Ball Upgrades/IceBurstUpgrade")]
public class IceBurstUpgrade : Upgrade
{
    [SerializeField] List<GameObject> iciclePrefabs;
    [SerializeField] float icicleDamage = 4;
    [SerializeField] float icicleLifeTime = 1;
    [SerializeField] float projectileCount = 3;
    [SerializeField] float icicleSpeed = 8;
    [SerializeField] float icicleSize = 0.4f;

    public override void OnWeaponCollision(Weapon otherWeapon)
    {
        for (int i = 0; i < projectileCount; i++)
        {
            float rotation = UnityEngine.Random.Range(-math.PI,math.PI);
            quaternion icicleRotation = quaternion.Euler(0,0,rotation-math.PI/2);
            GameObject projectileObject = Instantiate(iciclePrefabs[UnityEngine.Random.Range(0,iciclePrefabs.Count)], parentBall.transform.position, icicleRotation);
            projectileObject.transform.localScale = new Vector2(icicleSize,icicleSize);
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.lifetime = icicleLifeTime;
            projectile.ProjectileInit(new Vector3(math.cos(rotation), math.sin(rotation)), icicleSpeed, icicleDamage, null);    
            projectileObject.layer = parentBall.gameObject.layer+4;
        }
        base.OnWeaponCollision(otherWeapon);
    }
}
