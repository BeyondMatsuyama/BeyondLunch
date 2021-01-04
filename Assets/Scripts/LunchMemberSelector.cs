using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LunchMemberSelector : MonoBehaviour
{
    [SerializeField] private Button btnSelect;
    [SerializeField] private Transform parentBoard;
    [SerializeField] private GameObject prefabBoard;

    private BeyondMember beyond;    // 社員リスト
    private int groupNum;
    private List<GroupBoard> boardList = new List<GroupBoard>();

    // Start is called before the first frame update
    void Start()
    {
        // 社員読み込み
        loadMember();

        // インスタンス生成・配置
        initBoard();

        // 抽選
        btnSelect.onClick.AddListener(() =>
        {
            clearBoard();       // ボードクリア
            setLeader();        // リーダー設定
            lotteryMember();    // メンバー抽選
            showBoard();        // 表示
        });
    }

    /// <summary>
    /// 社員読み込み
    /// </summary>
    private void loadMember()
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
    /// ボードクリア
    /// </summary>
    private void clearBoard()
    {
        foreach(GroupBoard board in boardList)
        {
            board.Initialize();
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
