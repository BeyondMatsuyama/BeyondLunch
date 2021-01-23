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
    public void SetMember(string name)
    {
        memberNames.Add(name);
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
    public void Load(System.DateTime date)
    {
        string key = getKey(date);
        string val = PlayerPrefs.GetString(key);
        string[] members = val.Split(' ');

        // Debug.Log("--- Load[" + GroupNo + "] : " + val + "---");
        for (int i = 0; i < members.Length; i++)
        {
            // Debug.Log("  val : " + members[i]);
            SetMember(members[i]);
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
    /// PlayerPrefs の Key を取得
    /// </summary>
    /// <param name="date">日付（年月）</param>
    /// <returns>Key 文字列</returns>
    private string getKey(System.DateTime date)
    {
        return string.Format("group_{0}_{1}_{2}", GroupNo, date.Year, date.Month);
    }

}
