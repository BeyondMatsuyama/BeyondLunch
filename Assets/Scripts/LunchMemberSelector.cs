using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LunchMemberSelector : MonoBehaviour
{
    // ボタン群
    [SerializeField] private Button btnSelect;
    [SerializeField] private Button btnClear;
    [SerializeField] private Button btnPrevMonth;
    [SerializeField] private Button btnNextMonth;

    // テキスト
    [SerializeField] private Text txtYear;
    [SerializeField] private Text txtMonth;

    // オブジェクト
    [SerializeField] private Transform  parentBoard;
    [SerializeField] private GameObject prefabBoard;

    private BeyondMember beyond;    // 社員リスト
    private int groupNum;
    private List<GroupBoard> boardList = new List<GroupBoard>();

    private System.DateTime curDate;    

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        initialize();

        // 前月
        btnPrevMonth.onClick.AddListener(() =>
        {
            curDate = curDate.AddMonths(-1);
            updateDate();
        });

        // 翌月
        btnNextMonth.onClick.AddListener(() =>
        {
            curDate = curDate.AddMonths(1);
            updateDate();
        });

        // クリア
        btnClear.onClick.AddListener(() =>
        {
            PlayerPrefs.DeleteAll();
        });

        // 抽選
        btnSelect.onClick.AddListener(() =>
        {
            eraseBoard();       // ボード消去
            setLeader();        // リーダー設定
            lotteryMember();    // メンバー抽選
            saveMember();       // メンバー保存
            showBoard();        // 表示
        });
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void initialize()
    {
        loadEmployees();                // 社員名読み込み
        initBoard();                    // インスタンス生成・配置

        // カレンダー初期化
        curDate = System.DateTime.Now;
        updateDate();
    }

    // カレンダー更新
    private void updateDate()
    {
        // 年月更新
        txtYear.text = curDate.Year.ToString();
        txtMonth.text = curDate.Month.ToString();

        // データあり
        if (boardList[0].IsExist(curDate))
        {
            eraseBoard();   // ボード消去
            loadMember();   // メンバー読み込み
            showBoard();    // 表示
        }
        // データなし
        else
        {
            eraseBoard();   // 消去
        }
    }
    

    /// <summary>
    /// 社員名読み込み
    /// </summary>
    private void loadEmployees()
    {
        // scriptable object から社員名を取得
        beyond = Resources.Load<BeyondMember>("Beyond Member");
        // グループ数（= リーダー数）
        groupNum = beyond.leaderList.Count;
    }

    /// <summary>
    /// ボード初期化
    /// </summary>
    private void initBoard()
    {
        for(int i=0; i<groupNum; i++)
        {
            GameObject obj = Instantiate(prefabBoard, parentBoard);
            GroupBoard board = obj.GetComponent<GroupBoard>();
            board.GroupNo = i;
            boardList.Add(board);
        }
    }

    /// <summary>
    /// ボード表示
    /// </summary>
    private void showBoard()
    {
        foreach (GroupBoard board in boardList)
        {
            board.Show();
        }
    }

    /// <summary>
    /// ボード消去
    /// </summary>
    private void eraseBoard()
    {
        foreach(GroupBoard board in boardList)
        {
            board.Initialize();
        }
    }

    /// <summary>
    /// メンバーを保存
    /// </summary>
    private void saveMember()
    {
        foreach(GroupBoard board in boardList)
        {
            board.Save(curDate);
        }
    }

    /// <summary>
    /// メンバーを読み込み
    /// </summary>
    private void loadMember()
    {
        foreach(GroupBoard board in boardList)
        {
            board.Load(curDate);
        }
    }

    /// <summary>
    /// リーダー設定
    /// </summary>
    private void setLeader()
    {
        for(int i=0; i<groupNum; i++)
        {
            GroupBoard board = boardList[i];
            board.SetMember(beyond.leaderList[i]);
            // Debug.Log(string.Format("leader[{0}]:{1}", i, beyond.leaderList[i]));
        }
    }

    /// <summary>
    /// メンバーのランダム抽選
    /// </summary>
    private void lotteryMember()
    {
        int gno = Random.Range(0, groupNum - 1);    // 起点となるグループ番号
        List<string> members = new List<string>(beyond.membeList);
        do
        {
            int num = members.Count;
            int mid = Random.Range(0, num);
            boardList[gno].SetMember(members[mid]);
            // Debug.Log(string.Format("member[{0}]:{1}", gno, members[mid]));
            members.RemoveAt(mid);
            gno++;
            if (gno >= groupNum) gno = 0;
        } while (members.Count > 0);
    }

}
