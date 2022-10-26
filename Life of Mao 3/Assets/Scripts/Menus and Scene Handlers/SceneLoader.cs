using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

/// <summary>
///     This script is in charge of loading the main game's scene.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Image barFill;
    public Text progressText;

    /// <summary>
    ///     Loads main menu.
    /// </summary>
    public static void MainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void StartGame()
    {
        StartCoroutine(LoadSceneAsync());
    }

    /// <summary>
    ///     This function loads the scene and shows the progress value at the loading bar and printing its text.
    /// </summary>
    IEnumerator LoadSceneAsync()
    {
        loadingScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(2);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            progressText.text = (int)(Mathf.Clamp01(operation.progress / 0.9f)*100) +"%";
            barFill.fillAmount = progressValue;

            Destroy(GameObject.Find("Music"), 0.5f);
            yield return null;
        }
    }
}
