using UnityEngine;

public class ShopEndButton : MonoBehaviour
{
    public void EndShop()
    {
        GameManager.Instance.LoadBattleScene();
    }
}
