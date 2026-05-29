using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
{
    [SerializeField] private Image[] hearts;

    [SerializeField] private Color fullColor = Color.red;
    [SerializeField] private Color emptyColor = Color.gray;

    private void OnEnable()
    {
        GameEvents.OnLivesChanged += UpdateHearts;
    }

    private void OnDisable()
    {
        GameEvents.OnLivesChanged -= UpdateHearts;
    }

    private void Start()
    {
        UpdateHearts(hearts.Length);
    }

    private void UpdateHearts(int currentLives)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].color = i < currentLives ? fullColor : emptyColor;
        }
    }
}