using UnityEngine;

[CreateAssetMenu(fileName = "SpikeTrapBallUpgrade", menuName = "Scriptable Objects/SpikeTrapBallUpgrade")]
public class SpikeTrapBallUpgrade : Upgrade
{
    [SerializeField] GameObject spikeTrapPrefab;
    public float duration = 10f;
    public float spawnCooldown = 0.5f;

    public override void OnWeaponCollision(Weapon weapon)
    {
        base.OnWeaponCollision(weapon);
        GameObject trapObject = Instantiate(spikeTrapPrefab, parentBall.transform.position, Quaternion.identity);
        BearTrap bearTrap = trapObject.GetComponent<BearTrap>();
        bearTrap.duration = duration;
        bearTrap.TrapInit(0, 0, parentBall);
    }

}
