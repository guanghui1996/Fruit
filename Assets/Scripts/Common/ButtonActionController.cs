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

    #region 声音按键
    public void BMusic(UnityEngine.UI.Button button)
    {
        if (PlayerPrefs.GetInt("MUSIC", 0) != 1)
        {
            PlayerPrefs.SetInt("MUSIC", 1);
        }
        else
        {
            PlayerPrefs.SetInt("MUSIC", 0);
        }
    }

    public void BSound(UnityEngine.UI.Button button)
    {
        if (PlayerPrefs.GetInt("SOUND", 0) != 1)
        {
            PlayerPrefs.SetInt("SOUND", 1);
        }
        else
        {
            PlayerPrefs.SetInt("SOUND", 0);
        }
    }
    #endregion

    #region 场景按键
    public void ClassicScene(int level)
    {
        SoundController.Sound.Click();
        Time.timeScale = 1;
        PlayerInfo.MODE = 0;
        PlayerInfo.MapPlayer = new Player();
        PlayerInfo.MapPlayer.Level = level;
        PlayerInfo.MapPlayer.HightScore = level;
        PlayerInfo.MapPlayer.HightScore = PlayerPrefs.GetInt(PlayerInfo.KEY_CLASSIC_HISCORE, 0);
        SceneManager.LoadScene("PlayScene");
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
    #endregion

    IEnumerator GotoScene(string sceneName)
    {
        yield return new WaitForSeconds(0);
        SceneManager.LoadScene(sceneName);
    }
}
