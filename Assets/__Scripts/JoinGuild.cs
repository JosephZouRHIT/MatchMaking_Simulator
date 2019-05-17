using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data.SqlClient;

public class JoinGuild : MonoBehaviour {


    private Button btn;
    private MemberInGuildList list;
    private DBConnection dbService;
	// Use this for initialization
	void Start () {
        list = GameObject.Find("Canvas/MemberList/Viewport/Content").GetComponent<MemberInGuildList>();
        btn = GetComponent<Button>();
        dbService = GameObject.Find("Connection").GetComponent<DBConnection>();
        btn.onClick.AddListener(GetInGuild);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void GetInGuild()
    {
        Dropdown dropdown = GameObject.Find("Canvas/GuildList").GetComponent<Dropdown>();
        SqlCommand comm = new SqlCommand("join_Guild", dbService.getConnection());
        comm.CommandType = System.Data.CommandType.StoredProcedure;
        SqlParameter username = comm.Parameters.Add("@username", System.Data.SqlDbType.VarChar);
        SqlParameter guildName = comm.Parameters.Add("@GuildName", System.Data.SqlDbType.VarChar);
        username.Value = dbService.GetUsername();
        guildName.Value = dropdown.options[dropdown.value].text;
        comm.ExecuteNonQuery();
        list.UpdateMemberList(dropdown.options[dropdown.value].text);
    }
}
