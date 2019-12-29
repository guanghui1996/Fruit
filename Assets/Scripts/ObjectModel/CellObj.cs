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

    public void CelltypeProcess()
    {
        if (Cell.CellType > 1)
        {
            Cell.CellType--;
            runAnim();
            if (Cell.CellType == 1)
            {
                GameController.gameController.CellNotEmpty--;
                if (GameController.gameController.CellNotEmpty == 0)
                    GameController.gameController.IsShowStar = true;
            }
        }
    }

    void runAnim()
    {
        Animation anim = GetComponent<Animation>();
        anim.enabled = true;
        anim.Play("CellChangeSprite");
    }

    public void SetSprite(int type)
    {
        this.GetComponent<SpriteRenderer>().sprite = GridManager.grid.CellSprite[type];
        setChildEffectSprite(Cell.CellEffect);
    }

    public void RemoveEffect()
    {
        if (Cell.CellEffect > 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            if (Cell.CellEffect == 5)
            {
                SoundController.Sound.IceCrash();
            }
            else if (Cell.CellEffect == 4)
            {
                SoundController.Sound.LockCrash();
            }
            Cell.CellEffect = 0;
            if (FruitSpawner.spawn.FruitGridScript[(int)Cell.CellPosition.x, (int)Cell.CellPosition.y] != null) {
                FruitSpawner.spawn.FruitGridScript[(int)Cell.CellPosition.x, (int)Cell.CellPosition.y].RuleChecker();
            }
        }
    }

    void setChildEffectSprite(int cellEffect)
    {
        if (cellEffect > 0)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = GridManager.grid.CellSprite[cellEffect];
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
