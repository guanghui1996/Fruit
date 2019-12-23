/// ========================================================
/// 描述：
/// 问题小记： 
/// 作者：HUI 
/// 创建时间：2019-12-11 22:27:48
/// 版 本：1.0
/// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    #region 字段
    public static Timer timer;

    [SerializeField]
    private UnityEngine.UI.Image timeBar;

    [SerializeField]
    private Texture2D timebarTexture;

    [SerializeField]
    private GameObject noSelect;

    [SerializeField]
    private GameObject pauseUI;

    [SerializeField]
    private GameObject winUI;

    [SerializeField]
    private GameObject loseUI;

    [SerializeField]
    private GameObject noMove;

    [SerializeField]
    private float gameTime = 270;

    private bool inUpdate = true;

    private float m_time;

    private const int classicBaseScore = 5000;

    private int classicTargetScore;

    private int scoreStack = 0;

    private bool startPlus;

    private bool isAds;

    private bool isreq;


    public enum GameState
    {
        PLAYING = 0,
        PAUSE = 1,
        WIN = 2,
        LOST = 3
    }
    #endregion

    private void Awake()
    {
        timer = this;
    }

    // Use this for initialization
    void Start () {
        m_time = GameTime;
        TimeBar.fillAmount = 0;
        StartCoroutine(AdsCd());
	}
	
	// Update is called once per frame
	void Update () {
        if (inUpdate) {
            Tick();
        }
	}

    public void TimeTick(bool b)
    {
        if (b && PlayerInfo.MODE == 1)
        {
            inUpdate = true;
        }
        else
        {
            inUpdate = false;
        }
            
    }

    public void Tick() {
        if (GameTime > 0 && GameController.gameController.GameState == (int)GameState.PLAYING)
        {
            GameTime -= Time.deltaTime;
            TimeBarProcess(GameTime);
        }
        else if (GameController.gameController.GameState == (int)GameState.PLAYING)
        {
            GameController.gameController.GameState = (int)GameState.LOST;
            GameTime = 0;
            Lost();
            inUpdate = false;
        }
    }

    /// <summary>
    /// 时间剩余进度
    /// </summary>
    /// <param name="time"></param>
    void TimeBarProcess(float time)
    {
        float fillAmount = time / m_time;
        TimeBar.fillAmount = fillAmount;
    }

    /// <summary>
    /// 分数进度条
    /// </summary>
    /// <param name="score"></param>
    void ScoreBarProcess(int score)
    {
        ScoreStack += score;
        if (!startPlus)
        {
            StartPlus = true;
            StartCoroutine(IEScoreBarProcess());
        }
    }

    #region 按键
    public void Home()
    {
        ButtonActionController.Click.SelectMap(PlayerInfo.MODE);
    }

    public void Next()
    {
        ButtonActionController.Click.SelectMap(1);
        if (PlayerInfo.MapPlayer.Level < 297)
            CameraMovement.startPointMoveIndex = PlayerInfo.MapPlayer.Level;
        else
            CameraMovement.startPointMoveIndex = -1;
    }

    public void Music(UnityEngine.UI.Button button)
    {
        ButtonActionController.Click.BMusic(button);
    }

    public void Sound(UnityEngine.UI.Button button)
    {
        ButtonActionController.Click.BSound(button);
    }

    public void RePlay()
    {
        if (PlayerInfo.MODE == 1)
        {
            PlayerInfo.Info.Score = 0;
            ButtonActionController.Click.ArcadeScene(PlayerInfo.MapPlayer);
        }
        else
        {
            ButtonActionController.Click.ClassicScene(1);
        }
    }

    #endregion

    #region 失败  胜利  暂停  恢复
    public void Lost()
    {
        GameController.gameController.GameState = (int)GameState.LOST;
        NoSelect.SetActive(true);

        StartCoroutine(DisableAll());
        SoundController.Sound.Lose();
        showFullAds();
        Debug.Log("LOSE");
    }

    public void Win()
    {
        GameController.gameController.GameState = (int)GameState.WIN;
        NoSelect.SetActive(true);
        StartCoroutine(IEWin());
        Debug.Log("WIN");
    }

    public void Pause()
    {
        SoundController.Sound.Click();
        if (GameController.gameController.GameState == (int)GameState.PLAYING)
        {
            GameController.gameController.GameState = (int)GameState.PAUSE;
            NoSelect.SetActive(true);
            PauseUI.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void Resume()
    {
        SoundController.Sound.Click();
        if (GameController.gameController.GameState == (int)GameState.PAUSE)
        {
            GameController.gameController.GameState = (int)GameState.PLAYING;
            NoSelect.SetActive(false);
            PauseUI.SetActive(false);
            Time.timeScale = 1;
        }
    }

    #endregion

    public void DisableFruits(bool b)
    {
        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                if (!b)
                {
                    if (FruitSpawner.spawn.FruitGridScript[x, y] != null)
                        FruitSpawner.spawn.FruitGridScript[x, y].FruitDisable();
                }
                else
                {
                    if (FruitSpawner.spawn.FruitGridScript[x, y] != null && FruitSpawner.spawn.FruitGridScript[x, y] != GameController.gameController.FruitStar)
                        FruitSpawner.spawn.FruitGridScript[x, y].FruitDisable();
                }
            }
        }
    }

    void showFullAds()
    {
        if (IsAds)
        {
            GoogleMobileAdsScript.advertise.ShowInterstitial();
            IsAds = false;
            Isreq = false;
        }
    }

    #region 协程
    /// <summary>
    /// 销毁场景
    /// </summary>
    /// <returns></returns>
    IEnumerator DisableAll()
    {
        DisableFruits(true);
        yield return new WaitForSeconds(1f);
        LoseUI.SetActive(true);
    }

    /// <summary>
    /// 显示胜利界面
    /// </summary>
    /// <returns></returns>
    IEnumerator IEWin()
    {
        DisableFruits(true);

        SoundController.Sound.Win();
        GameController.gameController.FruitStar.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        WinUI.SetActive(true);
        showFullAds();
    }

    /// <summary>
    /// 更新玩家分数值
    /// </summary>
    /// <returns></returns>
    IEnumerator IEScoreBarProcess()
    {
        while (ScoreStack > 0 && GameController.gameController.GameState == (int)GameState.PLAYING)
        {
            ScoreStack -= 10;
            if (PlayerInfo.Info.Score + 10 < 5000 * PlayerInfo.MapPlayer.Level)
            {
                PlayerInfo.Info.Score += 10;
            }
            else
            {
                PlayerInfo.Info.Score = 5000 * PlayerInfo.MapPlayer.Level;
                break;
            }
            float fillAmount = PlayerInfo.Info.Score / (5000 * PlayerInfo.MapPlayer.Level);
            TimeBar.fillAmount = fillAmount;
            yield return null;
        }
        StartPlus = false;
    }

    IEnumerator AdsCd()
    {
        while (true)
        {
            yield return new WaitForSeconds(119f);
            IsAds = true;
        }
    }

    #endregion

    #region 属性
    public bool Isreq
    {
        get
        {
            return isreq;
        }

        set
        {
            isreq = value;
        }
    }

    public float GameTime
    {
        get
        {
            return gameTime;
        }

        set
        {
            gameTime = value;
        }
    }

    public GameObject NoSelect
    {
        get
        {
            return noSelect;
        }

        set
        {
            noSelect = value;
        }
    }

    public Image TimeBar
    {
        get
        {
            return timeBar;
        }

        set
        {
            timeBar = value;
        }
    }

    public GameObject PauseUI
    {
        get
        {
            return pauseUI;
        }

        set
        {
            pauseUI = value;
        }
    }

    public GameObject WinUI
    {
        get
        {
            return winUI;
        }

        set
        {
            winUI = value;
        }
    }

    public GameObject LoseUI
    {
        get
        {
            return loseUI;
        }

        set
        {
            loseUI = value;
        }
    }

    public GameObject NoMove
    {
        get
        {
            return noMove;
        }

        set
        {
            noMove = value;
        }
    }

    public static int ClassicBaseScore
    {
        get
        {
            return classicBaseScore;
        }
    }

    public int ClassicTargetScore
    {
        get
        {
            return classicTargetScore;
        }

        set
        {
            classicTargetScore = value;
        }
    }

    public int ScoreStack
    {
        get
        {
            return scoreStack;
        }

        set
        {
            scoreStack = value;
        }
    }

    public bool IsAds
    {
        get
        {
            return isAds;
        }

        set
        {
            isAds = value;
        }
    }

    public bool StartPlus
    {
        get
        {
            return startPlus;
        }

        set
        {
            startPlus = value;
        }
    }
    #endregion
}
