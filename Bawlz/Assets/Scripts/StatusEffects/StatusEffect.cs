using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "Scriptable Objects/StatusEffect")]
public class StatusEffect : ScriptableObject
{
    [SerializeField] protected string statusName = "Status Effect";
    public string StatusName {get{return statusName;}}
    [SerializeField] public bool stackable = true;
    protected BallController appliedBall;
    protected BallController sourceBall;
    [SerializeField] protected float statusDuration;
    protected float statusTimer;

    public virtual void Init(BallController appliedBall, BallController sourceBall)
    {
        this.appliedBall = appliedBall;
        this.sourceBall = sourceBall;
        statusTimer = statusDuration;
        OnStatusApplied();
    }

    public virtual void OnStatusApplied()
    {
        //override to add effects when the status begins
    }

    public virtual void Update()
    {
        statusTimer -= Time.deltaTime;
        if (statusTimer <= 0)
        {
            OnStatusEnd();
            appliedBall.RemoveStatus(this);
        }
    }

    public virtual void OnStatusEnd()
    {
        //override to add(or remove) effects when the status ends
    }


    //  ball functions

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
