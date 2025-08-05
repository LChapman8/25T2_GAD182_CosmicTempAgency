using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerBarController : MonoBehaviour
{
    [Header("Gameplay Settings")]
    public float maxPower = 100f;
    public float decayRate = 15f;
    public float powerGainPerPress = 10f;
    public float spaceSFXCooldown = 2f;
    public float level1Threshold = 30f;
    public float level2Threshold = 60f;

    [Header("Timer")]
    public float roundDuration = 10f;
    private float timeRemaining;

    [Header("UI Elements")]
    public Slider powerSlider;
    public TextMeshProUGUI timerText;

    [Header("Character Models")]
    public GameObject level0Model;
    public GameObject level1Model;
    public GameObject level2Model;

    private float currentPower = 0f;
    private bool isGameActive = false;
    private float spaceSFXTimer = 0f;
    private int currentLevel = 0;

    [Header("Audio Clips")]
    public AudioClip spaceSFX;
    public AudioClip levelUpSFX;
    public AudioClip cheerSFX;
    public AudioClip booSFX;

    [Header("MainMenuUI")]
    public MainMenuUI menuUI;

  

    void Update()
    {
        if (!isGameActive) return;

        HandleInput();
        UpdatePower();
        UpdateTimer();
        UpdateCharacter();
    }

    void HandleInput()
    {
        spaceSFXTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentPower = Mathf.Min(currentPower + powerGainPerPress, maxPower);

            if (spaceSFXTimer <= 0f)
            {
                AudioManagerStrongman.Instance.PlaySFX(spaceSFX);
                spaceSFXTimer = spaceSFXCooldown;
            }
        }
    }

    void UpdatePower()
    {
        currentPower = Mathf.Max(currentPower - decayRate * Time.deltaTime, 0f);
        powerSlider.value = currentPower / maxPower;
    }

    void UpdateTimer()
    {
        timeRemaining -= Time.deltaTime;
        timerText.text = Mathf.Ceil(timeRemaining).ToString();

        if (timeRemaining <= 0f)
        {
            EndRound();
        }
    }

    void UpdateCharacter()
    {
        if (currentPower >= level2Threshold && currentLevel < 2)
        {
            currentLevel = 2;
            SetModelActive(2);
            AudioManagerStrongman.Instance.PlaySFX(levelUpSFX);
        }
        else if (currentPower >= level1Threshold && currentLevel < 1)
        {
            currentLevel = 1;
            SetModelActive(1);
            AudioManagerStrongman.Instance.PlaySFX(levelUpSFX);
        }
    }

    void SetModelActive(int level)
    {
        level0Model.SetActive(level == 0);
        level1Model.SetActive(level == 1);
        level2Model.SetActive(level == 2);
    }

    void EndRound()
    {
        isGameActive = false;

        bool didWin = currentLevel == 2;

        menuUI.ShowEndGameUI(didWin);
    }

    public void StartRound()
    {
        currentPower = 0f;
        currentLevel = 0;
        timeRemaining = roundDuration;
        isGameActive = true;
        powerSlider.value = 0f;
        timerText.text = timeRemaining.ToString();

        SetModelActive(0);
    }

    public void ResetRound()
    {
        currentPower = 0f;
        currentLevel = 0;
        powerSlider.value = 0f;
        timerText.text = "";
        isGameActive = false;

        SetModelActive(0);
    }
}
