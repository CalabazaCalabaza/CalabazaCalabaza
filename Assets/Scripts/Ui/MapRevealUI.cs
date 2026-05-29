using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapRevealUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Image mapImage;
    [SerializeField] private Sprite mapSprite;

    [Header("Settings")]
    [SerializeField] private float displayDuration = 4f;
    [SerializeField] private string nextSceneName = "Level2";

    private void OnEnable()
    {
        GameEvents.OnMapReveal += Show;
    }

    private void OnDisable()
    {
        GameEvents.OnMapReveal -= Show;
    }

    private void Start()
    {
        panel.SetActive(false);
    }

    private void Show()
    {
        panel.SetActive(true);
        mapImage.sprite = mapSprite;
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(displayDuration);
        SceneManager.LoadScene(nextSceneName);
    }
}