using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileUpgrade", menuName = "Scriptable Objects/ProjectileUpgrade")]
public class ProjectileUpgrade : WeaponUpgrade
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 15;

}
