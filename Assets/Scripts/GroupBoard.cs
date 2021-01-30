using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupBoard : MonoBehaviour
{
    [SerializeField] private GameObject objBoard;
    [SerializeField] private List<GameObject> objList = new List<GameObject>();
    [SerializeField] private List<Text> txtName = new List<Text>();

    public int GroupNo { get; set; }
    private List<string> memberNames = new List<string>();
    private List<string> lastMonthNames = new List<string>();

    /// <summary>
    /// 起動時処理
    /// </summary>
    void Awake()
    {
        Initialize();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        memberNames.Clear();
        hide();
    }

    /// <summary>
    /// 非表示（非アクティブ）
    /// </summary>
    private void hide()
    {
        // 全てのプレートを非表示
        foreach (GameObject obj in objList)
        {
            obj.SetActive(false);
        }

        // 下敷きを非表示
        objBoard.SetActive(false);
    }

    /// <summary>
    /// 表示（アクティブ）
    /// </summary>
    public void Show()
    {
        objBoard.SetActive(true);
        for(int i=0; i<memberNames.Count; i++)
        {
            objList[i].SetActive(true);
            txtName[i].text = memberNames[i];
        }
    }

    /// <summary>
    /// メンバー登録
    /// </summary>
    /// <param name="name">メンバー名</param>
    /// <param name="isLastMonth">先月のメンバーか</param>
    public void SetMember(string name, bool isLastMonth = false)
    {
        if (!isLastMonth)   // 今月
        {
            memberNames.Add(name);
        }
        else                // 先月
        {
            lastMonthNames.Add(name);
        }
    }

    /// <summary>
    /// メンバーの保存
    /// </summary>
    /// <param name="date">日付（年月）</param>
    public void Save(System.DateTime date)
    {
        string key = getKey(date);
        string val = "";
        for(int i=0; i<memberNames.Count; i++)
        {
            if (i > 0) val += " ";
            val += memberNames[i];            
        }
        /*
        Debug.Log("--- Save[" + GroupNo + "] ---");
        Debug.Log("  key : " + key);
        Debug.Log("  val : " + val);
        */
        PlayerPrefs.SetString(key, val);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// メンバーの読み込み
    /// </summary>
    /// <param name="date">日付（年月）</param>
    /// <param name="isLastMonth">先月のメンバーか</param>
    public void Load(System.DateTime date, bool isLastMonth = false)
    {
        string key = getKey(date);
        string val = PlayerPrefs.GetString(key);
        string[] members = val.Split(' ');

        // 先月メンバーをクリア
        if (isLastMonth) lastMonthNames.Clear();

        // Debug.Log("--- Load[" + GroupNo + "] : " + val + "---");
        for (int i = 0; i < members.Length; i++)
        {
            // Debug.Log("  val : " + members[i]);
            SetMember(members[i], isLastMonth);
        }
    }

    /// <summary>
    /// メンバーが保存済みか確認
    /// </summary>
    /// <param name="date">日付（年月）</param>
    /// <returns>保存済みの場合は true を返す</returns>
    public bool IsExist(System.DateTime date)
    {
        string key = getKey(date);        
        return PlayerPrefs.GetString(key).Length > 0 ? true : false;
    }

    /// <summary>
    /// 先月もこのグループにいたか確認
    /// </summary>
    /// <param name="name">候補者名</param>
    /// <returns>true の場合、先月と同じ</returns>
    public bool IsContainLastMonth(string name)
    {
        return lastMonthNames.Contains(name);

        // 動作テスト用
        /*
        bool isContain = lastMonthNames.Contains(name);
        if(isContain)
        {
            Debug.Log(string.Format("--- {0} is contains ---", name));
            foreach(string mem in lastMonthNames)
            {
                Debug.Log(" ->" + mem);
            }
        }
        return isContain;
        */
    }

    /// <summary>
    /// 先月所属したグループメンバーを取得
    /// </summary>
    /// <param name="name">候補者名</param>
    /// <returns>先月のグループメンバー</returns>
    public List<string> GetLastMonthGroupMembers(string name)
    {
        if(lastMonthNames.Contains(name))
        {
            return lastMonthNames;
        }
        return null;
    }

    /// <summary>
    /// 今月のメンバーと先月のメンバーに被りがないか判定
    /// </summary>
    /// <param name="lastManthMembers">先月同じグループだったメンバー</param>
    /// <returns>被りがある場合 true</returns>
    public bool IsSameMember(List<string> lastManthMembers)
    {
        // 今月のメンバーに、先月のメンバーがいないかチェック
        bool isSame = false;
        foreach (string mem in memberNames)
        {
            if(lastManthMembers.Contains(mem))
            {
                // Debug.Log(string.Format("--- {0} is same member. gno:{1} ---", mem, GroupNo));
                isSame = true;
                break;
            }
        }
        return isSame;
    }

    /// <summary>
    /// PlayerPrefs の Key を取得
    /// </summary>
    /// <param name="date">日付（年月）</param>
    /// <returns>Key 文字列</returns>
    private string getKey(System.DateTime date)
    {
        return string.Format("group_{0}_{1}_{2}", GroupNo, date.Year, date.Month);
    }

}
