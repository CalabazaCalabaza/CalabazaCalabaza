using System.Collections;
using UnityEngine;
using TMPro;

public class IntroTimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    private void Update()
    {
        if (IntroTimer.Instance == null) return;
        timerText.text = Mathf.CeilToInt(IntroTimer.Instance.TimeRemaining).ToString();
    }
}