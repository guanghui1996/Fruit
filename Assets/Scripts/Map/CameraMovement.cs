/// ========================================================
/// 描述：
/// 问题小记： 
/// 作者：HUI 
/// 创建时间：2019-12-11 01:15:27
/// 版 本：1.0
/// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour {

    public static CameraMovement mcamera;

    public static int startPointMoveIndex;

    public RectTransform container;

    public GameObject PopUp;

    public GameObject starPoint;

    public GameObject fade;

    public Sprite[] star;

    public bool isPopUp;

    public static bool setstate;

    public static bool movement;

    float distance = 90.8f / 8680f;

    Player player;

    private void Awake()
    {
        mcamera = this;
    }

    // Use this for initialization
    void Start () {
        SetLastPos();
        SetPoint();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SetLastPos() {
        float lastp = PlayerPrefs.GetFloat("LASTPOSY", 0);
        if (lastp < 0) lastp = 0;
        else if (lastp > 90.8f) lastp = 90.8f;
        transform.position += new Vector3(0, lastp);
        container.anchoredPosition = new Vector2(container.anchoredPosition.x, -lastp / distance + 4740f);
    }

    void SetPoint() {
        float x = PlayerPrefs.GetFloat("LASTPOSX", 0);
        float y = PlayerPrefs.GetFloat("LASTPOSY", 0);
        starPoint.transform.position = new Vector3(x, y, starPoint.transform.position.z);
    }

    public void CameraPosUpdate()
    {
        transform.position = new Vector3(transform.position.x, -(container.anchoredPosition.y - 4740f) * distance, transform.position.z);
        if (setstate)
            movement = true;
    }

    public void PopUpShow(Player player_map)
    {
        isPopUp = true;
        CameraMovement.mcamera.FreezeMap();
        player = player_map;
        Image[] stars = new Image[3];
        stars[0] = PopUp.transform.GetChild(1).GetComponent<Image>();
        stars[1] = PopUp.transform.GetChild(2).GetComponent<Image>();
        stars[2] = PopUp.transform.GetChild(3).GetComponent<Image>();

        for (int i = 0; i < 3; i++)
        {
            if (i < player_map.Stars)
            {
                stars[i].sprite = star[0];
            }
            else {
                stars[i].sprite = star[1];
            }
        }

        PopUp.transform.GetChild(4).GetComponent<Text>().text = player_map.HightScore.ToString();
        PopUp.transform.GetChild(5).GetComponent<Text>().text = player_map.Level.ToString();
        Animation anim = PopUp.GetComponent<Animation>();
        anim.enabled = true;
        PopUp.SetActive(true);
    }

    public void FreezeMap()
    {
        DataLoader.enableClick = false;
        fade.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void UnFreezeMap() {
        SoundController.Sound.Click();
        PopUp.SetActive(false);
        isPopUp = false;
        DataLoader.enableClick = true;
        fade.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void ArcadeScene() {
        ButtonActionController.Click.ArcadeScene(player);
    }

    public void BackScene() {
        ButtonActionController.Click.HomeScene();
    }

}
