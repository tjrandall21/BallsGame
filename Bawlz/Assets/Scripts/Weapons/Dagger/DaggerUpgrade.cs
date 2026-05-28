using UnityEngine;

[CreateAssetMenu(fileName = "DaggerUpgrade", menuName = "Weapon Upgrades/Dagger Upgrades/DaggerUpgrade")]
public class DaggerUpgrade : MonoBehaviour
{
    [SerializeField] public float DaggerSpeed = 0;
    [SerializeField] public float DaggerDamage = 0;
    [SerializeField] public float DaggerDamageScaling = 0;
    [SerializeField] public int extraProjectiles = 0;
    [SerializeField] public float attackSpeedScaling = 0;
}
