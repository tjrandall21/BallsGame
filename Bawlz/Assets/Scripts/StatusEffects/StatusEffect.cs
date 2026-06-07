using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "Status Effects/StatusEffect")]
public class StatusEffect : ScriptableObject
{
    [SerializeField] ParticleSystem Fx;
    [SerializeField] AudioClip SFX;

    public string statusName = "Status Effect";
    public string StatusName { get { return statusName; } }
    [SerializeField] public bool stackable = true;

    protected BallController appliedBall;
    protected BallController sourceBall;
    public float statusDuration;
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

    }

    public void PlayTickFX()
    {
       
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
    }

    public virtual void OnStatusRefresh() { }
    public virtual void OnBallSpawned(BallController newBall) { }
    public virtual void OnDamageTaken(float amount) { }
    public virtual void OnWeaponCollision(Weapon weapon) { }
    public virtual void OnBallCollision(BallController otherBall) { }
    public virtual void OnWallCollision() { }
}