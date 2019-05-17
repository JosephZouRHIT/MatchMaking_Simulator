using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Data.SqlClient;

public class CountdownTimer : MonoBehaviour {

    private float tgtTime;
    private float time;
    private float previousTime;
    private Text txt;
    private int minute = 0;
    private int second = 0;
    private DBConnection dbService;
    // Use this for initialization
    void Start () {
        dbService = GameObject.Find("Connection").GetComponent<DBConnection>();
        txt = GetComponent<Text>();
        time = 0f;
        tgtTime = 0f;
        previousTime = 0f;
        GetTime();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (tgtTime > 0f)
        {
            if (previousTime - time >= 1)
            {
                second--;
                if(second == -1)
                {
                    second = 59;
                }
                if(second != 0)
                {
                    minute = (int)(time / 60);
                }
                previousTime = time;
                UpdateString();
            }
            time -= Time.deltaTime;
            if (time <= 0)
            {
                SetTime(0f);
                LoadNext();
            }
        }
    }

    private void GetTime()
    {
        SqlCommand comm = new SqlCommand("select m.Class, sum(c.amount) as amount from [Contains] c join Monster m on m.MonsterID = c.MonsterID join Dungeon d on d.DungeonID = c.DungeonID where DungeonName = @DungeonName group by m.Class order by amount desc", dbService.getConnection());
        SqlParameter dungeonName = comm.Parameters.Add("@DungeonName", System.Data.SqlDbType.VarChar);
        dungeonName.Value = dbService.GetDungeonName();
        SqlDataReader reader = comm.ExecuteReader();
        string dungeonClass = "";
        while (reader.Read())
        {
            if(reader["Class"].ToString() != "Ruler")
            {
                dungeonClass = reader["Class"].ToString();
                break;
            }
        }
        reader.Close();
        comm = new SqlCommand("select Class, count(Username) as amount from Player where TeamID = @TeamID group by Class", dbService.getConnection());
        SqlParameter teamID = comm.Parameters.Add("@TeamID", System.Data.SqlDbType.Int);
        teamID.Value = dbService.GetTeam();
        reader = comm.ExecuteReader();
        Dictionary<string, int> classCount = new Dictionary<string, int>();
        while (reader.Read())
        {
            if(reader["Class"].ToString() != "Ruler")
            {
                classCount.Add(reader["Class"].ToString(), int.Parse(reader["amount"].ToString()));
            }
        }
        reader.Close();
        float timeC = 1.1f;
        switch (dungeonClass)
        {
            case "Saber":
                foreach(string str in classCount.Keys)
                {
                    if(str == "Lancer")
                    {
                        timeC -= 0.1f * classCount[str];
                    }
                    if(str == "Archer")
                    {
                        timeC += 0.1f * classCount[str];
                    }
                }
                break;
            case "Lancer":
                foreach (string str in classCount.Keys)
                {
                    if (str == "Archer")
                    {
                        timeC -= 0.1f * classCount[str];
                    }
                    if (str == "Saber")
                    {
                        timeC += 0.1f * classCount[str];
                    }
                }
                break;
            case "Archer":
                foreach (string str in classCount.Keys)
                {
                    if (str == "Saber")
                    {
                        timeC -= 0.1f * classCount[str];
                    }
                    if (str == "Lancer")
                    {
                        timeC += 0.1f * classCount[str];
                    }
                }
                break;
            default:
                break;
        }
        comm = new SqlCommand("select sum(amount) as amount from [Contains] c join Dungeon d on d.DungeonID = c.DungeonID where DungeonName = @DungeonName group by DungeonName", dbService.getConnection());
        dungeonName = comm.Parameters.Add("@DungeonName", System.Data.SqlDbType.VarChar);
        dungeonName.Value = dbService.GetDungeonName();
        reader = comm.ExecuteReader();
        reader.Read();
        int time = (int)(int.Parse(reader["amount"].ToString()) * 10/ timeC);
        reader.Close();
        print(time);
        SetTime(time);
    }

    private void UpdateString()
    {
        string minuteStr = minute.ToString();
        string secondStr = second.ToString();
        if (minuteStr.Length < 2)
        {
            minuteStr = "0" + minuteStr;
        }
        if (secondStr.Length < 2)
        {
            secondStr = "0" + secondStr;
        }
        txt.text = minuteStr + ":" + secondStr;
    }

    public void SetTime(float target)
    {
        tgtTime = target;
        time = target;
        previousTime = target;
        minute = (int)(target / 60);
        second = (int)(target % 60);
        UpdateString();
    }

    private void LoadNext()
    {
        SqlCommand comm = new SqlCommand("delete_Team", dbService.getConnection());
        comm.CommandType = System.Data.CommandType.StoredProcedure;
        SqlParameter teamID = comm.Parameters.Add("@TeamID", System.Data.SqlDbType.Int);
        teamID.Value = dbService.GetTeam();
        comm.ExecuteNonQuery();
        dbService.SetTeam(-1);
        dbService.SetDungeon(0);
        DontDestroyOnLoad(GameObject.Find("Connection"));
        SceneManager.LoadScene("EquipmentClaim");
    }
}
