using UnityEngine;
using UnityEngine.UI;

public class TestMenuButton : MonoBehaviour
{

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        GameManager.Instance.LoadBattleScene();
    }
}
