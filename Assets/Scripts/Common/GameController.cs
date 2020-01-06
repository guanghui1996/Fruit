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

    public static float DROP_SPEED = 8;
    public static float DROP_DELAY = -1f;

    [SerializeField]
    private GameObject selector;

    [SerializeField]
    private FruitObj fruitStar;

    [SerializeField]
    private GameObject noSelector;

    [SerializeField]
    private Animation starAnim;

    [SerializeField]
    private SpawnController drop;

    private FruitObj fruitScript;
    private FruitObj fruitScript1;

    private int cellNotEmpty;

    private int gameState;

    private bool isStar;

    private bool isShowStar;

    private bool isAddPower;

    private bool isHold;

    private GameObject Pointer;

    private GameObject Selected;



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
        StartCoroutine(EffectSpawner.effect.ComboTick());
        Timer.timer.TimeTick(true);
        GameState = (int)Timer.GameState.PLAYING;
        NoSelector.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        FruitSelecter();
        
    }

    #region 游戏规则内控制

    /// <summary>
    /// 水果选择
    /// </summary>
    void FruitSelecter()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isHold = true;

            if (Pointer == null)
            {
                Pointer = FruitTouchChecker(Input.mousePosition);
                //Debug.Log(Pointer);
            }

            Supporter.supporter.StopSuggesttionAnim();
            if (Pointer != null && !Pointer.name.Contains("Fruit"))
                Pointer = null;
        }
        else if (Input.GetMouseButton(0) && isHold)
        {
            if (Pointer != null)
            {
                EnableSelector(Pointer.transform.position);
                Selected = FruitTouchChecker(Input.mousePosition);
                if (Selected != null && Pointer != Selected && Selected.name.Contains("Fruit"))
                {
                    if (DistanceChecker(Pointer, Selected))
                    {
                        RuleChecker(Pointer, Selected);
                        Pointer = null;
                        Selected = null;
                        Selector.SetActive(false);
                    }
                    else
                    {
                        Pointer = Selected;
                        Selected = null;
                        EnableSelector(Pointer.transform.position);
                    }
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isHold = false;
        }
    }

    /// <summary>
    /// 距离判断  判断选择的两个物体是否相邻
    /// </summary>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    /// <returns></returns>
    bool DistanceChecker(GameObject obj1, GameObject obj2)
    {
        Vector2 v1 = obj1.GetComponent<FruitObj>().Fruit.FruitPosition;
        Vector2 v2 = obj2.GetComponent<FruitObj>().Fruit.FruitPosition;
        if (Vector2.Distance(v1, v2) <= 1)
            return true;
        return false;
    }


    public void RuleChecker(GameObject obj1, GameObject obj2)
    {
        FruitObj fruit1 = obj1.GetComponent<FruitObj>();
        FruitObj fruit2 = obj2.GetComponent<FruitObj>();
        List<FruitObj> fruitObjs1 = Ulti.ListPlus(fruit1.GetCollumn(fruit2.Fruit.FruitPosition, fruit1.Fruit.FruitType, null), fruit1.GetRow(fruit2.Fruit.FruitPosition, fruit1.Fruit.FruitType, null),fruit1);
        
        List<FruitObj> fruitObjs2 = Ulti.ListPlus(fruit2.GetCollumn(fruit1.Fruit.FruitPosition, fruit2.Fruit.FruitType, null), fruit2.GetRow(fruit1.Fruit.FruitPosition, fruit2.Fruit.FruitType, null), fruit2);


        if (fruit1.Fruit.FruitType == 99 || fruit2.Fruit.FruitType == 99)
        {
            if (fruit1.Fruit.FruitType == 8 || fruit2.Fruit.FruitType == 8)
            {
                fruit1.SetBackAnimation(obj2);
                fruit2.SetBackAnimation(obj1);
                return;
            }
        }

        if (fruitObjs1.Count >= 3 || fruitObjs2.Count >= 3 || fruit1.Fruit.FruitType == 8 || fruit2.Fruit.FruitType == 8)
        {
            Ulti.MoveTo(obj1, obj2.transform.localPosition, 0.2f);
            Ulti.MoveTo(obj2, obj1.transform.localPosition, 0.2f);
            SwapFruitPosition(obj1, obj2);
            FruitProcess(fruitObjs1, fruitObjs2, obj1, obj2);
        }
        else
        {
            fruit1.SetBackAnimation(obj2);
            fruit2.SetBackAnimation(obj1);
        }
    }

    GameObject FruitTouchChecker(Vector3 mousePos)
    {
        Vector3 wp = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 touchPos = new Vector2(wp.x, wp.y);
        if (Physics2D.OverlapPoint(touchPos))
        {
            //Debug.Log(Physics2D.OverlapPoint(touchPos).gameObject);
            return Physics2D.OverlapPoint(touchPos).gameObject;
        }
        return null;
    }

    /// <summary>
    /// 交换水果位置
    /// </summary>
    /// <param name="fruit1"></param>
    /// <param name="fruit2"></param>
    void SwapFruitPosition(GameObject fruit1, GameObject fruit2)
    {
        FruitObj tmp1 = fruit1.GetComponent<FruitObj>();
        FruitObj tmp2 = fruit2.GetComponent<FruitObj>();

        Vector2 tmp = tmp1.Fruit.FruitPosition;
        tmp1.Fruit.FruitPosition = tmp2.Fruit.FruitPosition;
        tmp2.Fruit.FruitPosition = tmp;

        GameObject Objtmp = FruitSpawner.spawn.FruitGrid[(int)tmp1.Fruit.FruitPosition.x, (int)tmp1.Fruit.FruitPosition.y];
        FruitSpawner.spawn.FruitGrid[(int)tmp1.Fruit.FruitPosition.x, (int)tmp1.Fruit.FruitPosition.y] = fruit2;
        FruitSpawner.spawn.FruitGrid[(int)tmp2.Fruit.FruitPosition.x, (int)tmp2.Fruit.FruitPosition.y
            ] = Objtmp;

        FruitObj scriptObj = tmp1;
        FruitSpawner.spawn.FruitGridScript[(int)tmp2.Fruit.FruitPosition.x, (int)tmp2.Fruit.FruitPosition.y] = tmp2;
        FruitSpawner.spawn.FruitGridScript[(int)tmp1.Fruit.FruitPosition.x, (int)tmp1.Fruit.FruitPosition.y] = scriptObj;
        if (tmp1.Fruit.FruitType == 99 || tmp2.Fruit.FruitType == 99)
        {
            WinChecker();
        }

    }

    public void WinChecker()
    {
        int Min = 0;
        for (int y = 0; y < 9; y++)
        {
            if (FruitStar == null)
            {
                return;
            }
            if (GridManager.grid.GridCellObj[(int)FruitStar.Fruit.FruitPosition.x, y] != null)
            {
                Min = y;
                break;
            }

            if ((int)FruitStar.Fruit.FruitPosition.y == Min)
            {
                Timer.timer.Win();
                Destroy(FruitStar.gameObject);
            }
        }
    }

    void EnableSelector(Vector3 pos)
    {
        Selector.transform.position = pos;
        Selector.SetActive(true);
    }

    public void FruitProcess(List<FruitObj> list1, GameObject obj1)
    {
        int c1 = list1.Count;
        if (c1 > 2)
        {
            ListProcess(list1, obj1, null, obj1.GetComponent<FruitObj>().Fruit.FruitType);
        }
    }

    void FruitProcess(List<FruitObj> list1, List<FruitObj> list2, GameObject obj1, GameObject obj2)
    {
        int c1 = list1.Count;
        int c2 = list2.Count;


        if (c1 > 2)
        {
            ListProcess(list1, obj2, obj1, obj1.GetComponent<FruitObj>().Fruit.FruitType);
        }
        else if (obj1.GetComponent<FruitObj>().Fruit.FruitType == 8)
        {
            obj2.GetComponent<FruitObj>().Destroy();
            PDestroyType(obj2.GetComponent<FruitObj>().Fruit.FruitType, obj2.transform.position);
            obj1.GetComponent<FruitObj>().Destroy();
        }

        if (c2 > 2)
        {
            ListProcess(list2, obj1, obj2, obj2.GetComponent<FruitObj>().Fruit.FruitType);
        }
        else if (obj2.GetComponent<FruitObj>().Fruit.FruitType == 8)
        {
            obj1.GetComponent<FruitObj>().Destroy();
            PDestroyType(obj1.GetComponent<FruitObj>().Fruit.FruitType, obj1.transform.position);
            obj2.GetComponent<FruitObj>().Destroy();
        }
    }

    bool ListProcess(List<FruitObj> list, GameObject obj, GameObject obj1, int type)
    {
        Vector3 v;

        if (obj1 != null)
        {
            FruitScript = obj1.GetComponent<FruitObj>();
            v = new Vector3(FruitScript.Fruit.FruitPosition.x, FruitScript.Fruit.FruitPosition.y);
        }
        else
        {
            FruitScript = obj.GetComponent<FruitObj>();
            v = new Vector3(FruitScript.Fruit.FruitPosition.x, FruitScript.Fruit.FruitPosition.y);
        }

        int c = list.Count;
        if (c == 3)
        {
            DestroyFruit(list);
            EffectSpawner.effect.ComboInc();
            dropFruit();
            return false;
        }
        else if (c == 4)
        {
            ReGroup(list, type, (int)Power.BOOM, v);
            DestroyRandom();
            EffectSpawner.effect.ComboInc();
            dropFruit();
        }
        else if (c >= 5)
        {
            ReGroup(list, 8, (int)Power.MAGIC, v);
            EffectSpawner.effect.ComboInc();
            DestroyRandom();
            DestroyRandom();
            dropFruit();
        }
        return true;
    }
    #endregion

    public void PDestroyType(int type, Vector3 pos)
    {
        StartCoroutine(DestroyType(type, pos));
    }

    void DestroyFruit(List<FruitObj> list)
    {
        SoundController.Sound.FruitCrash();
        EffectSpawner.effect.Glass();
        foreach (var item in list)
        {
            item.Destroy();
        }
    }

    public void DestroyRandom()
    {
        dropFruit();
        if (PlayerInfo.MODE == 1)
        {
            if (!IsStar)
            {
                List<CellObj> listeff = getListCellEffect();

                if (listeff.Count > 0)
                {
                    CellObj tmp = listeff[Random.Range(0, listeff.Count)];
                    tmp.RemoveEffect();
                    EffectSpawner.effect.Thunder(GridManager.grid.GridCell[(int)tmp.Cell.CellPosition.x, (int)tmp.Cell.CellPosition.y].transform.position);
                }
                else
                {
                    destroyNotEmpty();
                }
            }
            else
            {
                Vector2 vtmp = posUnderStar();
                FruitObj tmp = FruitSpawner.spawn.FruitGridScript[(int)vtmp.x, (int)vtmp.y];
                if (tmp != null && tmp != FruitStar)
                {
                    tmp.Destroy();
                    EffectSpawner.effect.Thunder(GridManager.grid.GridCell[(int)tmp.Fruit.FruitPosition.x, (int)tmp.Fruit.FruitPosition.y].transform.position);
                }
            }
        }
    }

    void ReGroup(List<FruitObj> list, int type, int power, Vector2 pos)
    {
        SoundController.Sound.FruitCrash();
        EffectSpawner.effect.Glass();
        foreach (var item in list)
        {
            item.ReGroup(pos);
        }
        StartCoroutine(SpawnFruitPower(type, power, pos));
    }

    /// <summary>
    /// 获取单元格属性
    /// </summary>
    /// <returns></returns>
    private List<CellObj> getListCellEffect()
    {
        List<CellObj> tmp = new List<CellObj>();
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 7; x++)
            {
                if (GridManager.grid.GridCellObj[x, y] != null && GridManager.grid.GridCellObj[x, y].Cell.CellEffect > 0)
                {
                    tmp.Add(GridManager.grid.GridCellObj[x, y]);
                }
            }
        }
        return tmp;
    }

    /// <summary>
    /// 删除随机非空单元上物体
    /// </summary>
    private void destroyNotEmpty()
    {
        try
        {
            List<CellObj> listNotEmpty = getcListNotEmpty();
            if (listNotEmpty.Count > 0)
            {
                Vector2 tmp = listNotEmpty[Random.Range(0, listNotEmpty.Count)].Cell.CellPosition;
                if (FruitSpawner.spawn.FruitGridScript[(int)tmp.x, (int)tmp.y] != null)
                {
                    FruitSpawner.spawn.FruitGridScript[(int)tmp.x, (int)tmp.y].Destroy();
                    EffectSpawner.effect.Thunder(GridManager.grid.GridCellObj[(int)tmp.x, (int)tmp.y].transform.position);
                }
            }
        }
        catch
        {

        }
    }

    /// <summary>
    /// 获取非空单元列表
    /// </summary>
    /// <returns></returns>
    private List<CellObj> getcListNotEmpty()
    {
        List<CellObj> tmp = new List<CellObj>();
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 7; x++)
            {
                if (GridManager.grid.GridCellObj[x, y] != null && GridManager.grid.GridCellObj[x, y].Cell.CellType > 1)
                {
                    if (FruitSpawner.spawn.FruitGridScript[x, y] != null)
                        tmp.Add(GridManager.grid.GridCellObj[x, y]);
                }
            }
        }
        return tmp;
    }

    private Vector2 posUnderStar()
    {
        List<Vector2> under = new List<Vector2>();
        int x = (int)FruitStar.Fruit.FruitPosition.x;
        int y = (int)FruitStar.Fruit.FruitPosition.y;
        for (int i = 0; i < y; i++)
        {
            if (FruitSpawner.spawn.FruitGridScript[x, i] != null)
                under.Add(FruitSpawner.spawn.FruitGridScript[x, i].Fruit.FruitPosition);
        }
        if (under.Count > 0)
        {
            return under[Random.Range(0, under.Count)];
        }
        else
        {
            return new Vector2(x, y);
        }
    }

    #region 特效相关

    /// <summary>
    /// 控制水果下落
    /// </summary>
    void dropFruit()
    {
        if (DROP_DELAY < 0)
            DROP_DELAY = 0.5f;
        Drop.DELAY = DROP_DELAY;
        Drop.enabled = true;
    }

    /// <summary>
    /// 随机奖励
    /// </summary>
    public void AddBonusPower()
    {
        int dem = 0;
        while (true)
        {
            dem++;
            if (dem >= 63)
                return;
            int x = Random.Range(0, 7);
            int y = Random.Range(0, 9);
            FruitObj tmp = FruitSpawner.spawn.FruitGridScript[x, y];
            if (tmp != null && tmp.Fruit.FruitType != 8 && tmp.Fruit.fruitPower == 0 && GridManager.grid.GridCellObj[x, y].Cell.CellEffect == 0)
            {
                int r = Random.Range(2, 4);
                tmp.Fruit.FruitPower = r;
                EffectSpawner.effect.ThunderRow(FruitSpawner.spawn.FruitGrid[x, y], r);
                return;
            }
        }
    }

    /// <summary>
    /// 爆炸消除
    /// </summary>
    public void PBoom(int x, int y)
    {
        dropFruit();
        SoundController.Sound.Boom();
        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (i != x || j != y)
                {
                    if (i >= 0 && i < 7 && j >= 0 && j < 9 && FruitSpawner.spawn.FruitGridScript[i, j] != null && FruitSpawner.spawn.FruitGridScript[i, j].Fruit.FruitType != 99)
                    {
                        FruitSpawner.spawn.FruitGridScript[i, j].Destroy();
                    }
                }
            }
        }
    }

    /// <summary>
    /// 消除行
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    public void PDestroyRow(int _x, int y)
    {
        dropFruit();
        SoundController.Sound.Fire();
        List<CellObj> cellEffect = new List<CellObj>();
        List<FruitObj> fruitObjs = new List<FruitObj>();
        for (int x = 0; x < 7; x++)
        {
            if (x != _x)
            {
                if (GridManager.grid.GridCellObj[x, y] != null && GridManager.grid.GridCellObj[x, y].Cell.CellEffect > 0)
                {
                    cellEffect.Add(GridManager.grid.GridCellObj[x, y]);
                }
                if (FruitSpawner.spawn.FruitGridScript[x, y] != null && FruitSpawner.spawn.FruitGridScript[x, y].Fruit.FruitType != 99 && GridManager.grid.GridCellObj[x, y].Cell.CellEffect == 0)
                {
                    fruitObjs.Add(FruitSpawner.spawn.FruitGridScript[x, y]);
                }
            }

            foreach (CellObj item in cellEffect)
            {
                item.RemoveEffect();
            }
            foreach (FruitObj item in fruitObjs)
            {
                item.Destroy();
            }
        }
    }

    /// <summary>
    /// 消除列
    /// </summary>
    /// <param name="x"></param>
    /// <param name="_y"></param>
    public void PDestroyCollumn(int x, int _y)
    {
        dropFruit();
        SoundController.Sound.Fire();
        List<CellObj> cellEffect = new List<CellObj>();
        List<FruitObj> fruitObjs = new List<FruitObj>();
        for (int y = 0; y < 9; y++)
        {
            if (y != _y)
            {
                if (GridManager.grid.GridCellObj[x, y] != null && GridManager.grid.GridCellObj[x, y].Cell.CellEffect > 0)
                {
                    cellEffect.Add(GridManager.grid.GridCellObj[x, y]);
                }
                if (FruitSpawner.spawn.FruitGridScript[x, y] != null && FruitSpawner.spawn.FruitGridScript[x, y].Fruit.FruitType != 99 && GridManager.grid.GridCellObj[x, y].Cell.CellEffect == 0)
                {
                    fruitObjs.Add(FruitSpawner.spawn.FruitGridScript[x, y]);
                }
            }

            foreach (CellObj item in cellEffect)
            {
                item.RemoveEffect();
            }
            foreach (FruitObj item in fruitObjs)
            {
                item.Destroy();
            }
        }
    }

    /// <summary>
    /// 移除特效
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void CellRemoveEffect(int x, int y)
    {
        if (x - 1 >= 0 && GridManager.grid.GridCellObj[x - 1, y] != null)
            GridManager.grid.GridCellObj[x - 1, y].RemoveEffect();
        if (x + 1 < 7 && GridManager.grid.GridCellObj[x + 1, y] != null)
            GridManager.grid.GridCellObj[x + 1, y].RemoveEffect();
        if (y - 1 >= 0 && GridManager.grid.GridCellObj[x, y - 1] != null)
            GridManager.grid.GridCellObj[x, y - 1].RemoveEffect();
        if (y + 1 < 9 && GridManager.grid.GridCellObj[x, y + 1] != null)
            GridManager.grid.GridCellObj[x, y + 1].RemoveEffect();
    }

    /// <summary>
    /// 时间道具
    /// </summary>
    public void PBonusTime()
    {
        StartCoroutine(TimeInc());
    }
    #endregion

    /// <summary>
    /// 星星显示
    /// </summary>
    public void ShowStar()
    {
        List<Vector2> listpos = new List<Vector2>();
        Vector2 pos;
        for (int y = 9 - 1; y >= 0; y--)
        {
            for (int x = 0; x < 7; x++)
            {
                if (GridManager.grid.GridCellObj[x, y] != null)
                    listpos.Add(new Vector2(x, y));
            }
            if (listpos.Count > 0)
                break;
        }
        pos = listpos[Random.Range(0, listpos.Count)];
        FruitSpawner.spawn.SpawnStar(pos);
        SoundController.Sound.StarIn();
    }


    /// <summary>
    /// 时间增加
    /// </summary>
    /// <returns></returns>
    IEnumerator TimeInc()
    {
        int dem = 0;
        int t = 22;
        while (t > 0)
        {
            dem++;
            Timer.timer.GameTime += 1;
            if (Timer.timer.GameTime >= 270f)
            {
                Timer.timer.GameTime = 270f;
                break;
            }
            t -= 1;
            yield return null;
            if (dem >= 270) break;
        }
    }

    IEnumerator DestroyType(int type, Vector3 pos)
    {
        NoSelector.SetActive(true);
        dropFruit();
        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                FruitObj tmp = FruitSpawner.spawn.FruitGridScript[x, y];
                if (tmp != null && tmp.Fruit.FruitType == type)
                {
                    //生成特效
                    tmp.Destroy();
                }
            }
        }
        yield return new WaitForSeconds(0.2f);
        NoSelector.SetActive(false);
    }

    IEnumerator SpawnFruitPower(int type, int power, Vector2 pos)
    {
        yield return new WaitForSeconds(0.4f);
        GameObject tmp = FruitSpawner.spawn.SpawnFruitPower(type, power, pos);
        yield return new WaitForSeconds(0.2f);
        tmp.GetComponent<Collider2D>().enabled = true;
    }



 


    #region 属性
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

    public int GameState
    {
        get
        {
            return gameState;
        }

        set
        {
            gameState = value;
        }
    }

    public GameObject Selector
    {
        get
        {
            return selector;
        }

        set
        {
            selector = value;
        }
    }

    public GameObject NoSelector
    {
        get
        {
            return noSelector;
        }

        set
        {
            noSelector = value;
        }
    }

    public bool IsStar
    {
        get
        {
            return isStar;
        }

        set
        {
            isStar = value;
        }
    }

    public bool IsShowStar
    {
        get
        {
            return isShowStar;
        }

        set
        {
            isShowStar = value;
        }
    }

    public bool IsAddPower
    {
        get
        {
            return isAddPower;
        }

        set
        {
            isAddPower = value;
        }
    }

    public Animation StarAnim
    {
        get
        {
            return starAnim;
        }

        set
        {
            starAnim = value;
        }
    }

    public SpawnController Drop
    {
        get
        {
            return drop;
        }

        set
        {
            drop = value;
        }
    }

    public FruitObj FruitScript
    {
        get
        {
            return fruitScript;
        }

        set
        {
            fruitScript = value;
        }
    }

    public FruitObj FruitScript1
    {
        get
        {
            return fruitScript1;
        }

        set
        {
            fruitScript1 = value;
        }
    }

    public enum Power
    {
        BOOM = 1,
        ROW_LIGHTING = 2,
        COLLUMN_LIGHTING = 3,
        MAGIC = 8,
        TIME = 4
    }

    #endregion
}
