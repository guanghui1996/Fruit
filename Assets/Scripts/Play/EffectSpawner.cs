/// ========================================================
/// 描述：
/// 问题小记： 
/// 作者：HUI 
/// 创建时间：2019-12-29 15:41:09
/// 版 本：1.0
/// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectSpawner : MonoBehaviour {

    public static EffectSpawner effect;

    [SerializeField]
    private GameObject parent;

    [SerializeField]
    private GameObject[] effectPrefabs;

    [SerializeField]
    private Animator redGlass;

    [SerializeField]
    private GameObject[,] fruitCrashArray;

    [SerializeField]
    private GameObject fruitCrashParent;

    [SerializeField]
    private Text level;

    [SerializeField]
    private Text best;

    [SerializeField]
    private Text score;

    /// <summary>
    /// 能量条
    /// </summary>
    [SerializeField]
    private Image energy;

    public float REFRESH_COMBO_TIME = 2f;

    [SerializeField]
    private float comboCountdown;


    /// <summary>
    /// 爆炸
    /// </summary>
    private const float BOOM_TIME = 0.5f;

    /// <summary>
    /// 冰冻
    /// </summary>
    private const float ICECRASH_TIME = 0.5f;

    /// <summary>
    /// 水果
    /// </summary>
    private const float FRUITCRASH_TIME = 0.35f;

    /// <summary>
    /// 分数显示
    /// </summary>
    private const float SCORESHOW_TIME = 0.5f;

    /// <summary>
    /// 雷
    /// </summary>
    private const float THUNDER_TIME = 0.4f;

    /// <summary>
    /// 箭头
    /// </summary>
    private const float FILEARROW_TIME = 0.4f;

    private int ComboCount = 0;

    private int ThunderCount = 0;

    private int PowerCount = 0;

    private float EnergyStack = 0;

    private bool isEnergyInc;

    private void Awake()
    {
        effect = this;
        FruitCrashArray = new GameObject[7, 9];
    }

    public void ContinueCombo()
    {
        ComboCountdown = REFRESH_COMBO_TIME;
    }

    public void ComboInc()
    {
        ComboCount++;
    }

    /// <summary>
    /// 为水果添加消除的动画效果
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public GameObject FruitCrash(Vector3 pos)
    {
        GameObject tmp = Instantiate(EffectPrefabs[0]) as GameObject;
        tmp.transform.SetParent(FruitCrashParent.transform, false);
        tmp.transform.localPosition = new Vector3(pos.x, pos.y, -0.2f);
        return tmp;
    }

    /// <summary>
    /// 分数自增
    /// </summary>
    /// <param name="pos"></param>
    public void ScoreInc(Vector3 pos)
    {
        int scoreBonus = 10 + ComboCount * 10;
        if (PlayerInfo.MODE == 1)
        {
            if (PlayerInfo.Info.Score < PlayerInfo.MapPlayer.Level * 5000)
            {
                Timer.timer.ScoreBarProcess(scoreBonus);
            }
            else if (GameController.gameController.GameState == (int)Timer.GameState.PLAYING)
            {
                Timer.timer.ClassicLvUp();
            }
        }
        else
        {
            if (GameController.gameController.GameState == (int)Timer.GameState.PLAYING)
            {
                PlayerInfo.Info.Score += scoreBonus;
            }
            BonusEffect();
            MiniStar(pos);
        }
        ScoreEff(scoreBonus, pos);
        SetScore(PlayerInfo.Info.Score);
    }

    /// <summary>
    /// 增加能量
    /// </summary>
    private void EnergyInc()
    {
        if (!isEnergyInc)
        {
            StartCoroutine(IEEnergyInc());
        }
    }

    /// <summary>
    /// 奖励效果
    /// </summary>
    private void BonusEffect()
    {
        ThunderCount++;
        PowerCount++;
        EnergyStack += 1 / 21f;
        EnergyInc();
        if (ThunderCount >= 21)
        {
            GameController.gameController.DestroyRandom();
            ThunderCount = 0;
            Energy.fillAmount = 0;
            EnergyStack = 0;
        }
        if (PowerCount >= 32)
        {
            PowerCount = 0;
            GameController.gameController.IsAddPower = true;
        }
    }

    /// <summary>
    /// 闪电特效
    /// </summary>
    /// <param name="pos"></param>
    public void Thunder(Vector3 pos)
    {
        MGE(Energy.transform.position, pos, -0.4f);
    }

    /// <summary>
    /// 爆炸特效
    /// </summary>
    /// <param name="pos"></param>
    public void Boom(Vector3 pos)
    {
        GameObject tmp = Instantiate(EffectPrefabs[1]) as GameObject;
        SoundController.Sound.Boom();
        tmp.transform.SetParent(parent.transform, false);
        tmp.transform.position = pos;
        Destroy(tmp, BOOM_TIME);
    }

    /// <summary>
    /// 附魔
    /// </summary>
    /// <param name="obj"></param>
    public void Enchant(GameObject obj)
    {
        GameObject tmp = Instantiate(EffectPrefabs[2]) as GameObject;
        tmp.transform.SetParent(obj.transform, false);
    }

    /// <summary>
    /// 行消灭特效
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="power"></param>
    public void ThunderRow(GameObject obj, int power)
    {
        GameObject tmp = Instantiate(EffectPrefabs[5]) as GameObject;
        tmp.transform.SetParent(obj.transform.GetChild(0).transform, false);
        if (power == 3)
            tmp.transform.localEulerAngles = new Vector3(0, 0, 90);
    }

    /// <summary>
    /// 火箭特效
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="c"></param>
    public void FireArrow(Vector3 pos, bool c)
    {
        GameObject tmp = (GameObject)Instantiate(EffectPrefabs[6]);
        tmp.transform.SetParent(parent.transform, false);
        tmp.transform.position = new Vector3(pos.x, pos.y, -2.2f);
        if (c)
            tmp.transform.localEulerAngles = new Vector3(0, 0, 90);
        Destroy(tmp, FILEARROW_TIME);
    }

    /// <summary>
    /// 时钟特效
    /// </summary>
    /// <param name="obj"></param>
    public void Clock(GameObject obj)
    {
        GameObject tmp = (GameObject)Instantiate(EffectPrefabs[7]);
        tmp.transform.SetParent(obj.transform.GetChild(0).transform, false);
    }

    /// <summary>
    /// 星星 胜利特效
    /// </summary>
    /// <param name="pos"></param>
    public void StarWinEffect(Vector3 pos)
    {
        GameObject tmp = (GameObject)Instantiate(EffectPrefabs[8]);
        tmp.transform.SetParent(parent.transform, false);
        tmp.transform.position = new Vector3(pos.x, pos.y, tmp.transform.position.z);
        Animation anim = tmp.GetComponent<Animation>();
        StarEffectAnim(anim, tmp);
        Destroy(tmp, 1f);

    }

    /// <summary>
    /// 冰冻特效
    /// </summary>
    /// <param name="pos"></param>
    public void IceCrash(Vector2 pos)
    {

        GameObject tmp = (GameObject)Instantiate(EffectPrefabs[9]);
        tmp.transform.SetParent(parent.transform, false);
        tmp.transform.position = GridManager.grid.GridCell[(int)pos.x, (int)pos.y].transform.position;
        Destroy(tmp, ICECRASH_TIME);

    }

    /// <summary>
    /// 锁死特效
    /// </summary>
    /// <param name="pos"></param>
    public void LockCrash(Vector2 pos)
    {

        GameObject tmp = (GameObject)Instantiate(EffectPrefabs[10]);
        tmp.transform.SetParent(parent.transform, false);
        tmp.transform.position = GridManager.grid.GridCell[(int)pos.x, (int)pos.y].transform.position;
        Destroy(tmp, ICECRASH_TIME);

    }


    public GameObject MGE(Vector3 pos, Vector3 target, float z)
    {
        GameObject tmp = MGE(pos, target);
        tmp.transform.position += new Vector3(pos.x, pos.y, z);
        return tmp;
    }

    public GameObject MGE(Vector3 pos, Vector3 target)
    {
        GameObject tmp = Instantiate(EffectPrefabs[11]);
        tmp.transform.SetParent(parent.transform, false);
        tmp.transform.position = new Vector3(pos.x, pos.y, -0.22f);

        float AngleRad = Mathf.Atan2(target.y - pos.y, target.x - pos.x);

        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        tmp.transform.rotation = Quaternion.Euler(0, 0, AngleDeg);

        Ulti.MoveTo(tmp, target, 0.4f);
        Destroy(tmp, 0.4f);

        SoundController.Sound.Gun();
        return tmp;
    }

    /// <summary>
    /// 能量球的红玻璃展示
    /// </summary>
    public void Glass()
    {
        if (PlayerInfo.MODE == 1)
            redGlass.enabled = true;
    }

    /// <summary>
    /// 小星星
    /// </summary>
    /// <param name="starPos"></param>
    public void MiniStar(Vector3 starPos)
    {
        GameObject tmp = Instantiate(EffectPrefabs[12]) as GameObject;
        tmp.transform.SetParent(parent.transform, false);
        Ulti.MoveTo(tmp, starPos, new Vector2(-2.5f, 4.4f), 1.2f, -2.2f);
        Destroy(tmp, 1.2f);
    }

    /// <summary>
    /// 分数效果
    /// </summary>
    /// <param name="score"></param>
    /// <param name="pos"></param>
    private void ScoreEff(int score, Vector3 pos)
    {
        GameObject tmp = Instantiate(EffectPrefabs[4]) as GameObject;
        tmp.transform.GetChild(0).GetComponent<TextMesh>().text = score.ToString();
        tmp.transform.SetParent(Parent.transform, false);
        tmp.transform.position = new Vector3(pos.x, pos.y, tmp.transform.position.z);
        Destroy(tmp, SCORESHOW_TIME);
    }

    /// <summary>
    /// 星星动画
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="tmp"></param>
    void StarEffectAnim(Animation anim, GameObject tmp)
    {
        //Debug.Break();
        anim.enabled = true;
        AnimationClip animclip = new AnimationClip();
#if UNITY_5
        animclip.legacy = true;
#endif
        AnimationCurve curveScalex = AnimationCurve.Linear(0, tmp.transform.localScale.x, 1, 3);
        //AnimationCurve curveScaley = AnimationCurve.Linear(0, tmp.transform.localScale.y, 1, 3);
        AnimationCurve curvex = AnimationCurve.Linear(0, tmp.transform.position.x, 1, 0);
        AnimationCurve curvey = AnimationCurve.Linear(0, tmp.transform.position.y, 1, 0);
        AnimationCurve curvez = AnimationCurve.Linear(0, tmp.transform.position.z, 1, tmp.transform.position.z);
        AnimationCurve curveColora = AnimationCurve.Linear(0, 1, 1, 0);

        animclip.SetCurve("", typeof(Transform), "m_LocalScale.x", curveScalex);
        animclip.SetCurve("", typeof(Transform), "m_LocalScale.y", curveScalex);
        animclip.SetCurve("", typeof(Transform), "localPosition.x", curvex);
        animclip.SetCurve("", typeof(Transform), "localPosition.y", curvey);
        animclip.SetCurve("", typeof(Transform), "localPosition.z", curvez);
        animclip.SetCurve(tmp.transform.GetChild(0).name, typeof(SpriteRenderer), "m_Color.a", curveColora);
        // animclip.SetCurve("", typeof(Animation), "m_Enabled", curvenable);
        anim.wrapMode = WrapMode.Once;
        anim.AddClip(animclip, "Startwin");
        anim.Play("Startwin");
    }

    /// <summary>
    /// 设置等级
    /// </summary>
    /// <param name="lv"></param>
    public void SetLevel(int lv)
    {
        level.text = lv.ToString();
    }

    /// <summary>
    /// 设置分数
    /// </summary>
    /// <param name="_score"></param>
    public void SetScore(int _score)
    {
        Score.text = _score.ToString();
    }

    /// <summary>
    /// 设置最佳
    /// </summary>
    /// <param name="bestScore"></param>
    public void SetBest(int bestScore)
    {
        best.text = bestScore.ToString();
    }

    /// <summary>
    /// 增加能量效果展示
    /// </summary>
    /// <returns></returns>
    IEnumerator IEEnergyInc()
    {
        isEnergyInc = true;
        float d = 1 / 210f;
        while (EnergyStack > 0)
        {
            Energy.fillAmount += d;
            EnergyStack -= d;
            yield return null;
            if (Energy.fillAmount == 1)
                Energy.fillAmount = 0;
        }
        EnergyStack = 0;
        isEnergyInc = false;
    }

    /// <summary>
    /// 连击标记
    /// </summary>
    /// <returns></returns>
    public IEnumerator ComboTick()
    {
        while (true)
        {
            if (ComboCountdown > 0)
                ComboCountdown -= Time.deltaTime;
            else
                ComboCount = 0;
            yield return null;
        }
    }
   

    #region 属性
    public GameObject Parent
    {
        get
        {
            return parent;
        }

        set
        {
            parent = value;
        }
    }

    public GameObject[] EffectPrefabs
    {
        get
        {
            return effectPrefabs;
        }

        set
        {
            effectPrefabs = value;
        }
    }

    public Animator RedGlass
    {
        get
        {
            return redGlass;
        }

        set
        {
            redGlass = value;
        }
    }

    public GameObject[,] FruitCrashArray
    {
        get
        {
            return fruitCrashArray;
        }

        set
        {
            fruitCrashArray = value;
        }
    }

    public GameObject FruitCrashParent
    {
        get
        {
            return fruitCrashParent;
        }

        set
        {
            fruitCrashParent = value;
        }
    }

    public Text Level
    {
        get
        {
            return level;
        }

        set
        {
            level = value;
        }
    }

    public Text Best
    {
        get
        {
            return best;
        }

        set
        {
            best = value;
        }
    }

    public Text Score
    {
        get
        {
            return score;
        }

        set
        {
            score = value;
        }
    }

    public Image Energy
    {
        get
        {
            return energy;
        }

        set
        {
            energy = value;
        }
    }

    public float ComboCountdown
    {
        get
        {
            return comboCountdown;
        }

        set
        {
            comboCountdown = value;
        }
    }
    #endregion
}
