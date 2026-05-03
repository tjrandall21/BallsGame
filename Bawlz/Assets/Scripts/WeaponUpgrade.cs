using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponUpgrade", menuName = "Scriptable Objects/WeaponUpgrade")]
//will probably need to extend this class for each weapon type (swordUpgrade, bowUpgrade, etc)
public class WeaponUpgrade : ScriptableObject
{
    public Sprite shopIcon = null;
    public string upgradeName = "Weapon Upgrade";
    public string description = "This is a weapon upgrade.";
    public bool stackable = true;
    public float damage = 0;
    public float knockbackSpeed = 0;
    public float attackCooldown = 0;
    public float attackCooldownMultiplier = 1;
    public BallController parentBall = null;
    public Weapon parentWeapon = null;

    

    public void Init(BallController ball, Weapon weapon)
    {
        parentBall = ball;
        parentWeapon = weapon;
    }


    //override any of the following functions to add behaviour to specific upgrades

    public virtual void Update()
    {
        //Called every frame
    }
    public virtual void OnBallHit(BallController otherBall)
    {
        //Called when this weapon hits another ball
    }
    public virtual void OnWeaponHit(Weapon otherWeapon)
    {
        //called when this weapon hits another weapon
    }
    public virtual void OnAttack()
    {
        //called when this weapon uses its cooldown attack
    }
    public virtual void OnAttackEnd()
    {
        //called when this weapon finishes its cooldown attack
    }
    public virtual void OnRoundStart()
    {
        //called when the round starts
    }


    //ball functions

    public virtual void OnBallSpawned(BallController newBall)
    {
        //called when this ball spawns another ball
    }
    public virtual void OnDamageTaken(float amount)
    {
        //called when the ball takes damage from any source
    }
    public virtual void OnWeaponCollision(Weapon weapon)
    {
        //called when the ball gets hit by a weapon or projectile
    }
    public virtual void OnBallCollision(BallController otherBall)
    {
        //called when the ball collides directly with another ball
    }
    public virtual void OnWallCollision()
    {
        //called when the ball bounces off of a wall
    }
}
