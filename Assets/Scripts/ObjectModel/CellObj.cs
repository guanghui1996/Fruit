/// ========================================================
/// 描述：
/// 问题小记： 
/// 作者：HUI 
/// 创建时间：2019-12-16 00:12:20
/// 版 本：1.0
/// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellObj : MonoBehaviour {

    private int cellCode;
    private Cell cell;

    public Cell Cell
    {
        get
        {
            return cell;
        }

        set
        {
            cell = value;
        }
    }

    public int CellCode
    {
        get
        {
            return cellCode;
        }

        set
        {
            cellCode = value;
        }
    }

    public void SetSpriteEvent()
    {
        SetSprite(cell.CellType - 1);
    }

    public void SetSprite(int type)
    {
        this.GetComponent<SpriteRenderer>().sprite = GridManager.grid.CellSprite[type];
    }
}
