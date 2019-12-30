/// ========================================================
/// 描述：
/// 问题小记： 
/// 作者：HUI 
/// 创建时间：2019-12-12 23:37:10
/// 版 本：1.0
/// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    private Player player;



    private void OnMouseDown()
    {

        Debug.Log("鼠标按下");
        CameraMovement.setstate = true;
        CameraMovement.movement = false;
        if (DataLoader.enableClick == true) {
            transform.GetChild(0).transform.localScale = new Vector3(0.8f, 0.75f, 1);
        }
    }

    private void OnMouseUp()
    {
        Debug.Log("鼠标释放");
        CameraMovement.setstate = false;
        if (DataLoader.enableClick && !CameraMovement.movement) {
            SoundController.Sound.Click();
            transform.GetChild(0).transform.localScale = new Vector3(0.8f, 0.75f, 1);
            PlayerPrefs.DeleteKey("LASTPOSY");
            PlayerPrefs.DeleteKey("LASTPOSX");
            PlayerPrefs.SetFloat("LASTPOSY", transform.position.y);
            PlayerPrefs.SetFloat("LASTPOSX", transform.position.x);

            CameraMovement.mcamera.starPoint.transform.position = transform.position + new Vector3(0, 0, -0.2f);
            CameraMovement.mcamera.PopUpShow(player);
        }
        CameraMovement.movement = false;
    }

    public void SetMapInfo()
    {
        SpriteRenderer render = transform.GetChild(0).GetComponent<SpriteRenderer>();
        SpriteRenderer[] star = new SpriteRenderer[3];
        star[0] = transform.GetChild(2).GetComponent<SpriteRenderer>();
        star[1] = transform.GetChild(3).GetComponent<SpriteRenderer>();
        star[2] = transform.GetChild(4).GetComponent<SpriteRenderer>();

        if (player.Locked)
        {
            render.sprite = DataLoader.dataLoader.MapSprite[0];
            star[0].sprite = null;
            star[1].sprite = null;
            star[2].sprite = null;
            transform.GetComponent<Collider2D>().enabled = false;
            transform.GetChild(1).gameObject.SetActive(false);
        }
        else if (player.Stars == 0)
        {
            render.sprite = DataLoader.dataLoader.MapSprite[1];
            star[0].sprite = DataLoader.dataLoader.MapSprite[4];
            star[1].sprite = DataLoader.dataLoader.MapSprite[4];
            star[2].sprite = DataLoader.dataLoader.MapSprite[4];
        }
        else
        {

            render.sprite = DataLoader.dataLoader.MapSprite[2];
            star[0].sprite = DataLoader.dataLoader.MapSprite[4];
            star[1].sprite = DataLoader.dataLoader.MapSprite[4];
            star[2].sprite = DataLoader.dataLoader.MapSprite[4];
            for (int i = 0; i < player.Stars; i++)
            {
                star[i].sprite = DataLoader.dataLoader.MapSprite[3];
            }
        }
    }
    public Player Player
    {
        get
        {
            return player;
        }

        set
        {
            player = value;
        }
    }
}
