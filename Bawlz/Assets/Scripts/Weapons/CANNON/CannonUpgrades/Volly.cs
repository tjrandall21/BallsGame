using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "BurstUpgrade", menuName = "Weapon Upgrades/BurstUpgrade")]
public class BurstUpgrade : CannonUpgrade
{
    [SerializeField, Tooltip("Number of volleys in the burst")] int burstCount = 3;
    [SerializeField, Tooltip("Delay in seconds between each volley")] float timeBetweenShots = 0.1f;

    public override void OnAttack()
    {
        Cannon cannon = parentWeapon as Cannon;
        if (cannon == null) return;

        cannon.StartCoroutine(FireBurst(cannon));
    }

    private IEnumerator FireBurst(Cannon cannon)
    {
        // First volley already fired by Cannon.OnAttack, so fire burstCount - 1 more
        for (int i = 1; i < burstCount; i++)
        {
            yield return new WaitForSeconds(timeBetweenShots);
            cannon.FireVolley();
        }
    }
}