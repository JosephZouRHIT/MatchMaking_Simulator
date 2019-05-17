using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data.SqlClient;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class StartMatchMaking : MonoBehaviour {

    private GameObject timerPanel;
    private Timer timer;
    private Button btn;
    private DBConnection dbService;
	// Use this for initialization
	void Start () {
        btn = GetComponent<Button>();
        timerPanel = GameObject.Find("Canvas").transform.Find("MatchmakingTimer").gameObject;
        timer = timerPanel.transform.Find("Time").GetComponent<Timer>();
        dbService = GameObject.Find("Connection").GetComponent<DBConnection>();
        btn.onClick.AddListener(FindTeam);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FindTeam()
    {
        if(dbService.GetDungeonName() == "")
        {
            System.Windows.Forms.MessageBox.Show("Please Select a Dungeon");
            return;
        }
        SqlCommand getPowerlevel = new SqlCommand("select Powerlevel from Player where Username = @username", dbService.getConnection());
        SqlParameter username = getPowerlevel.Parameters.Add("@username", System.Data.SqlDbType.VarChar);
        username.Value = dbService.GetUsername();
        SqlDataReader reader = getPowerlevel.ExecuteReader();
        reader.Read();
        int playerPower = int.Parse(reader["Powerlevel"].ToString());
        reader.Close();
        SqlCommand getTeamMember = new SqlCommand("Select * from Match_Making(@MaxPowerlevel, @MinPowerlevel, @ServerName) where TeamID is null", dbService.getConnection());
        SqlParameter maxPowerlevel = getTeamMember.Parameters.Add("@MaxPowerlevel", System.Data.SqlDbType.Int);
        SqlParameter minPowerlevel = getTeamMember.Parameters.Add("@MinPowerlevel", System.Data.SqlDbType.Int);
        SqlParameter serverName = getTeamMember.Parameters.Add("@ServerName", System.Data.SqlDbType.VarChar);
        maxPowerlevel.Value = playerPower + 30;
        int min = playerPower - 30;
        if(min < 0)
        {
            min = 0;
        }
        minPowerlevel.Value = min;
        serverName.Value = dbService.GetServerName();
        reader = getTeamMember.ExecuteReader();
        List<string> players = new List<string>();
        while (reader.Read())
        {
            players.Add(reader["Username"].ToString());
        }
        reader.Close();
        SqlCommand getTeamID = new SqlCommand("add_Team", dbService.getConnection());
        getTeamID.CommandType = System.Data.CommandType.StoredProcedure;
        serverName = getTeamID.Parameters.Add("@ServerName", System.Data.SqlDbType.VarChar);
        SqlParameter dungeonName = getTeamID.Parameters.Add("@Dungeon", System.Data.SqlDbType.VarChar);
        SqlParameter teamID = getTeamID.Parameters.Add("@TeamID", System.Data.SqlDbType.Int);
        dungeonName.Value = dbService.GetDungeonName();
        serverName.Value = dbService.GetServerName();
        teamID.Direction = System.Data.ParameterDirection.Output;
        getTeamID.ExecuteNonQuery();
        dbService.SetTeam(int.Parse(teamID.Value.ToString()));
        Random random = new Random();
        List<string> teamMembers = new List<string>();
        teamMembers.Add(dbService.GetUsername());
        while(true)
        {
            if(players.Count < 4)
            {
                System.Windows.Forms.MessageBox.Show("No suitable member found");
                return;
            }
            int tempindex = (int)(random.NextDouble() * players.Count);
            string temp = players[tempindex];
            if(temp != dbService.GetUsername())
            {
                players.RemoveAt(tempindex);
                teamMembers.Add(temp);
            }
            if (teamMembers.Count >= 4) break;
        }
        foreach(string name in teamMembers)
        {
            SqlCommand joinTeam = new SqlCommand("update Player set TeamID = @TeamID where username = @Username", dbService.getConnection());
            teamID = joinTeam.Parameters.Add("@TeamID", System.Data.SqlDbType.Int);
            username = joinTeam.Parameters.Add("@Username", System.Data.SqlDbType.VarChar);
            teamID.Value = dbService.GetTeam();
            username.Value = name;
            joinTeam.ExecuteNonQuery();
        }
        int time = (int)(random.NextDouble() * 80 + 10);
        StartTimer(time);
    }

    private void StartTimer(int time)
    {
        timerPanel.SetActive(true);
        timer.SetTime(time);
    }
}
