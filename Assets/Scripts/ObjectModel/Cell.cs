/// ========================================================
/// 描述：
/// 问题小记： 
/// 作者：HUI 
/// 创建时间：2019-12-16 00:12:11
/// 版 本：1.0
/// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cell {
    private int cellType;
    private Vector2 cellPosition;
    private int cellEffect;

    public int CellType
    {
        get
        {
            return cellType;
        }

        set
        {
            cellType = value;
        }
    }

    public Vector2 CellPosition
    {
        get
        {
            return cellPosition;
        }

        set
        {
            cellPosition = value;
        }
    }

    public int CellEffect
    {
        get
        {
            return cellEffect;
        }

        set
        {
            cellEffect = value;
        }
    }
}
