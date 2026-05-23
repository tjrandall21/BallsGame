using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectButton : MonoBehaviour
{
    public GameObject weaponPrefab;
    public int playerNum = 1;

    void Start()
    {
        GetComponent<Image>().sprite = weaponPrefab.GetComponentInChildren<SpriteRenderer>().sprite; 
    }
}
