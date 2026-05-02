using UnityEngine;

public class Sword : Weapon
{
    //damage scaling
    float damageScalingAmount = 1;

    public override void OnWallCollision()
    {
        base.OnWallCollision();
        damage += damageScalingAmount;
    }

    
}
