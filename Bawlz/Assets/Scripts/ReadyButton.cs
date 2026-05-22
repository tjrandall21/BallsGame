using UnityEngine;
using UnityEngine.UI;

public class ReadyButton : MonoBehaviour
{
    [SerializeField] Button readyButton;
    [SerializeField] int readyCount = 0;
    [SerializeField] bool deactivateGameObjectInsteadOfInteractable = false;

    void Awake()
    {
        if (readyButton == null)
        {
            readyButton = GetComponent<Button>();
        }
    }

    void Start()
    {
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

    void UpdateButtonState()
    {
        int required = 0;
        if (GameManager.Instance != null)
        {
            required = GameManager.Instance.PlayerCount;
        }

        bool shouldDeactivate = (required > 0 && readyCount >= required);

        if (deactivateGameObjectInsteadOfInteractable)
        {
            gameObject.SetActive(!shouldDeactivate);
        }
        else if (readyButton != null)
        {
            readyButton.interactable = !shouldDeactivate;
        }
    }
}
