using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Data.SqlClient;

public class CancelExploration : MonoBehaviour {

    private Button btn;
    private DBConnection dbService;
	// Use this for initialization
	void Start () {
        btn = GetComponent<Button>();
        dbService = GameObject.Find("Connection").GetComponent<DBConnection>();
        btn.onClick.AddListener(Cancel);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Cancel()
    {
        SqlCommand comm = new SqlCommand("delete_Team", dbService.getConnection());
        comm.CommandType = System.Data.CommandType.StoredProcedure;
        SqlParameter teamID = comm.Parameters.Add("@TeamID", System.Data.SqlDbType.Int);
        teamID.Value = dbService.GetTeam();
        comm.ExecuteNonQuery();
        dbService.SetTeam(-1);
        dbService.SetDungeon(0);
        dbService.SetDungeonName("");
        DontDestroyOnLoad(dbService);
        SceneManager.LoadScene("DisplayDungeon");
    }
}
