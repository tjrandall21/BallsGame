using UnityEngine;

[CreateAssetMenu(fileName = "BowUpgrade", menuName = "Weapon Upgrades/Bow Upgrades/BowUpgrade")]
public class BowUpgrade : WeaponUpgrade
{
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
