using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data.SqlClient;

public class LeaveGuild : MonoBehaviour {

    private Button btn;
    private Dropdown dropdown;
    private DBConnection dbService;
	// Use this for initialization
	void Start () {
        btn = GetComponent<Button>();
        dropdown = GameObject.Find("Canvas/GuildList").GetComponent<Dropdown>();
        dbService = GameObject.Find("Connection").GetComponent<DBConnection>();
        btn.onClick.AddListener(QuitGuild);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void QuitGuild()
    {
        SqlCommand comm = new SqlCommand("update Player set GuildID = null where Username = @Username", dbService.getConnection());
        SqlParameter username = comm.Parameters.Add("@Username", System.Data.SqlDbType.VarChar);
        username.Value = dbService.GetUsername();
        comm.ExecuteNonQuery();
        dropdown.value = 0;
    }
}
