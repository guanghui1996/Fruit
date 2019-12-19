using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonActionController : MonoBehaviour {

    public static ButtonActionController Click;

    [SerializeField]
    private Sprite[] buttonSprites;

    public Sprite[] ButtonSprites
    {
        get
        {
            return buttonSprites;
        }

        set
        {
            buttonSprites = value;
        }
    }

    private void Awake()
    {
        if (Click == null)
        {
            DontDestroyOnLoad(gameObject);
            Click = this;
        }
        else if (Click != this) {
            Destroy(this);
        }
    }

    public void ArcadeScene(Player player)
    {
        SoundController.Sound.Click();
        Time.timeScale = 1;
        PlayerInfo.MODE = 1;
        PlayerInfo.MapPlayer = player;
        StartCoroutine(GotoScene("PlayScene"));
    }

    public void SelectMap(int mode) {
        SoundController.Sound.Click();
        if (mode == 1)
            SceneManager.LoadScene("MapScene");
        else
            SceneManager.LoadScene("HomeScene");
    }

    public void HomeScene() {
        SoundController.Sound.Click();
        Time.timeScale = 1;
        StartCoroutine(GotoScene("HomeScene"));
    }

    IEnumerator GotoScene(string sceneName)
    {
        yield return new WaitForSeconds(0);
        SceneManager.LoadScene(sceneName);
    }
}
