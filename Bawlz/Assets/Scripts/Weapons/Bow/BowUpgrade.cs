using UnityEngine;

[CreateAssetMenu(fileName = "BowUpgrade", menuName = "Weapon Upgrades/Bow Upgrades/BowUpgrade")]
public class BowUpgrade : WeaponUpgrade
{
    [SerializeField] public float arrowSpeed = 0;
    [SerializeField] public float arrowDamage = 0;
    [SerializeField] public float arrowDamageScaling = 0;
    [SerializeField] public int extraProjectiles = 0;
    [SerializeField] public float attackSpeedScaling = 0;

    public bool passToArrows = false;
    
    public virtual void OnArrowBallHit(BallController otherBall, BowProjectile arrow)
    {

    }

    public virtual void OnArrowWallHit(BowProjectile arrow)
    {

    }

    public virtual void OnArrowWeaponHit(Weapon otherWeapon, BowProjectile arrow)
    {

    }

    public virtual void OnArrowDestroyed(BowProjectile arrow)
    {
        
    }
}
