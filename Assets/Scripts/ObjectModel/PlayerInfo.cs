/// ========================================================
/// 描述：
/// 问题小记： 
/// 作者：HUI 
/// 创建时间：2019-12-11 22:11:51
/// 版 本：1.0
/// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour {

    public static PlayerInfo Info;

    public static Player MapPlayer;

    public static byte MODE;

    public static int BACKGROUND;

    [SerializeField]
    private int score;

    public const string KEY_CLASSIC_HISCORE = "classichightscore";

    [SerializeField]
    private TextMesh textMesh;

    private void Awake()
    {
        Info = this;
        BACKGROUND = MapPlayer.Background;
    }

    // Use this for initialization
    void Start () {
        Score = 0;

        textMesh.text = MapPlayer.Level.ToString();
	}

    public int Score
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
}
