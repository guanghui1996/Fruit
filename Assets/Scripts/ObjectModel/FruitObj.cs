/// ========================================================
/// 描述：
/// 问题小记： 
/// 作者：HUI 
/// 创建时间：2019-12-15 23:50:22
/// 版 本：1.0
/// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitObj : MonoBehaviour {

    [SerializeField]
    private Fruit fruit;

    [SerializeField]
    private SpriteRenderer render;

    private Animation anim;

    private const float DELAY = 0.2f;

    public bool Checked;

    public bool isMove;

    private void Awake()
    {
        anim = Render.GetComponent<Animation>();
    }

    public void Destroy()
    {
        RemoveFromList((int)Fruit.FruitPosition.x, (int)Fruit.FruitPosition.y);
        StartCoroutine(_Destroy());
    }

    public void RuleChecker()
    {
        if (Fruit.FruitType != 99)
        {
            List<FruitObj> list = Ulti.ListPlus(GetRow(Fruit.FruitPosition, Fruit.FruitType, null), GetCollumn(Fruit.FruitPosition, Fruit.FruitType, null), this);
            if (list.Count >= 3)
            {
                listProcess(list);
                Checked = true;
            }
            else
            {
                GameController.gameController.WinChecker();
            }
        }
    }

    private void RemoveFromList(int x, int y)
    {

    }

    IEnumerator _Destroy()
    {
        GridManager.grid.GridCellObj[(int)Fruit.FruitPosition.x, (int)Fruit.FruitPosition.y].CelltypeProcess();


        yield return new WaitForSeconds(DELAY);
        if (Fruit.FruitPower > 0)
        {
            Debug.Log("消失特效");
        }
        GameController.gameController.Drop.DELAY = GameController.DROP_DELAY;
        FruitCrash();
        yield return new WaitForEndOfFrame();


        StopAllCoroutines();
        Destroy(gameObject);
    }

    void FruitCrash()
    {
        int x = (int)Fruit.FruitPosition.x;
        int y = (int)Fruit.FruitPosition.y;

        //特效
    }

    /// <summary>
    /// 获取新位置  并进行下落动画
    /// </summary>
    public void getNewPosition()
    {
        int newpos = (int)Fruit.FruitPosition.y;
        int x = (int)Fruit.FruitPosition.x;
        int oldpos = (int)Fruit.FruitPosition.y;

        for (int y = newpos; y >= 0; y--)
        {
            if (GridManager.grid.Map[x, y] != 0 && GridManager.grid.GridCellObj[x, y].Cell.CellEffect != 4 && FruitSpawner.spawn.FruitGridScript[x, y] == null)
                newpos = y;
            else if (GridManager.grid.Map[x, y] != 0 && GridManager.grid.GridCellObj[x, y].Cell.CellEffect == 4)
                break;

            FruitSpawner.spawn.FruitGridScript[x, (int)Fruit.FruitPosition.y] = null;
            FruitSpawner.spawn.FruitGrid[x, (int)Fruit.FruitPosition.y] = null;

            Fruit.FruitPosition = new Vector2(x, newpos);
            FruitSpawner.spawn.FruitGridScript[x, newpos] = this;
            FruitSpawner.spawn.FruitGrid[x, newpos] = this.gameObject;
            if (oldpos != newpos)
                StartCoroutine(Ulti.IEDrop(this.gameObject, Fruit.FruitPosition, GameController.DROP_SPEED));
        }
    }

    void listProcess(List<FruitObj> list)
    {
        List<int> _listint = new List<int>();
        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].Checked)
                _listint.Add(list[i].getListcount());
            else
                _listint.Add(list.Count);
        }
        int max = Mathf.Max(_listint.ToArray());
        int idx = _listint.IndexOf(max);
        GameController.gameController.FruitProcess(list[idx].getList(), this.gameObject);
    }

    public int getListcount()
    {
        List<FruitObj> list = Ulti.ListPlus(GetRow(Fruit.FruitPosition, Fruit.FruitType, null),
                                            GetCollumn(Fruit.FruitPosition, Fruit.FruitType, null),
                                            this);

        return list.Count;
    }

    public List<FruitObj> getList()
    {
        List<FruitObj> list = Ulti.ListPlus(GetRow(Fruit.FruitPosition, Fruit.FruitType, null),
                                            GetCollumn(Fruit.FruitPosition, Fruit.FruitType, null),
                                            this);

        return list;
    }


    void PowerProcess(int power)
    {
        switch (power)
        {
            case 1:
                
                break;
            case 2:

                break;
            case 3:

                break;
            case 4:

                break;
            default:
                break;
        }
    }


    #region 动画相关

    public void Bounce()
    {
        if (GameController.gameController.GameState == (int)Timer.GameState.PLAYING && !Supporter.supporter.IsNoMove)
        {
            Animation anim = render.GetComponent<Animation>();
            anim.enabled = true;
            anim.Play("bounce");
        }
    }

    public void FruitDisable()
    {
        anim.enabled = true;
        anim.Play("Disable");
    }

    public void FruitEnable()
    {
        anim.enabled = true;
        anim.Play("Enable");
    }

    public void FruitSuggesttion()
    {
        anim.enabled = true;
        anim.Play("Suggesttion");
    }

    public void FruitStopSuggesttion()
    {
        if (anim.IsPlaying("Suggesttion"))
        {
            anim.Stop("Suggesttion");
            anim.enabled = false;
            transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    public void SetBackAnimation(GameObject Obj)
    {
        if (!Supporter.supporter.IsNoMove)
        {
            Vector2 ObjPos = Obj.GetComponent<FruitObj>().Fruit.FruitPosition;
            Animation anim = transform.GetChild(0).GetComponent<Animation>();
            anim.enabled = true;

            if (ObjPos.x == Fruit.FruitPosition.x)
            {
                if (ObjPos.y > Fruit.FruitPosition.y)
                {
                    anim.Play("MoveBack_Up");
                }
                else
                {
                    anim.Play("MoveBack_Down");
                }
            }
            else
            {
                if (ObjPos.x > Fruit.FruitPosition.x)
                {
                    anim.Play("MoveBack_Right");
                }
                else
                {
                    anim.Play("MoveBack_Left");
                }
            }
        }
    }

    #endregion

    #region 相连列表
    /// <summary>
    /// 获取同列相连
    /// </summary>
    /// <param name="Pos"></param>
    /// <param name="type"></param>
    /// <param name="bonus"></param>
    /// <returns></returns>
    public List<FruitObj> GetCollumn(Vector2 Pos, int type, FruitObj bonus)
    {
        List<FruitObj> tmp1 = GetTop(Pos, type);
        List<FruitObj> tmp2 = GetBot(Pos, type);
        if (tmp1.Count + tmp2.Count > 1)
        {
            return Ulti.ListPlus(tmp1, tmp2, bonus);
        }
        else
            return new List<FruitObj>();
    }
    /// <summary>
    /// 获取同行相连
    /// </summary>
    /// <param name="Pos"></param>
    /// <param name="type"></param>
    /// <param name="bonus"></param>
    /// <returns></returns>
    public List<FruitObj> GetRow(Vector2 Pos, int type, FruitObj bonus)
    {
        List<FruitObj> tmp1 = GetLeft(Pos, type);
        List<FruitObj> tmp2 = GetRight(Pos, type);
        if (tmp1.Count + tmp2.Count > 1)
        {
            return Ulti.ListPlus(tmp1, tmp2, bonus);
        }
        else
            return new List<FruitObj>();
    }

    private List<FruitObj> GetTop(Vector2 Pos,int type)
    {
        List<FruitObj> tmp = new List<FruitObj>();
        for (int y = (int)Pos.y + 1; y < 9; y++)
        {
            if (y != Fruit.FruitPosition.y && FruitSpawner.spawn.FruitGridScript[(int)Pos.x, y] != null && FruitSpawner.spawn.FruitGridScript[(int)Pos.x, y].Fruit.FruitType == type && GridManager.grid.GridCellObj[(int)Pos.x, y].Cell.CellEffect == 0)
            {
                tmp.Add(FruitSpawner.spawn.FruitGridScript[(int)Pos.x, y]);
            }
            else
                return tmp;
        }
        return tmp;
    }
    private List<FruitObj> GetBot(Vector2 Pos, int type)
    {
        List<FruitObj> tmp = new List<FruitObj>();
        for (int y = (int)Pos.y - 1; y >= 0; y--)
        {
            if (y != Fruit.FruitPosition.y && FruitSpawner.spawn.FruitGridScript[(int)Pos.x, y] != null && FruitSpawner.spawn.FruitGridScript[(int)Pos.x, y].Fruit.FruitType == type && GridManager.grid.GridCellObj[(int)Pos.x, y].Cell.CellEffect == 0)
            {
                tmp.Add(FruitSpawner.spawn.FruitGridScript[(int)Pos.x, y]);
            }
            else
                return tmp;
        }
        return tmp;
    }
    private List<FruitObj> GetLeft(Vector2 Pos, int type)
    {
        List<FruitObj> tmp = new List<FruitObj>();
        for (int x = (int)Pos.x - 1; x >= 0; x--)
        {
            if (x != Fruit.FruitPosition.x && FruitSpawner.spawn.FruitGridScript[x, (int)Pos.y] != null && FruitSpawner.spawn.FruitGridScript[x, (int)Pos.y].Fruit.FruitType == type && GridManager.grid.GridCellObj[x, (int)Pos.y].Cell.CellEffect == 0)
            {
                tmp.Add(FruitSpawner.spawn.FruitGridScript[x, (int)Pos.y]);
            }
            else
                return tmp;
        }
        return tmp;
    }
    private List<FruitObj> GetRight(Vector2 Pos, int type)
    {
        List<FruitObj> tmp = new List<FruitObj>();
        for (int x = (int)Pos.x + 1; x < 7; x++)
        {
            if (x != Fruit.FruitPosition.x && FruitSpawner.spawn.FruitGridScript[x, (int)Pos.y] != null && FruitSpawner.spawn.FruitGridScript[x, (int)Pos.y].Fruit.FruitType == type && GridManager.grid.GridCellObj[x, (int)Pos.y].Cell.CellEffect == 0)
            {
                tmp.Add(FruitSpawner.spawn.FruitGridScript[x, (int)Pos.y]);
            }
            else
                return tmp;
        }
        return tmp;
    }
    #endregion


    #region 属性
    public Fruit Fruit
    {
        get
        {
            return fruit;
        }

        set
        {
            fruit = value;
        }
    }

    public SpriteRenderer Render
    {
        get
        {
            return render;
        }

        set
        {
            render = value;
        }
    }
    #endregion
}
