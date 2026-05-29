using UnityEngine;

[CreateAssetMenu(fileName = "SceptrePassiveMinions", menuName = "Weapon Upgrades/Sceptre Upgrades/PassiveMinionUpgrade")]
public class PassiveMinions : SceptreUpgrade
{
    [SerializeField] float minionSpawnRate = 3;
    float spawnTimer = 0;

    public override void Init(BallController ball, Weapon weapon)
    {
        base.Init(ball, weapon);
        spawnTimer = minionSpawnRate;
    }

    public override void Update()
    {
        base.Update();
        spawnTimer-=Time.deltaTime;
        if (spawnTimer<=0)
        {
            spawnTimer = minionSpawnRate;
            ((Sceptre)parentWeapon).SpawnMinion();
        }
    }
}
