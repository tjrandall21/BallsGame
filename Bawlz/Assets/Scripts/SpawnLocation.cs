using UnityEngine;

public class SpawnLocation : MonoBehaviour
{
    void Awake()
    {
        GameManager.Instance.AddSpawnLocation(transform.position);
    }
}
