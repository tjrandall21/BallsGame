using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReadyButton : MonoBehaviour
{
    [SerializeField] Button readyButton;
    [SerializeField] static int readyCount = 0;
    [SerializeField] bool deactivateGameObjectInsteadOfInteractable = false;
    [SerializeField] bool canUnready = false;
    [SerializeField] Color readyColor;
    [SerializeField] Color unreadyColor;
    bool ready = false;

    void Awake()
    {
        if (readyButton == null)
        {
            readyButton = GetComponent<Button>();
            readyButton.onClick.AddListener(ReadyClick);
        }
    }

    void Start()
    {
        readyCount = 0;
        UpdateButtonState();
    }

    // set the exact number of players currently "ready"
    public void SetReadyCount(int count)
    {
        readyCount = Mathf.Max(0, count);
        UpdateButtonState();
    }

    // player becomes ready
    public void IncrementReady()
    {
        readyCount++;
        UpdateButtonState();
    }

    // player cancels ready
    public void DecrementReady()
    {
        readyCount = Mathf.Max(0, readyCount - 1);
        UpdateButtonState();
    }

    public void ReadyClick()
    {
        if (ready)
        {
            ready = false;
            DecrementReady();
        }
        else
        {
            ready = true;
            IncrementReady();
        }
    }

    void UpdateButtonState()
    {
        int required = 0;
        if (GameManager.Instance != null)
        {
            required = GameManager.Instance.PlayerCount;
        }
        if (readyCount >= required)
        {
            GameManager.Instance.LoadBattleScene();
            return;
        }

        if (ready && !canUnready)
        {
            if (deactivateGameObjectInsteadOfInteractable)
            {
                gameObject.SetActive(false);
            }
            else if (readyButton != null)
            {
                readyButton.interactable = false;
            }
        }
        else if (ready)
        {
            readyButton.GetComponentInChildren<Image>().color = readyColor;
            readyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Unready";
        }
        else
        {
            readyButton.GetComponentInChildren<Image>().color = unreadyColor;
            readyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Ready!";
        }
    }
}
