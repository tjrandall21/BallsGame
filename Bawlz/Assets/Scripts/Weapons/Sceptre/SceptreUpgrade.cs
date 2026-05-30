using UnityEngine;

[CreateAssetMenu(fileName = "SceptreUpgrade", menuName = "Weapon Upgrades/Sceptre Upgrades/SceptreUpgrade")]
public class SceptreUpgrade : WeaponUpgrade
{
    public float minionDamageScaling = 0;
    public float minionDamage = 0;
    public float minionHealth = 0;
    public float minionHealthScaling = 0;
    public int minionsPerHit = 0;
    public float extraMinionChance = 0;
    public float superMinionChance = 0;
    public float superMinionDamageMult = 1;
    public float superMinionHealthMult = 1;
}
