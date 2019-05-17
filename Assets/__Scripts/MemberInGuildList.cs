using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data.SqlClient;

public class MemberInGuildList : MonoBehaviour {

    public GameObject entry;

    private List<GameObject> entries;
    private DBConnection dbService;
	// Use this for initialization
	void Start () {
        entries = new List<GameObject>();
        dbService = GameObject.Find("Connection").GetComponent<DBConnection>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Clear()
    {
        foreach(GameObject obj in entries)
        {
            Destroy(obj);
        }
        entries = new List<GameObject>();
    }

    public void UpdateMemberList(string guildName)
    {
        SqlCommand comm = new SqlCommand("select * from Member_In_Guild(@username, @GuildName) order by Powerlevel desc", dbService.getConnection());
        SqlParameter username = comm.Parameters.Add("@username", System.Data.SqlDbType.VarChar);
        SqlParameter guildNameStr = comm.Parameters.Add("@GuildName", System.Data.SqlDbType.VarChar);
        guildNameStr.Value = guildName;
        username.Value = dbService.GetUsername();
        SqlDataReader reader = comm.ExecuteReader();
        while (reader.Read())
        {
            GameObject temp = Instantiate(entry, GameObject.Find("Canvas").transform.Find("MemberList").Find("Viewport").Find("Content"));
            //temp.transform.SetParent(GameObject.Find("Canvas").transform.Find("MonsterInfo").Find("Viewport").Find("content"));
            temp.transform.Find("Name").GetComponent<Text>().text = reader["Name"].ToString();
            temp.transform.Find("Class").GetComponent<Text>().text = reader["Class"].ToString();
            temp.transform.Find("Powerlevel").GetComponent<Text>().text = reader["Powerlevel"].ToString();
            entries.Add(temp);
        }
        reader.Close();
    }
}
