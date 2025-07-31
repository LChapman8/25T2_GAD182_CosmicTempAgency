using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public Button playButton;
    public GameObject mainMenuUI;
    public GameObject gameplayUI;

    public AudioClip bellSFX;
    public AudioClip introMusic;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        AudioManagerStrongman.Instance.PlayMusic(introMusic);

        playButton.onClick.AddListener(() =>
        {
            AudioManagerStrongman.Instance.PlaySFX(bellSFX);
            Invoke(nameof(StartGame), 1f); // Slight delay for bell sound
        });
    }

    void StartGame()
    {
        mainMenuUI.SetActive(false);
        gameplayUI.SetActive(true);
    }
}
