using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreenScript : MonoBehaviour
{
    public GameObject gameEndScreen;
    public GameObject HUD;

    public void ActivateEndScreen()
    { 
    
        gameEndScreen.SetActive(true);
        
        HUD.SetActive(false);
    
    
    }







}
