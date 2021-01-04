using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupBoard : MonoBehaviour
{
    [SerializeField] private GameObject objBoard;
    [SerializeField] private List<GameObject> objList = new List<GameObject>();
    [SerializeField] private List<Text> txtName = new List<Text>();

    private List<string> memberNames = new List<string>();

    /// <summary>
    /// 起動時処理
    /// </summary>
    void Start()
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
        int mid = 0;
        objBoard.SetActive(true);
        for(int i=0; i<memberNames.Count; i++)
        {
            objList[mid].SetActive(true);
            txtName[mid].text = memberNames[mid];
            mid++;
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

}
