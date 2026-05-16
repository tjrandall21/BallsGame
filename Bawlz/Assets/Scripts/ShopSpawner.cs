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
            new Vector2(-830, 400),   // P1 top left
            new Vector2( 180, 400),   // P2 top right
            new Vector2(-830,-180),   // P3 bottom left
            new Vector2( 180,-180)    // P4 bottom right
        };

        for (int i = 0; i < playerCount; i++)
        {
            GameObject shop = Instantiate(playerShopPrefab);

            // Parent correctly for UI
            shop.transform.SetParent(shopParent, false);

            // Move UI
            RectTransform rect =
                shop.GetComponent<RectTransform>();

            rect.anchoredPosition = positions[i];

            // Assign player
            shop.GetComponent<PlayerShop>()
                 .SetPlayer(i);
        }
    }
}