using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data.SqlClient;

public class GuildList : MonoBehaviour {

    private Dropdown dropdown;
    private DBConnection dbService;
    private MemberInGuildList members;
    private Transform label;
	// Use this for initialization
	void Start () {
        dropdown = GetComponent<Dropdown>();
        label = dropdown.transform.Find("Label");
        dbService = GameObject.Find("Connection").GetComponent<DBConnection>();
        members = GameObject.Find("Canvas/MemberList/Viewport/Content").GetComponent<MemberInGuildList>();
        GetGuilds();
        dropdown.onValueChanged.AddListener(delegate { UpdateDisplay(); });
        ChangeToCurrentGuild();
    }

    // Update is called once per frame
    void Update()
    {
        if (dropdown.options.Capacity != 0)
        {
            label.GetComponent<Text>().text = dropdown.options[dropdown.value].text;
        }
    }

    public void GetGuilds()
    {
        dropdown.options.Clear();
        members.Clear();
        SqlCommand comm = new SqlCommand("select ServerID, GuildID from Player where Username = @username", dbService.getConnection());
        SqlParameter username = comm.Parameters.Add("@username", System.Data.SqlDbType.VarChar);
        username.Value = dbService.GetUsername();
        SqlDataReader reader = comm.ExecuteReader();
        reader.Read();
        string serverID = reader["ServerID"].ToString();
        reader.Close();
        comm = new SqlCommand("select GuildName from Guild where ServerID = cast(@ServerID as uniqueidentifier)", dbService.getConnection());
        SqlParameter serverIDStr = comm.Parameters.Add("@ServerID", System.Data.SqlDbType.VarChar);
        serverIDStr.Value = serverID;
        reader = comm.ExecuteReader();
        List<string> guilds = new List<string>();
        guilds.Add("None");
        while (reader.Read())
        {
            guilds.Add(reader["GuildName"].ToString());
        }
        reader.Close();
        foreach (string str in guilds)
        {
            dropdown.options.Add(new Dropdown.OptionData(str));
        }

    }

    private void UpdateDisplay()
    {
        if (dropdown.options[dropdown.value].text.CompareTo("None") == 0)
        {
            members.Clear();
        }
        else
        {
            members.Clear();
            members.UpdateMemberList(dropdown.options[dropdown.value].text);
        }
    }

    public void ChangeToCurrentGuild()
    {
        SqlCommand comm = new SqlCommand("select GuildName from Player p join Guild g on p.GuildID = g.GuildID where Username = @username", dbService.getConnection());
        SqlParameter username = comm.Parameters.Add("@username", System.Data.SqlDbType.VarChar);
        username.Value = dbService.GetUsername();
        SqlDataReader reader = comm.ExecuteReader();
        string guildName = "";
        if (reader.Read())
        {
            guildName = reader["GuildName"].ToString();
        }
        reader.Close();
        List<Dropdown.OptionData> list = dropdown.options;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].text == guildName)
            {
                dropdown.value = i;
            }
        }
    }
}
