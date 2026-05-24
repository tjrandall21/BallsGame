using UnityEngine;

[CreateAssetMenu(fileName = "HammerUpgrade", menuName = "Weapon Upgrades/Hammer Upgrades/HammerUpgrade")]
public class HammerUpgrade : WeaponUpgrade
{
    [SerializeField] public float u_baseDmg = 0;
    [SerializeField] public float u_maxWeaponSpin = 0;
    [SerializeField] public float u_baseRotationSpeed = 0;
    [SerializeField] public float u_rotationAcceleration = 0;
    [SerializeField] public float u_maxSpinIncreasePerHit = 0;

    public virtual void OnBallHit(BallController otherBall) { }
    public virtual void OnWeaponHit(Weapon otherWeapon) { }
}