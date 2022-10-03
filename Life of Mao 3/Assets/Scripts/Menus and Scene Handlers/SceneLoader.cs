using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Image barFill;
    public Text progressText;

    public void StartGame()
    {
        StartCoroutine(LoadSceneAsync());
        //SceneManager.LoadScene(2);
        //Destroy(GameObject.Find("Music"), 0.5f);
    }

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
