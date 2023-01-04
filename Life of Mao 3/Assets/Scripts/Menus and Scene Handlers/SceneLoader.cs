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
    public GameObject howToPlayScreen;
    public GameObject creditsScreen;
    public Image barFill;
    public Text progressText;
    public Image characterImg;
    public Text loadingScreenTip;
    
    public Sprite[] posters;
    public List<string> tips;

    private void Start()
    {
        tips.Add("Keep your guns loaded.");
        tips.Add("Always look your back, there can always be a zombie behind you.");
        tips.Add("Aim to the head, you'll save lots of ammo.");
        tips.Add("Always keep an eye on your stats, you could be starving without even realizing it.");
    }

    /// <summary>
    ///     Loads main menu.
    /// </summary>
    public static void MainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public static void Quit()
    {
        Application.Quit();
    }

    public void HowToPlay()
    {
        howToPlayScreen.SetActive(true);
    }

    public void Credits()
    {
        creditsScreen.SetActive(true);
        StartCoroutine(BackToMenuFromCredits());
    }

    public void BackToMenuFromInstructions()
    {
        howToPlayScreen.SetActive(false);
    }

    public IEnumerator BackToMenuFromCredits()
    {
        yield return new WaitForSeconds(40f);
        creditsScreen.SetActive(false);
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
        int characterIndex = GameObject.FindGameObjectWithTag("Settings").GetComponent<SettingsController>().selectedCharacter;
        characterImg.sprite = posters[characterIndex];
        
        loadingScreen.SetActive(true);
        int pickedTip = Random.Range(0, tips.Count);
        loadingScreenTip.text = tips[pickedTip];

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
