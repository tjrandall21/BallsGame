using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "TeleportUpgrade", menuName = "Weapon Upgrades/TK Upgrades/TeleportUpgrade")]
public class TKTeleport : TKUpgrade
{
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float teleportChance = 0.3f;
    [SerializeField] float teleportCooldown = 3f;
    [SerializeField] float explosionDamage = 5f;
    [SerializeField] float explosionKnockback = 10f;
    [SerializeField] float explosionSize = 1f;
    float teleportTimer = 0;

    List<TKProdj> stuckKnives = new List<TKProdj>();

    [SerializeField] GameObject ballAreaPrefab = null;
    BallArea ballArea = null;
    [SerializeField] float radius = 1.5f;

    List<BallController> ballsInContact = new List<BallController>();


    public override void Init(BallController ball, Weapon weapon)
    {
        base.Init(ball, weapon);
        teleportTimer = teleportCooldown;
        GameObject area = Instantiate(ballAreaPrefab, parentBall.transform);
        ballArea = area.GetComponent<BallArea>();
        ballArea.Init(this, radius, parentBall.gameObject.layer+4);
    }

    public override void Update()
    {
        base.Update();
        if (teleportTimer > 0)
        {
            teleportTimer -= Time.deltaTime;
        }
        else if (parentBall != null && CheckBallsInRange())
        {
            Debug.Log(ballsInContact.Count);
            Teleport();
        }
    }

    bool CheckBallsInRange()
    {
        ballsInContact.RemoveAll(item => item == null);
        ballsInContact = ballsInContact.Distinct().ToList();
        for (int i = ballsInContact.Count-1; i >= 0; i--)
        {
            if ((ballsInContact[i].transform.position-parentBall.transform.position).magnitude > radius+ballsInContact[i].GetComponent<CircleCollider2D>().radius)
            ballsInContact.RemoveAt(i);
        }
        return ballsInContact.Count > 0;
    }

    public override void OnProjectileWallHit(TKProdj projectile)
    {
        base.OnProjectileWallHit(projectile);
        if (projectile.Stuck && !stuckKnives.Contains(projectile))
        {
            stuckKnives.Add(projectile);
        }
    }

    public override void OnProjectileTimeout(TKProdj projectile)
    {
        if (stuckKnives.Contains(projectile))
        {
            stuckKnives.Remove(projectile);
        }

        base.OnProjectileTimeout(projectile);
    }

    void Teleport()
    {
        stuckKnives.RemoveAll(item => item == null);
        if (stuckKnives.Count > 0)
        {
            TKProdj projectile = stuckKnives[Random.Range(0,stuckKnives.Count)];
            
            GameObject explosionObject = Instantiate(explosionPrefab);
            explosionObject.layer = parentBall.gameObject.layer+4;
            Explosion explosion = explosionObject.GetComponent<Explosion>();
            explosion.ExplosionInit(explosionDamage,parentBall.transform.position,explosionKnockback,explosionSize);

            ballsInContact.Clear();
            parentBall.transform.position = projectile.transform.position;
            teleportTimer = teleportCooldown;

            stuckKnives.Remove(projectile);
            Destroy(projectile.gameObject);
            teleportTimer = teleportCooldown;
        }
    }

    public void BallEnteredArea(BallController otherBall)
    {
        ballsInContact.RemoveAll(item => item == null);
        ballsInContact.Add(otherBall);
    }
    
    public void BallExitedArea(BallController otherBall)
    {
        Debug.Log("Ball Exited");
        ballsInContact = ballsInContact.Distinct().ToList();
        ballsInContact.Remove(otherBall);
        ballsInContact.RemoveAll(item => item == null);
    }
}