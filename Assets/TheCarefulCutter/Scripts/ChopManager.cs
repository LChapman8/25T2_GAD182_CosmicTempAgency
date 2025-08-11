using UnityEngine;

public class ChopManager : MonoBehaviour
{
    public KeyCode chopKey = KeyCode.Space;
    public bool gameOver = false;

    public Animator knifeAnimator;
    public HazardManager hazardManager;
    public GameManager gameManager;
    public AudioManager audioManager;
    public VegetableManager vegetableManager;

    public float gameDuration = 30f;
    private float timeRemaining;
    private int score = 0;

    void Start()
    {
        timeRemaining = gameDuration;
        vegetableManager.StartGame(this);
    }

    void Update()
    {
        if (gameOver) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0f)
        {
            gameOver = true;
            gameManager.GameOver(true);
            return;
        }

        if (Input.GetKeyDown(chopKey))
        {
            knifeAnimator.SetTrigger("Chop");
            audioManager.PlayChop();

            if (hazardManager.IsFingerPresent)
            {
                audioManager.PlayFail();
                gameOver = true;
                gameManager.GameOver(false);
            }
            else
            {
                vegetableManager.RegisterChop();
            }
        }
    }

    public void IncreaseScore(int value)
    {
        score += value;
        gameManager.UpdateScore(score);
    }
}
