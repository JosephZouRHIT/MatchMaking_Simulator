using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data.SqlClient;

public class CancelTimer : MonoBehaviour {


    private Button btn;
    private GameObject timer;
    private DBConnection dbService;
	// Use this for initialization
	void Start () {
        btn = GetComponent<Button>();
        dbService = GameObject.Find("Connection").GetComponent<DBConnection>();
        btn.onClick.AddListener(RemoveTimer);
        timer = GameObject.Find("Canvas").transform.Find("MatchmakingTimer").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void RemoveTimer()
    {
        timer.SetActive(false);
        timer.transform.Find("Time").GetComponent<Timer>().SetTime(0f);
        SqlCommand comm = new SqlCommand("delete_Team", dbService.getConnection());
        comm.CommandType = System.Data.CommandType.StoredProcedure;
        SqlParameter teamID = comm.Parameters.Add("@TeamID", System.Data.SqlDbType.Int);
        teamID.Value = dbService.GetTeam();
        comm.ExecuteNonQuery();
        dbService.SetTeam(-1);
    }
}
