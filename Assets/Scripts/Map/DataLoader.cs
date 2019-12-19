/// ========================================================
/// 描述：
/// 问题小记： 
/// 作者：HUI 
/// 创建时间：2019-12-12 01:08:54
/// 版 本：1.0
/// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour {

    public static DataLoader dataLoader;

    public static List<Player> MyData = new List<Player>();

    const string KEY_DATA = "DATA";

    const string KEY_FRISTTIME = "one";

    public const string KEY_mapPos = "mapPos";

    [SerializeField]
    private GameObject map;

    private GameObject tmp;

    [SerializeField]
    private GameObject mapParent;

    [SerializeField]
    private List<GameObject> listMap1;

    [SerializeField]
    private GameObject[] listMap;

    [SerializeField]
    private Vector2[] mapPos = new Vector2[297];

    [SerializeField]
    private UnityEngine.UI.Image processBar;

    [SerializeField]
    private Sprite[] mapSprite;

    private bool hold;

    public static bool enableClick;

    GameObject holdObj;

    const float STARMOVE_TIME = 1f;


    private void Awake()
    {
        dataLoader = this;
    }

    // Use this for initialization
    void Start () {
        listMap = new GameObject[297];
        if (PlayerPrefs.GetInt(KEY_FRISTTIME, 0) == 0) {
            PlayerPrefs.SetString(KEY_DATA, datadefaut);
            PlayerPrefs.SetInt(KEY_FRISTTIME, 1);
        }

        StartCoroutine(MapLoad());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator MapLoad() {
        DataLoader.enableClick = false;
        MapPosD();
        processBar.fillAmount = 0.3f;
        yield return new WaitForSeconds(0.3f);

        PlayerUtils playerUtils = new PlayerUtils();
        MyData = playerUtils.Load();
        processBar.fillAmount = 0.5f;

        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < 99; i++)
        {
            processBar.fillAmount += 0.0016835016835017f * 3;
            InsMap(mapPos[i], i);
            InsMap(mapPos[i + 99], i + 99);
            InsMap(mapPos[i + 198], i + 198);

            yield return null;
        }

        processBar.transform.parent.gameObject.SetActive(false);
        DataLoader.enableClick = true;

        if (CameraMovement.startPointMoveIndex != -1 && CameraMovement.startPointMoveIndex != 297)
        {
            StartPointMove();
            yield return new WaitForSeconds(STARMOVE_TIME);
            CameraMovement.mcamera.PopUpShow(MyData[CameraMovement.startPointMoveIndex]);

            PlayerPrefs.SetFloat("LASTPOSY", listMap[CameraMovement.startPointMoveIndex].transform.position.y);
            PlayerPrefs.SetFloat("LASTPOSX", listMap[CameraMovement.startPointMoveIndex].transform.position.x);
        }
        else
        {
            CameraMovement.mcamera.starPoint.transform.GetChild(0).GetComponent<Animation>().Play("StarPoint");
        }
    }

    void StartPointMove()
    {
        DataLoader.enableClick = false;
        Vector3 newPos = listMap[CameraMovement.startPointMoveIndex].transform.position + new Vector3(0, 0 - 0.3f);

        StartCoroutine(StopAnimation());
    }

    IEnumerator StopAnimation()
    {
        Transform child = CameraMovement.mcamera.starPoint.transform.GetChild(0);
        Debug.Log("StopAnim" + child.position);
        CameraMovement.mcamera.starPoint.transform.GetChild(0).transform.localPosition = new Vector3(0, 0.619f, 0);
        yield return new WaitForSeconds(STARMOVE_TIME);
        CameraMovement.mcamera.starPoint.transform.GetChild(0).GetComponent<Animation>().Play("StarPoint");
    }


    void InsMap(Vector3 pos, int index)
    {
        tmp = Instantiate(map) as GameObject;
        tmp.transform.position = new Vector3(pos.x, pos.y);
        tmp.transform.SetParent(mapParent.transform, false);
        listMap[index] = tmp;
        tmp.transform.GetChild(1).GetComponent<TextMesh>().text = (index + 1).ToString();
        tmp.name = (index + 1).ToString();
        Map mp = tmp.GetComponent<Map>();
        mp.Player = MyData[index];
        mp.SetMapInfo();
    }

    public Sprite[] MapSprite
    {
        get
        {
            return mapSprite;
        }

        set
        {
            mapSprite = value;
        }
    }

    #region
    string datadefaut = "False,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,1,True,0,0,1,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,3,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,1,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,0,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,True,0,0,2,";

    /// <summary>
    /// position of buttons level is fixed
    /// </summary>
    void MapPosD()
    {

        mapPos[0] = new Vector2(-0.004228723f, -3.587f);

        mapPos[1] = new Vector2(0.695f, -3.232f);

        mapPos[2] = new Vector2(-0.09021276f, -3.03f);

        mapPos[3] = new Vector2(-0.925f, -2.859967f);

        mapPos[4] = new Vector2(-0.188883f, -2.489f);

        mapPos[5] = new Vector2(0.6173936f, -2.367f);

        mapPos[6] = new Vector2(1.342f, -2f);

        mapPos[7] = new Vector2(0.5240414f, -1.590361f);

        mapPos[8] = new Vector2(-0.2140727f, -1.613735f);

        mapPos[9] = new Vector2(-1.06f, -1.636f);

        mapPos[10] = new Vector2(-1.579f, -1.265327f);

        mapPos[11] = new Vector2(-1.274f, -0.7695142f);

        mapPos[12] = new Vector2(-0.8071734f, 0.4705882f);

        mapPos[13] = new Vector2(-0.9679077f, 1f);

        mapPos[14] = new Vector2(-1.731092f, 1.497479f);

        mapPos[15] = new Vector2(-0.9021276f, 1.77f);

        mapPos[16] = new Vector2(-0.05638298f, 1.736f);

        mapPos[17] = new Vector2(0.7893617f, 1.752f);

        mapPos[18] = new Vector2(1.557f, 1.98f);

        mapPos[19] = new Vector2(1.307f, 2.653f);

        mapPos[20] = new Vector2(0.439f, 2.86f);

        mapPos[21] = new Vector2(-0.271578f, 2.98f);

        mapPos[22] = new Vector2(-0.9669681f, 3.106f);

        mapPos[23] = new Vector2(-1.39f, 3.66f);

        mapPos[24] = new Vector2(-0.8239896f, 4.221f);

        mapPos[25] = new Vector2(-1f, 4.758f);

        mapPos[26] = new Vector2(-1.562f, 5.279f);

        mapPos[27] = new Vector2(-0.996f, 5.581f);

        mapPos[28] = new Vector2(-0.2349291f, 5.59f);

        mapPos[29] = new Vector2(0.4792553f, 5.6f);

        mapPos[30] = new Vector2(1.184042f, 5.72f);

        mapPos[31] = new Vector2(1.631163f, 6.270145f);

        mapPos[32] = new Vector2(1.090071f, 6.87f);

        mapPos[33] = new Vector2(0.3523936f, 7.128f);

        mapPos[34] = new Vector2(-0.3636702f, 7.196f);

        mapPos[35] = new Vector2(-1.067f, 7.494f);

        mapPos[36] = new Vector2(-0.963f, 8.204f);

        mapPos[37] = new Vector2(-0.601f, 8.72f);

        mapPos[38] = new Vector2(0.017f, 9.103f);

        mapPos[39] = new Vector2(0.5887378f, 9.264387f);

        mapPos[40] = new Vector2(1.207f, 9.45f);

        mapPos[41] = new Vector2(1.583f, 9.921f);

        mapPos[42] = new Vector2(0.9122202f, 10.28887f);

        mapPos[43] = new Vector2(0.3170127f, 10.39903f);

        mapPos[44] = new Vector2(-0.3274053f, 10.45336f);

        mapPos[45] = new Vector2(-0.9444384f, 10.69456f);

        mapPos[46] = new Vector2(-1.259f, 11.225f);

        mapPos[47] = new Vector2(-1.013f, 11.708f);

        mapPos[48] = new Vector2(-0.62f, 12.499f);

        mapPos[49] = new Vector2(-0.173f, 12.927f);

        mapPos[50] = new Vector2(0.2264377f, 13.25734f);

        mapPos[51] = new Vector2(0.6922522f, 13.51896f);

        mapPos[52] = new Vector2(1.184f, 13.786f);

        mapPos[53] = new Vector2(1.528f, 14.277f);

        mapPos[54] = new Vector2(1.381f, 14.842f);

        mapPos[55] = new Vector2(0.818f, 15.127f);

        mapPos[56] = new Vector2(0.2264377f, 15.20605f);

        mapPos[57] = new Vector2(-0.329952f, 15.26113f);

        mapPos[58] = new Vector2(-0.9251596f, 15.40536f);

        mapPos[59] = new Vector2(-1.41f, 15.694f);

        mapPos[60] = new Vector2(-1.674f, 16.34f);

        mapPos[61] = new Vector2(-1.663f, 16.935f);

        mapPos[62] = new Vector2(-1.254f, 17.392f);

        mapPos[63] = new Vector2(-0.615f, 17.391f);

        mapPos[64] = new Vector2(0.03234824f, 17.3409f);

        mapPos[65] = new Vector2(0.7051916f, 17.40975f);

        mapPos[66] = new Vector2(1.337f, 17.79f);

        mapPos[67] = new Vector2(1.428f, 18.401f);

        mapPos[68] = new Vector2(1.139f, 18.952f);

        mapPos[69] = new Vector2(0.698f, 19.339f);

        mapPos[70] = new Vector2(0.218f, 19.65f);

        mapPos[71] = new Vector2(-0.3040734f, 19.95054f);

        mapPos[72] = new Vector2(-0.595f, 20.795f);

        mapPos[73] = new Vector2(-1.171f, 21.165f);

        mapPos[74] = new Vector2(-1.759f, 21.565f);

        mapPos[75] = new Vector2(-0.848f, 21.87f);

        mapPos[76] = new Vector2(-0.1488019f, 21.88109f);

        mapPos[77] = new Vector2(0.6146165f, 21.9224f);

        mapPos[78] = new Vector2(1.257f, 22.087f);

        mapPos[79] = new Vector2(1.636f, 22.677f);

        mapPos[80] = new Vector2(1.002796f, 23.1341f);

        mapPos[81] = new Vector2(0.2911341f, 23.24426f);

        mapPos[82] = new Vector2(-0.4205271f, 23.28556f);

        mapPos[83] = new Vector2(-1.067491f, 23.36818f);

        mapPos[84] = new Vector2(-1.568f, 23.924f);

        mapPos[85] = new Vector2(-0.769888f, 24.30333f);

        mapPos[86] = new Vector2(0.006469648f, 24.34464f);

        mapPos[87] = new Vector2(0.848f, 24.354f);

        mapPos[88] = new Vector2(1.634f, 24.472f);

        mapPos[89] = new Vector2(2.097f, 25.069f);

        mapPos[90] = new Vector2(1.583f, 25.679f);

        mapPos[91] = new Vector2(0.796f, 25.85f);

        mapPos[92] = new Vector2(-0.05822683f, 25.80419f);

        mapPos[93] = new Vector2(-0.8345845f, 25.68026f);

        mapPos[94] = new Vector2(-1.657f, 25.697f);

        mapPos[95] = new Vector2(-1.737f, 26.462f);

        mapPos[96] = new Vector2(-1.155f, 27.123f);

        mapPos[97] = new Vector2(-0.213f, 27.34f);

        mapPos[98] = new Vector2(0.429f, 27.742f);

        mapPos[99] = new Vector2(0.892f, 28.338f);

        mapPos[100] = new Vector2(0.986f, 29.143f);

        mapPos[101] = new Vector2(0.455f, 29.713f);

        mapPos[102] = new Vector2(-0.1876198f, 30.10146f);

        mapPos[103] = new Vector2(-0.8863418f, 30.37684f);

        mapPos[104] = new Vector2(-1.542f, 30.722f);

        mapPos[105] = new Vector2(-1.79f, 31.43f);

        mapPos[106] = new Vector2(-1.293f, 31.832f);

        mapPos[107] = new Vector2(-0.537f, 31.743f);

        mapPos[108] = new Vector2(0.493f, 31.705f);

        mapPos[109] = new Vector2(1.339217f, 31.8842f);

        mapPos[110] = new Vector2(2.045f, 32.401f);

        mapPos[111] = new Vector2(1.5f, 33.043f);

        mapPos[112] = new Vector2(0.7569487f, 33.12344f);

        mapPos[113] = new Vector2(-0.03234824f, 33.09591f);

        mapPos[114] = new Vector2(-0.7569487f, 33.09591f);

        mapPos[115] = new Vector2(-1.442731f, 33.26114f);

        mapPos[116] = new Vector2(-1.326278f, 34.01845f);

        mapPos[117] = new Vector2(-0.5887378f, 34.21122f);

        mapPos[118] = new Vector2(0.09704469f, 34.23876f);

        mapPos[119] = new Vector2(0.8216451f, 34.32138f);

        mapPos[120] = new Vector2(1.563f, 34.803f);

        mapPos[121] = new Vector2(1.054552f, 35.4792f);

        mapPos[122] = new Vector2(0.2523163f, 35.56182f);

        mapPos[123] = new Vector2(-0.55f, 35.563f);

        mapPos[124] = new Vector2(-1.371f, 35.665f);

        mapPos[125] = new Vector2(-1.954f, 36.421f);

        mapPos[126] = new Vector2(-1.927f, 37.132f);

        mapPos[127] = new Vector2(-1.365096f, 37.64099f);

        mapPos[128] = new Vector2(-0.5757986f, 37.66853f);

        mapPos[129] = new Vector2(0.2264377f, 37.29676f);

        mapPos[130] = new Vector2(1.05f, 37.046f);

        mapPos[131] = new Vector2(1.589f, 37.553f);

        mapPos[132] = new Vector2(1.451f, 38.275f);

        mapPos[133] = new Vector2(0.705f, 38.834f);

        mapPos[134] = new Vector2(0.03234824f, 39.11264f);

        mapPos[135] = new Vector2(-0.375f, 39.774f);

        mapPos[136] = new Vector2(0.2134984f, 40.287f);

        mapPos[137] = new Vector2(0.95f, 40.561f);

        mapPos[138] = new Vector2(1.352f, 41.178f);

        mapPos[139] = new Vector2(0.7957666f, 41.69f);

        mapPos[140] = new Vector2(-0.07116612f, 41.865f);

        mapPos[141] = new Vector2(-0.8345845f, 42.07f);

        mapPos[142] = new Vector2(-1.391f, 42.64414f);

        mapPos[143] = new Vector2(-0.8475238f, 43.2008f);

        mapPos[144] = new Vector2(0.05822683f, 43.40195f);

        mapPos[145] = new Vector2(0.874f, 43.678f);

        mapPos[146] = new Vector2(1.15f, 44.36f);

        mapPos[147] = new Vector2(0.54992f, 44.88013f);

        mapPos[148] = new Vector2(-0.2134984f, 44.918f);

        mapPos[149] = new Vector2(-0.9251596f, 45.02f);

        mapPos[150] = new Vector2(-1.631f, 45.40336f);

        mapPos[151] = new Vector2(-1.69f, 46.1f);

        mapPos[152] = new Vector2(-1.132188f, 46.68477f);

        mapPos[153] = new Vector2(-0.4205271f, 46.89131f);

        mapPos[154] = new Vector2(0.2781948f, 47.08408f);

        mapPos[155] = new Vector2(0.982f, 47.572f);

        mapPos[156] = new Vector2(1.212f, 48.299f);

        mapPos[157] = new Vector2(0.7828273f, 48.911f);

        mapPos[158] = new Vector2(0.109984f, 49.03011f);

        mapPos[159] = new Vector2(-0.6275558f, 49.04388f);

        mapPos[160] = new Vector2(-1.379f, 49.142f);

        mapPos[161] = new Vector2(-1.592f, 49.856f);

        mapPos[162] = new Vector2(-0.8863418f, 50.31066f);

        mapPos[163] = new Vector2(-0.1876198f, 50.442f);

        mapPos[164] = new Vector2(0.6146165f, 50.44f);

        mapPos[165] = new Vector2(1.264f, 50.744f);

        mapPos[166] = new Vector2(0.866f, 51.208f);

        mapPos[167] = new Vector2(0.2393769f, 51.482f);

        mapPos[168] = new Vector2(-0.382f, 52.011f);

        mapPos[169] = new Vector2(0.1229233f, 52.654f);

        mapPos[170] = new Vector2(0.848f, 52.865f);

        mapPos[171] = new Vector2(1.449f, 53.399f);

        mapPos[172] = new Vector2(0.9122202f, 53.93117f);

        mapPos[173] = new Vector2(0.1746805f, 54.11017f);

        mapPos[174] = new Vector2(-0.6016772f, 54.31671f);

        mapPos[175] = new Vector2(-1.346f, 54.684f);

        mapPos[176] = new Vector2(-1.632f, 55.268f);

        mapPos[177] = new Vector2(-0.731f, 55.509f);

        mapPos[178] = new Vector2(0.05822683f, 55.50087f);

        mapPos[179] = new Vector2(0.860463f, 55.45956f);

        mapPos[180] = new Vector2(1.39f, 56.074f);

        mapPos[181] = new Vector2(0.8345845f, 56.79091f);

        mapPos[182] = new Vector2(0.1229233f, 57.0663f);

        mapPos[183] = new Vector2(-0.6016772f, 57.159f);

        mapPos[184] = new Vector2(-1.291f, 57.354f);

        mapPos[185] = new Vector2(-1.478f, 57.954f);

        mapPos[186] = new Vector2(-1.041613f, 58.51208f);

        mapPos[187] = new Vector2(-0.3428913f, 58.7737f);

        mapPos[188] = new Vector2(0.4334664f, 58.7737f);

        mapPos[189] = new Vector2(1.095f, 58.771f);

        mapPos[190] = new Vector2(1.654f, 59.268f);

        mapPos[191] = new Vector2(1.698f, 60.006f);

        mapPos[192] = new Vector2(0.2523163f, 61.01823f);

        mapPos[193] = new Vector2(-0.4981629f, 61.01926f);

        mapPos[194] = new Vector2(-1.18f, 61.044f);

        mapPos[195] = new Vector2(-1.764f, 61.535f);

        mapPos[196] = new Vector2(-1.27452f, 61.99689f);

        mapPos[197] = new Vector2(-0.64f, 62.125f);

        mapPos[198] = new Vector2(0.07116612f, 62.09327f);

        mapPos[199] = new Vector2(0.8216451f, 62.07951f);

        mapPos[200] = new Vector2(1.577f, 62.14f);

        mapPos[201] = new Vector2(1.507428f, 62.802f);

        mapPos[202] = new Vector2(0.860463f, 63.042f);

        mapPos[203] = new Vector2(0.1876198f, 63.20859f);

        mapPos[204] = new Vector2(-0.554f, 63.559f);

        mapPos[205] = new Vector2(-0.679313f, 64.07061f);

        mapPos[206] = new Vector2(-0.006469648f, 64.26888f);

        mapPos[207] = new Vector2(0.679313f, 64.3515f);

        mapPos[208] = new Vector2(1.331f, 64.598f);

        mapPos[209] = new Vector2(1.733f, 65.177f);

        mapPos[210] = new Vector2(1.416f, 65.765f);

        mapPos[211] = new Vector2(0.7957666f, 66.01759f);

        mapPos[212] = new Vector2(0.1617411f, 66.18282f);

        mapPos[213] = new Vector2(-0.4852235f, 66.34806f);

        mapPos[214] = new Vector2(-1.118f, 66.678f);

        mapPos[215] = new Vector2(-1.479f, 67.221f);

        mapPos[216] = new Vector2(-1.387f, 67.987f);

        mapPos[217] = new Vector2(-1.241f, 68.757f);

        mapPos[218] = new Vector2(-0.8345845f, 69.316f);

        mapPos[219] = new Vector2(-0.1617411f, 69.34177f);

        mapPos[220] = new Vector2(0.4722843f, 69.35554f);

        mapPos[221] = new Vector2(1.09337f, 69.45193f);

        mapPos[222] = new Vector2(1.652f, 69.92f);

        mapPos[223] = new Vector2(1.46861f, 70.59f);

        mapPos[224] = new Vector2(0.8345845f, 70.81509f);

        mapPos[225] = new Vector2(0.1488019f, 70.98032f);

        mapPos[226] = new Vector2(-0.534f, 71.172f);

        mapPos[227] = new Vector2(-1.156f, 71.48979f);

        mapPos[228] = new Vector2(-1.551f, 72.155f);

        mapPos[229] = new Vector2(-1.028674f, 72.745f);

        mapPos[230] = new Vector2(-0.2781948f, 72.78366f);

        mapPos[231] = new Vector2(0.4464056f, 72.639f);

        mapPos[232] = new Vector2(1.113f, 72.598f);

        mapPos[233] = new Vector2(1.525f, 73.121f);

        mapPos[234] = new Vector2(1.506f, 73.837f);

        mapPos[235] = new Vector2(1.011f, 74.29829f);

        mapPos[236] = new Vector2(0.3946485f, 74.577f);

        mapPos[237] = new Vector2(-0.2264377f, 74.80776f);

        mapPos[238] = new Vector2(-0.861f, 75.08315f);

        mapPos[239] = new Vector2(-1.428f, 75.544f);

        mapPos[240] = new Vector2(-1.464f, 76.365f);

        mapPos[241] = new Vector2(-0.7569487f, 76.68608f);

        mapPos[242] = new Vector2(-0.07116612f, 76.72739f);

        mapPos[243] = new Vector2(0.6404951f, 76.762f);

        mapPos[244] = new Vector2(1.371f, 76.925f);

        mapPos[245] = new Vector2(1.544f, 77.566f);

        mapPos[246] = new Vector2(0.951038f, 78.024f);

        mapPos[247] = new Vector2(0.2781948f, 78.231f);

        mapPos[248] = new Vector2(-0.446f, 78.376f);

        mapPos[249] = new Vector2(-1.101f, 78.629f);

        mapPos[250] = new Vector2(-1.63f, 79.149f);

        mapPos[251] = new Vector2(-1.609f, 79.814f);

        mapPos[252] = new Vector2(-1.025f, 80.375f);

        mapPos[253] = new Vector2(-0.2393769f, 80.52411f);

        mapPos[254] = new Vector2(0.3817092f, 80.60673f);

        mapPos[255] = new Vector2(0.936f, 80.96473f);

        mapPos[256] = new Vector2(1.248642f, 81.57059f);

        mapPos[257] = new Vector2(1.171006f, 82.25905f);

        mapPos[258] = new Vector2(0.5369807f, 82.43806f);

        mapPos[259] = new Vector2(-0.1358626f, 82.43806f);

        mapPos[260] = new Vector2(-0.7569487f, 82.45182f);

        mapPos[261] = new Vector2(-1.329f, 82.676f);

        mapPos[262] = new Vector2(-1.651f, 83.315f);

        mapPos[263] = new Vector2(-1.723f, 83.929f);

        mapPos[264] = new Vector2(-1.331f, 84.601f);

        mapPos[265] = new Vector2(-0.7181308f, 84.98347f);

        mapPos[266] = new Vector2(-0.07116612f, 84.88708f);

        mapPos[267] = new Vector2(0.6146165f, 84.74938f);

        mapPos[268] = new Vector2(1.261581f, 84.74938f);

        mapPos[269] = new Vector2(1.38f, 85.375f);

        mapPos[270] = new Vector2(0.941f, 85.829f);

        mapPos[271] = new Vector2(0.2781948f, 86.16763f);

        mapPos[272] = new Vector2(-0.4075879f, 86.27779f);

        mapPos[273] = new Vector2(-1.052f, 86.4f);

        mapPos[274] = new Vector2(-1.624f, 86.823f);

        mapPos[275] = new Vector2(-1.746f, 87.543f);

        mapPos[276] = new Vector2(-1.217f, 87.962f);

        mapPos[277] = new Vector2(-0.413f, 87.571f);

        mapPos[278] = new Vector2(0.3170127f, 87.69275f);

        mapPos[279] = new Vector2(0.8087059f, 88.20222f);

        mapPos[280] = new Vector2(1.653f, 88.344f);

        mapPos[281] = new Vector2(1.891f, 88.914f);

        mapPos[282] = new Vector2(1.64976f, 89.57915f);

        mapPos[283] = new Vector2(1.080431f, 89.84077f);

        mapPos[284] = new Vector2(0.3946485f, 89.68931f);

        mapPos[285] = new Vector2(-0.200559f, 89.40015f);

        mapPos[286] = new Vector2(-0.8216451f, 89.20738f);

        mapPos[287] = new Vector2(-1.503f, 89.506f);

        mapPos[288] = new Vector2(-1.736f, 90.223f);

        mapPos[289] = new Vector2(-1.506f, 91.0685f);

        mapPos[290] = new Vector2(-0.7440094f, 91.38519f);

        mapPos[291] = new Vector2(0.122f, 91.242f);

        mapPos[292] = new Vector2(0.93f, 91.686f);

        mapPos[293] = new Vector2(1.492f, 92.37659f);

        mapPos[294] = new Vector2(0.769888f, 92.56936f);

        mapPos[295] = new Vector2(-0.03234824f, 92.5969f);

        mapPos[296] = new Vector2(-0.956f, 92.74f);

    }
    #endregion
}
