/// ========================================================
/// 描述：
/// 问题小记： 
/// 作者：HUI 
/// 创建时间：2019-12-09 21:42:32
/// 版 本：1.0
/// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController gameController;

    private int cellNotEmpty;

    [SerializeField]
    private FruitObj fruitStar;

    public int CellNotEmpty
    {
        get
        {
            return cellNotEmpty;
        }

        set
        {
            cellNotEmpty = value;
        }
    }

    public FruitObj FruitStar
    {
        get
        {
            return fruitStar;
        }

        set
        {
            fruitStar = value;
        }
    }

    private void Awake()
    {
        gameController = this;
    }

    private IEnumerator Start()
    {
        if (PlayerInfo.MODE == 1)
        {
            StartCoroutine(GridManager.grid.GridMapCreate(PlayerInfo.MapPlayer.Name));
        }
        else
        {
            StartCoroutine(GridManager.grid.GridMapCreate("classic"));
        }
        yield return new WaitForSeconds(1.5f);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
