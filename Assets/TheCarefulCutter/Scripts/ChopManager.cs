using UnityEngine;

public class ChopManager : MonoBehaviour
{
    public KeyCode chopKey = KeyCode.Space;
    public int score = 0;
    public bool gameOver = false;

    public Animator knifeAnimator;
    public HazardManager hazardManager;
    public GameManager gameManager;
    public AudioSource chopSound;
    public AudioSource failSound;

    void Update()
    {
        if (gameOver) return;

        if (Input.GetKeyDown(chopKey))
        {
            knifeAnimator.SetTrigger("Chop");
            chopSound?.Play();

            if (hazardManager.IsFingerPresent)
            {
                failSound?.Play();
                gameOver = true;
                gameManager.GameOver(false);
            }
            else
            {
                score++;
                gameManager.UpdateScore(score);
            }
        }
    }
}