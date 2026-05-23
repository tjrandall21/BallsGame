using UnityEngine;

[CreateAssetMenu(fileName = "AdrenalineRush", menuName = "Scriptable Objects/AdrenalineRush")]
public class AdrenalineRush : Upgrade
{
    [SerializeField] float speedIncrease = 0.5f;

    public override void OnWeaponCollision(Weapon weapon)
    {
        base.OnWeaponCollision(weapon);
        parentBall.speed += speedIncrease;
    }

}
