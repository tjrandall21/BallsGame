using UnityEngine;

public class FXManager : MonoBehaviour
{
    public static FXManager Instance;

    [Header("Audio")]
    [SerializeField] AudioClip playerDeathSFX;
    [SerializeField] AudioClip minionDeathSFX;

    [Header("WEAPONS")]
    [SerializeField] AudioClip SwordHitSFX;
    [SerializeField] AudioClip HammerHitSFX;
    [SerializeField] AudioClip DaggerHitSFX;
    [SerializeField] AudioClip SceptreHitSFX;

    [Header("PROJECTILES FIRE")]
    [SerializeField] AudioClip CannonFireSFX;
    [SerializeField] AudioClip TKHitSFX;
    [SerializeField] AudioClip ArrowReleaseSFX;

    [Header("PROJECTILES HIT")]
    [SerializeField] AudioClip ArrowHitSFX;
    [SerializeField] AudioClip CannonHitSFX;



    [Header("Particle Systems")]
    [SerializeField] ParticleSystem playerDeathFX;
    [SerializeField] ParticleSystem minionDeathFX;

    [Header("Audio Settings")]
    [SerializeField, Range(0f, 1f)] float sfxVolume = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayWeaponHit(Vector3 pos, Weapon weapon)
    {
        AudioClip weaponSFX = null;

        switch (weapon)
        {
            case Hammer:
                weaponSFX = HammerHitSFX;
                break;
            case Dagger:
                weaponSFX = DaggerHitSFX;
                break;
            case Sceptre:
                weaponSFX = SceptreHitSFX;
                break;
            case Sword:
                weaponSFX = SwordHitSFX;
                break;
            case Cannon:
                weaponSFX = CannonFireSFX;
                break;

            case Bow:
                weaponSFX = ArrowReleaseSFX;
                break;

            default:
                return;
        }

        PlaySFX(weaponSFX, pos); // Fixed to use PlaySFX for audio clips
    }
    public void PlayProjectileFire(Vector3 pos, Projectile proj)
    {
        AudioClip projSFX = null;

        switch (proj)
        {
            case TKProdj:
                projSFX = TKHitSFX;
                break;
            case CannonProdj:
                projSFX = CannonHitSFX;
                break;
            case BowProjectile:
                projSFX = ArrowHitSFX;
                break;
            default:
                return;
        }

        PlaySFX(projSFX, pos); // Fixed to use PlaySFX for audio clips
    }
    public void PlayStausEffect(Vector3 pos, StatusEffect effect)
    {
        switch (effect)
        {
         
        }
    }

    public void PlayPlayerDeath(Vector3 pos)
    {
        PlaySFX(playerDeathSFX, pos);
        PlayFX(playerDeathFX, pos);
    } 
    public void PlayMinionDeath(Vector3 pos)
    { 
        PlaySFX(minionDeathSFX, pos);
        PlayFX(minionDeathFX, pos);
    }
    public void PlayDeath(GameObject entity)
    {
        if (entity.CompareTag("Minion") || entity.CompareTag("Rat"))
        {
            PlayMinionDeath(entity.transform.position);
        } 
        else PlayPlayerDeath(entity.transform.position);
    }

    public void PlaySFX(AudioClip clip, Vector3 pos)
    {
        if (clip == null)
        {
            Debug.LogWarning("Audio clip is null!");
            return;
        }

        // Dynamically create an AudioSource for this sound effect
        GameObject audioObject = new GameObject("TempAudio");
        AudioSource source = audioObject.AddComponent<AudioSource>();
        source.transform.position = pos;
        source.clip = clip;
        source.pitch = Random.Range(0.8f, 1.2f);
        source.volume = sfxVolume;
        
        source.Play();
        Destroy(audioObject, clip.length);
    }
    public void PlayFX(ParticleSystem particleSystem, Vector3 pos)
    {
        if (particleSystem == null)
        {
            Debug.LogWarning("Particle system is null!");
            return;
        }
        ParticleSystem instance = Instantiate(particleSystem, pos, Quaternion.identity);
        instance.Play();
        Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
    }
}