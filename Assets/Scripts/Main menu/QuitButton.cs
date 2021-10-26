using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void quit_game()
    {
        Debug.Log("exitgame");
        Application.Quit();
    }
}
