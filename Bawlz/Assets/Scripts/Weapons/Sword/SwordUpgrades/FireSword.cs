using UnityEngine;

[CreateAssetMenu(fileName = "FireSword", menuName = "Weapon Upgrades/Sword Upgrades/Fire Sword")]
public class FireSword : SwordUpgrade
{
    [SerializeField] GameObject firePatchPrefab;
    [SerializeField] float firePatchDmg = 0;
    [SerializeField] float firePatchCD = 2;
    [SerializeField] float firePatchSize = 1.2f;
    [SerializeField] float firePatchDuration = 3f;



    public override void OnBallHit(BallController otherBall)
    {
        base.OnBallHit(otherBall);
        GameObject fireObject = Instantiate(firePatchPrefab);
        fireObject.layer = parentBall.gameObject.layer+4;
        StaticDmgArea firePatch = fireObject.GetComponent<StaticDmgArea>();
        firePatch.Init(firePatchDmg,parentWeapon.transform.position,firePatchDuration,firePatchSize);
    }
}
