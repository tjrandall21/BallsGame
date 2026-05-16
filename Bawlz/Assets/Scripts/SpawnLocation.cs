using Unity.VisualScripting;
using UnityEngine;

public class SpawnLocation : MonoBehaviour
{
    [SerializeField] int forPlayerAmount = 4;
    [SerializeField] int playerNum;

    void Awake()
    {
        if (forPlayerAmount == GameManager.Instance.PlayerCount)
        {   
            GameManager.Instance.AddSpawnLocation(transform.position, playerNum-1);
        }
    }

    void OnDrawGizmos()
    {
        if (forPlayerAmount == 4)
            Gizmos.color = Color.red;
        else if (forPlayerAmount == 3)
            Gizmos.color = Color.green;
        else if (forPlayerAmount == 2)
            Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position,0.64f);
    }
}
