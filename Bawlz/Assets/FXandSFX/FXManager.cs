using UnityEngine;

public class FXManager : MonoBehaviour
{
    public static FXManager Instance;

    [Header("Audio")]
    [SerializeField] AudioClip weaponHitSFX;
    [SerializeField] AudioClip playerHitSFX;
    [SerializeField] AudioClip playerDeathSFX;
    [SerializeField] AudioClip minionDeathSFX;
    [SerializeField] AudioClip PlayerHitsPlayerSFX;

    [Header("Particles")]
    [SerializeField] ParticleSystem weaponVsWeaponFX;
    [SerializeField] ParticleSystem weaponVsPlayerFX;
    [SerializeField] ParticleSystem playerDeathFX;
    [SerializeField] ParticleSystem minionDeathFX;

    [SerializeField] int audioPoolSize = 10;
    private AudioSource[] audioPool;
    private int poolIndex;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        audioPool = new AudioSource[audioPoolSize];
        for (int i = 0; i < audioPoolSize; i++)
        {
            audioPool[i] = gameObject.AddComponent<AudioSource>();
            audioPool[i].playOnAwake = false;
        }
    }

    public void RegisterPlayer(AudioSource source) { }

    public void PlayWeaponHit(Vector3 pos) => PlayFX(weaponVsWeaponFX, weaponHitSFX, pos);
    public void PlayPlayerHit(Vector3 pos) => PlayFX(weaponVsPlayerFX, playerHitSFX, pos);
    public void PlayPlayerDeath(Vector3 pos) => PlayFX(playerDeathFX, playerDeathSFX, pos);
    public void PlayMinionDeath(Vector3 pos) => PlayFX(minionDeathFX, minionDeathSFX, pos);
    public void PlayPlayerHitsPlayer(Vector3 pos) => PlayFX(weaponVsPlayerFX, PlayerHitsPlayerSFX, pos);

    public void PlayDeath(GameObject entity)
    {
        if (entity.CompareTag("Minion")) PlayMinionDeath(entity.transform.position);
        else PlayPlayerDeath(entity.transform.position);
    }

    private void PlayFX(ParticleSystem prefab, AudioClip clip, Vector3 pos)
    {
        if (prefab != null)
        {
            ParticleSystem fx = Instantiate(prefab, pos, Quaternion.identity);
            fx.Play();
            Destroy(fx.gameObject, fx.main.duration + fx.main.startLifetime.constantMax);
        }
        if (clip == null) return;
        AudioSource source = GetFreeSource();
        source.transform.position = pos;
        source.clip = clip;
        source.pitch = Random.Range(0.9f, 1.1f);
        source.Play();
    }

    private AudioSource GetFreeSource()
    {
        foreach (var s in audioPool)
            if (!s.isPlaying) return s;
        return audioPool[poolIndex++ % audioPoolSize];
    }
}