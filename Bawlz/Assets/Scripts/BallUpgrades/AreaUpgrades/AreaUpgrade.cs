using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

[CreateAssetMenu(fileName = "AreaUpgrade", menuName = "Ball Upgrades/Area Upgrades/AreaUpgrade")]
public class AreaUpgrade : Upgrade
{
    [SerializeField] GameObject ballAreaPrefab = null;
    BallArea ballArea = null;
    [SerializeField] float radius = 1.5f;

    List<BallController> ballsInContact = new List<BallController>();

    public override void Init(BallController ball)
    {
        base.Init(ball);
        GameObject area = Instantiate(ballAreaPrefab, parentBall.transform);
        ballArea = area.GetComponent<BallArea>();
        ballArea.Init(this, radius, parentBall.gameObject.layer+4);
    }

    public override void Update()
    {
        base.Update();
        ballsInContact.RemoveAll(item => item == null);
        foreach (BallController ball in ballsInContact)
        {
            BallUpdate(ball);
        }
        ballArea.transform.rotation = quaternion.identity;
    }

    public virtual void BallEnteredArea(BallController otherBall)
    {
        ballsInContact.RemoveAll(item => item == null);
        ballsInContact.Add(otherBall);
    }

    public virtual void BallUpdate(BallController otherBall)
    {
        
    }
    
    public virtual void BallExitedArea(BallController otherBall)
    {
        ballsInContact.Remove(otherBall);
        ballsInContact.RemoveAll(item => item == null);
    }


}
