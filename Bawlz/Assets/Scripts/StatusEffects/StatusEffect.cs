using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "Status Effects/StatusEffect")]
public class StatusEffect : ScriptableObject
{
    [SerializeField] protected ParticleSystem Fx;
    [SerializeField] protected AudioClip SFX;

    public string statusName = "Status Effect";
    public string StatusName { get { return statusName; } }
    [SerializeField] public bool stackable = true;

    protected BallController appliedBall;
    protected BallController sourceBall;
    public float statusDuration;    
    protected float statusTimer;

    private AudioSource loopingAudio;

    public virtual void Init(BallController appliedBall, BallController sourceBall)
    {
        this.appliedBall = appliedBall;
        this.sourceBall = sourceBall;
        statusTimer = statusDuration;
        OnStatusApplied();
    }

    public virtual void OnStatusApplied()
    {
        PlayTickFX();
    }

    public void PlayTickFX()
    {
        if (appliedBall == null) return;
        FXManager.Instance.PlayFX(Fx, appliedBall.transform.position);
        // SFX is handled by the looping audio, not per tick
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

    public virtual void OnStatusEnd() { }
    public virtual void OnStatusRefresh() { }
    public virtual void OnBallSpawned(BallController newBall) { }
    public virtual void OnDamageTaken(float amount) { }
    public virtual void OnWeaponCollision(Weapon weapon) { }
    public virtual void OnBallCollision(BallController otherBall) { }
    public virtual void OnWallCollision() { }



    public void StartLoopSFX()
    {
        if (SFX == null || appliedBall == null) return;
        GameObject audioObject = new GameObject("LoopingSFX");
        audioObject.transform.SetParent(appliedBall.transform);
        audioObject.transform.localPosition = Vector3.zero;
        loopingAudio = audioObject.AddComponent<AudioSource>();
        loopingAudio.clip = SFX;
        loopingAudio.loop = true;
        loopingAudio.volume = 0.5f;
        loopingAudio.spatialBlend = 1f;
        loopingAudio.Play();
    }  // sfx stuff

    public void StopLoopSFX()
    {
        if (loopingAudio == null) return;
        loopingAudio.Stop();
        Object.Destroy(loopingAudio.gameObject);
        loopingAudio = null;
    }
}