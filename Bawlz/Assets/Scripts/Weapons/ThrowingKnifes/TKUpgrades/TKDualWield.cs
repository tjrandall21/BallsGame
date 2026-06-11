using UnityEngine;

[CreateAssetMenu(fileName = "DualWeild ThrowingKnives", menuName = "Weapon Upgrades/TK Upgrades/DualWeild")]
public class TKDualWield : TKUpgrade
{
    [SerializeField] GameObject secondDaggerPrefab;
    public override void Init(BallController ball, Weapon weapon)
    {
        base.Init(ball, weapon);
        BoxCollider2D boxCollider = parentWeapon.GetComponent<BoxCollider2D>();
        boxCollider.size = new Vector2(boxCollider.size.x,3.4f);
        boxCollider.offset = new Vector2(0,-0.1f);
        GameObject dualWield = Instantiate(secondDaggerPrefab, ball.transform);
    }
}
