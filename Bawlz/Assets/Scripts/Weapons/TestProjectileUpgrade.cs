using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ProjectileUpgrade : WeaponUpgrade
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 15;

}
