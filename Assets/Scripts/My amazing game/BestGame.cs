using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BestGame : MonoBehaviour
{
    public void go_back()
    {
        SceneManager.LoadScene("Main menu");
    }
}
