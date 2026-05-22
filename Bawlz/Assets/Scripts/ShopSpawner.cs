using UnityEngine;

public class ShopSpawner : MonoBehaviour
{
    [SerializeField] GameObject playerShopPrefab;
    [SerializeField] Transform shopParent;

    void Start()
    {
        SpawnShops();
    }

    void SpawnShops()
    {
        int playerCount = GameManager.Instance.players.Count;

        Vector2[] positions =
        {
            new Vector2(0.25f, 0.75f),   // P1 top left
            new Vector2(0.75f, 0.75f),   // P2 top right
            new Vector2(0.25f, 0.25f),   // P3 bottom left
            new Vector2(0.75f, 0.25f)    // P4 bottom right
        };

        for (int i = 0; i < playerCount; i++)
        {
            GameObject shop = Instantiate(playerShopPrefab);

            // parent UI
            shop.transform.SetParent(shopParent, false);

            // move UI
            RectTransform rect = shop.GetComponent<RectTransform>();

            rect.anchorMin = positions[i];
            rect.anchorMax = positions[i];

            // assign player
            shop.GetComponent<PlayerShop>().SetPlayer(i);
        }
    }
}