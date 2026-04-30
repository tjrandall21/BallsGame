using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileUpgrade", menuName = "Scriptable Objects/ProjectileUpgrade")]
public class ProjectileUpgrade : WeaponUpgrade
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 15;


    public override void OnAttack()
    {
        Debug.Log("bow attack");
        GameObject projectileObject = Instantiate(projectilePrefab, parentWeapon.transform.position, parentWeapon.transform.rotation);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        float rotation = parentWeapon.transform.eulerAngles.z * math.PI/180.0f + math.PI/2;
        Debug.Log(rotation);
        projectile.direction = new Vector3(math.cos(rotation),math.sin(rotation));
        Debug.Log(projectile.direction);
        projectile.speed = projectileSpeed;
        projectileObject.layer = parentWeapon.gameObject.layer;
        parentWeapon.OnAttackEnd();
    }
}
