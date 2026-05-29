using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color urgentColor = Color.red;
    [SerializeField] private float urgentThreshold = 10f;

    private void OnEnable()
    {
        GameEvents.OnTimerUpdated += UpdateDisplay;
        GameEvents.OnTimerExpired += OnTimerExpired;
    }

    private void OnDisable()
    {
        GameEvents.OnTimerUpdated -= UpdateDisplay;
        GameEvents.OnTimerExpired -= OnTimerExpired;
    }

    private void UpdateDisplay(float time)
    {
        int seconds = Mathf.CeilToInt(time);
        timerText.text = seconds.ToString();
        timerText.color = time <= urgentThreshold ? urgentColor : normalColor;
    }

    private void OnTimerExpired()
    {
        timerText.text = "0";
        timerText.color = urgentColor;
    }
}