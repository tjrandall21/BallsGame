using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class CharacterSelectCell : MonoBehaviour
{
    TextMeshProUGUI characterName = null;
    void Start()
    {
        characterName = GetComponentInChildren<TextMeshProUGUI>();
        characterName.text = name;
    }
    void Update()
    {
        characterName.text = name;
    }
}
