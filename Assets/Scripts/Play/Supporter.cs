/// ========================================================
/// 描述：
/// 问题小记： 
/// 作者：HUI 
/// 创建时间：2019-12-19 00:01:51
/// 版 本：1.0
/// ========================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supporter : MonoBehaviour {

    public static Supporter supporter;

    private bool isNoMove;

    public Vector2[] AvaiableMove;

    FruitObj[] AvaiableObj = new FruitObj[2];

    private float SP_DELAY = 5f;

    List<Vector2> vtmpList;

    FruitObj fruit;

    private void Awake()
    {
        supporter = this;
    }

    /// <summary>
    /// 判断当前地图是否还有可以移动的格子
    /// </summary>
    /// <returns></returns>
    public bool IsNoMoreMove()
    {
        StopSuggesttionAnim();
        AvaiableMove = new Vector2[2];
        AvaiableObj = new FruitObj[2];

        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                if (FruitSpawner.spawn.FruitGridScript[x, y] != null && GridManager.grid.GridCellObj[x, y].Cell.CellEffect == 0)
                {
                    fruit = FruitSpawner.spawn.FruitGridScript[x, y];

                    FruitObj obj = MoveChecker(x, y, fruit);

                    if (obj != null)
                    {
                        AvaiableMove[0] = fruit.Fruit.FruitPosition;
                        AvaiableObj[0] = FruitSpawner.spawn.FruitGridScript[(int)AvaiableMove[0].x, (int)AvaiableMove[0].y];
                        AvaiableMove[1] = obj.Fruit.FruitPosition;
                        AvaiableObj[1] = FruitSpawner.spawn.FruitGridScript[(int)AvaiableMove[1].x, (int)AvaiableMove[1].y];
                        isNoMove = false;
                        return true;
                    }
                }
            }
        }

        isNoMove = true;
        return false;
    }

    /// <summary>
    /// 返回可移动的物体
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="fruit"></param>
    /// <returns></returns>
    private FruitObj MoveChecker(int x, int y, FruitObj fruit)
    {
        vtmpList = getListPos(x, y);
        foreach (Vector2 item in vtmpList)
        {
            if (FruitSpawner.spawn.FruitGridScript[(int)item.x, (int)item.y] != null && FruitSpawner.spawn.FruitGridScript[(int)item.x, (int)item.y].Fruit.FruitType == 8)
                return FruitSpawner.spawn.FruitGridScript[(int)item.x, (int)item.y];
            else
            {
                List<FruitObj> objs = Ulti.ListPlus(fruit.GetCollumn(item, fruit.Fruit.FruitType, null), fruit.GetRow(item, fruit.Fruit.FruitType, null), fruit);
                if (objs.Count >= 3)
                    return FruitSpawner.spawn.FruitGridScript[(int)item.x, (int)item.y];
            }
        }
        return null;
    }

    List<Vector2> getListPos(int x, int y)
    {
        vtmpList = new List<Vector2>();
        if (y + 1 < 9 && GridManager.grid.GridCellObj[x, y + 1] != null && GridManager.grid.GridCellObj[x, y + 1].Cell.CellEffect == 0)
        {
            vtmpList.Add(new Vector2(x, y + 1));
        }
        if (y - 1 >= 0 && GridManager.grid.GridCellObj[x, y - 1] != null && GridManager.grid.GridCellObj[x, y - 1].Cell.CellEffect == 0)
        {
            vtmpList.Add(new Vector2(x, y - 1));
        }
        if (x + 1 < 7 && GridManager.grid.GridCellObj[x + 1, y] != null && GridManager.grid.GridCellObj[x + 1, y].Cell.CellEffect == 0)
        {
            vtmpList.Add(new Vector2(x + 1, y));
        }
        if (x - 1 >= 0 && GridManager.grid.GridCellObj[x - 1, y] != null && GridManager.grid.GridCellObj[x - 1, y].Cell.CellEffect == 0)
        {
            vtmpList.Add(new Vector2(x - 1, y));
        }
        return vtmpList;
    }

    public void StopSuggesttionAnim()
    {
        if (AvaiableObj[0] != null)
        {
            AvaiableObj[0].FruitStopSuggesttion();
        }
        if (AvaiableObj[1] != null)
        {
            AvaiableObj[1].FruitStopSuggesttion();
        }
    }

    public bool IsNoMove
    {
        get
        {
            return isNoMove;
        }

        set
        {
            isNoMove = value;
        }
    }
}
