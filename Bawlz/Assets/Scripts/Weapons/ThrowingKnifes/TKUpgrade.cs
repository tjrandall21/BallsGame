using UnityEngine;

[CreateAssetMenu(fileName = "ThrowingKnifeUpgrade", menuName = "Weapon Upgrades/TK Upgrades/TKUpgrade")]
public class TKUpgrade : WeaponUpgrade
{
    [SerializeField] public float TKSpeed = 0;
    [SerializeField] public float TKDamage = 0;
    [SerializeField] public float TKDamageScaling = 0;
    [SerializeField] public int extraProjectiles = 0;
    [SerializeField] public float attackSpeedScaling = 0;
    [SerializeField] public float fadeDurationMulti = 1f;
    [SerializeField] public float bleedDamageScaling = 0f;
    [SerializeField] public float bleedDurationScaling = 0f;
}