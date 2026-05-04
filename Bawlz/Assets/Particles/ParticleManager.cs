using UnityEngine;

public class ParticleHitManager : MonoBehaviour
{
    public static ParticleHitManager Instance;

    [SerializeField] ParticleSystem weaponVsWeaponFX;
    [SerializeField] ParticleSystem weaponVsPlayerFX;
    [SerializeField] ParticleSystem PlayerDeathFX;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayWeaponHit(Vector3 position)
    {
        Instantiate(weaponVsWeaponFX, position, Quaternion.identity);
    }

    public void PlayPlayerHit(Vector3 position)
    {
        Instantiate(weaponVsPlayerFX, position, Quaternion.identity);
    }

    public void PlayPlayerDeath(Vector3 position)
    {
        Instantiate(PlayerDeathFX, position, Quaternion.identity);
    }
}