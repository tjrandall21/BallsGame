using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class PreRoundPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countdownText;
    [SerializeField] TextMeshProUGUI roundText;
    [SerializeField] float countdownDuration = 3;
    float countdownTimer = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        roundText.text = $"Round {GameManager.Instance.roundNumber} of {GameManager.Instance.maxRounds}";
        countdownTimer = countdownDuration;
        GameManager.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        countdownTimer -= Time.unscaledDeltaTime;
        countdownText.text = math.ceil(countdownTimer).ToString();
        if (countdownTimer <= 0)
        {
            GameManager.Unpause();
            GameManager.Instance.OnRoundStart();
            gameObject.SetActive(false);
        }
    }
}
