using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIEffectManager : MonoBehaviour
{
    public Text difficultyUpText;

    void Awake()
    {
        if (difficultyUpText != null)
            difficultyUpText.gameObject.SetActive(false);
    }

    public void ShowLevelUp(string message)
    {
        StartCoroutine(ShowRoutine(message));
    }

    IEnumerator ShowRoutine(string message)
    {
        difficultyUpText.gameObject.SetActive(true);
        difficultyUpText.text = message;

        yield return new WaitForSeconds(1.5f);

        difficultyUpText.gameObject.SetActive(false);
    }
}
