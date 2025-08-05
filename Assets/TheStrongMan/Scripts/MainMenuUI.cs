using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;  // <-- Add this

public class MainMenuUI : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuUI;
    public GameObject gameplayUI;
    public GameObject endGameUI;

    [Header("Buttons")]
    public Button playButton;
    public Button returnToMenuButton;

    [Header("Audio Clips")]
    public AudioClip introMusic;
    public AudioClip bellSFX;
    public AudioClip gameplayMusic;

    [Header("GameController Reference")]
    public PowerBarController powerBarController;

    [Header("End Game Elements")]
    public TextMeshProUGUI endGameResultText;
    public Image endGameBackground;

    private void Start()
    {
        mainMenuUI.SetActive(true);
        gameplayUI.SetActive(false);
        endGameUI.SetActive(false);

        AudioManagerStrongman.Instance.PlayMusic(introMusic);

        playButton.onClick.AddListener(OnPlayClicked);
        returnToMenuButton.onClick.AddListener(ReturnToMainMenu);
    }

    private void OnPlayClicked()
    {
        AudioManagerStrongman.Instance.StopMusic();
        AudioManagerStrongman.Instance.PlaySFX(bellSFX);
        Invoke(nameof(StartGame), 1f);
    }

    private void StartGame()
    {
        mainMenuUI.SetActive(false);
        gameplayUI.SetActive(true);
        endGameUI.SetActive(false);

        AudioManagerStrongman.Instance.PlayMusic(gameplayMusic);

        powerBarController.StartRound();
    }

    public void ShowEndGameUI(bool didWin)
    {
        gameplayUI.SetActive(false);
        endGameUI.SetActive(true);

        endGameResultText.text = didWin ? "Wow look at you strong man! congratulations you got the job!" : "You call that strong?! youre fired!";
        AudioManagerStrongman.Instance.PlaySFX(didWin ? powerBarController.cheerSFX : powerBarController.booSFX);
    }

    public void ReturnToMainMenu()
    {
        // Stop all sounds
        AudioManagerStrongman.Instance.StopMusic();
        // Optional: If you want to stop all SFX immediately, you might want to stop sfxSource too
        AudioManagerStrongman.Instance.sfxSource.Stop();

        // Load the scene named "MainMenu"
        SceneManager.LoadScene("MainMenu");

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
