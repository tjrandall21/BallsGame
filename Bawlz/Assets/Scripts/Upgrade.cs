using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Scriptable Objects/Upgrade")]
public class Upgrade : ScriptableObject
{
    public Sprite shopIcon = null;
    public string upgradeName = "Upgrade";
    public string description = "This is an upgrade.";
    public bool stackable = true; //can only be purchased once if false

    //these stats will be automatically added to the ball
    public float health = 0; 
    public float contactDamage = 0; 
    public float moveSpeed = 0;
    public float rotationSpeed = 0; //In degrees
    public float defenseMultiplier = 1;
    protected BallController parentBall = null;

    public void Init(BallController ball)
    {
        parentBall = ball;
    }

    public virtual void Update()
    {
        //Called every frame by the ball this upgrade is applied to
    }

    public virtual void OnRoundStart()
    {
        //called when the round starts
    }

    public virtual void OnBallSpawned(BallController newBall)
    {
        //called when the ball this upgrade is applied to spawns another ball
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
