using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data.SqlClient;

public class CreateGuild : MonoBehaviour {


    private Button btn;
    private InputField name;
    private DBConnection dbService;
    private GuildList dropdown;
	// Use this for initialization
	void Start () {
        btn = GetComponent<Button>();
        dropdown = GameObject.Find("Canvas/GuildList").GetComponent<GuildList>();
        dbService = GameObject.Find("Connection").GetComponent<DBConnection>();
        name = transform.parent.Find("GuildName").GetComponent<InputField>();
        btn.onClick.AddListener(GuildCreation);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void GuildCreation()
    {
        SqlCommand comm = new SqlCommand("select g.Name from GameServer g join Player p on p.ServerID = g.ServerID where p.Username = @Username", dbService.getConnection());
        SqlParameter username = comm.Parameters.Add("@Username", System.Data.SqlDbType.VarChar);
        username.Value = dbService.GetUsername();
        SqlDataReader reader = comm.ExecuteReader();
        string serverName = "";
        if (reader.Read())
        {
            serverName = reader["Name"].ToString();
        }
        reader.Close();
        comm = new SqlCommand("insert_Guild", dbService.getConnection());
        comm.CommandType = System.Data.CommandType.StoredProcedure;
        SqlParameter guildName = comm.Parameters.Add("@Name", System.Data.SqlDbType.VarChar);
        SqlParameter serverNameStr = comm.Parameters.Add("@ServerName", System.Data.SqlDbType.VarChar);
        guildName.Value = name.text;
        serverNameStr.Value = serverName;
        comm.ExecuteNonQuery();
        comm = new SqlCommand("join_Guild", dbService.getConnection());
        comm.CommandType = System.Data.CommandType.StoredProcedure;
        guildName = comm.Parameters.Add("@GuildName", System.Data.SqlDbType.VarChar);
        username = comm.Parameters.Add("@username", System.Data.SqlDbType.VarChar);
        guildName.Value = name.text;
        username.Value = dbService.GetUsername();
        comm.ExecuteNonQuery();
        dropdown.GetGuilds();
        dropdown.ChangeToCurrentGuild();
        name.text = "";
        Destroy(transform.parent.gameObject);
    }
}
