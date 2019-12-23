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
    public static float DROP_DELAY = 0.5f;

    [SerializeField]
    private GameObject selector;

    [SerializeField]
    private FruitObj fruitStar;

    [SerializeField]
    private GameObject noSelector;

    [SerializeField]
    private Animation starAnim;

    private int cellNotEmpty;

    private int gameState;

    private bool isStar;

    private bool isShowStar;

    private bool isAddPower;

    private bool isHold;

    private GameObject Pointer;

    private GameObject Selected;

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

    public enum Power
    {
        BOOM = 1,
        ROW_LIGHTING = 2,
        COLLUMN_LIGHTING = 3,
        MAGIC = 8,
        TIME = 4
    }

    #endregion

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


        Timer.timer.TimeTick(true);
        GameState = (int)Timer.GameState.PLAYING;
        NoSelector.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        FruitSelecter();
        
    }

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
                Debug.Log(Pointer);
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
                Selector = FruitTouchChecker(Input.mousePosition);
                if (Selector != null && Pointer != Selector && Selected.name.Contains("Fruit"))
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
        List<FruitObj> fruitObjs2 = Ulti.ListPlus(fruit2.GetCollumn(fruit1.Fruit.FruitPosition, fruit2.Fruit.FruitType, null), fruit1.GetRow(fruit1.Fruit.FruitPosition, fruit2.Fruit.FruitType, null), fruit2);

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
    }

    GameObject FruitTouchChecker(Vector3 mousePos)
    {
        Vector3 wp = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 touchPos = new Vector2(wp.x, wp.y);
        if (Physics2D.OverlapPoint(touchPos))
        {
            return Physics2D.OverlapPoint(touchPos).gameObject;
        }
        return null;
    }

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
        FruitSpawner.spawn.FruitGridScript[(int)tmp1.Fruit.FruitPosition.x, (int)tmp1.Fruit.FruitPosition.y] = tmp1;
        FruitSpawner.spawn.FruitGridScript[(int)tmp2.Fruit.FruitPosition.x, (int)tmp2.Fruit.FruitPosition.y] = scriptObj;
        if (tmp1.Fruit.FruitType == 99 || tmp2.Fruit.FruitType == 99)
            WinChecker();
    }

    public void WinChecker()
    {
        int Min = 0;
        for (int y = 0; y < 9; y++)
        {
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

        }
    }

    bool ListProcess(List<FruitObj> list, GameObject obj, GameObject obj1, int type)
    {

        return true;
    }



}
