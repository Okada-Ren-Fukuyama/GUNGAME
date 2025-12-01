using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayManager : MonoBehaviour
{
    public void ReplayGame()
    {
        // ¡‚ÌƒV[ƒ“‚ğ‚à‚¤ˆê“x“Ç‚İ‚Ş
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
