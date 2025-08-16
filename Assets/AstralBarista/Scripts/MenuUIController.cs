using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIController : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject howToPanel;
    [SerializeField] private BaristaMinigameManager manager;

    void Awake()
    {
        ShowMain();
    }

    public void OnPlayClicked()
    {
        // Hide menu and start the round
        gameObject.SetActive(false);
        manager.BeginRound();
    }

    public void OnHowToClicked() => ShowHowTo();
    public void OnBackClicked() => ShowMain();

    private void ShowMain()
    {
        if (mainPanel) mainPanel.SetActive(true);
        if (howToPanel) howToPanel.SetActive(false);
    }

    private void ShowHowTo()
    {
        if (mainPanel) mainPanel.SetActive(false);
        if (howToPanel) howToPanel.SetActive(true);
    }
}
