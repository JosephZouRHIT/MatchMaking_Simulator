using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using UnityEngine;
using UnityEngine.UI;

public class DungeonInfo : MonoBehaviour {

    private DBConnection conn;
    private string dungeonName;
    private Text dungeonNameStr;
    private Text location;
    private Text description;
    private Text powerlevel;
	// Use this for initialization
	void Start () {
        conn = GameObject.Find("Connection").GetComponent<DBConnection>();
        dungeonNameStr = transform.Find("Name").GetComponent<Text>();
        location = transform.Find("Location").GetComponent<Text>();
        description = transform.Find("Description").GetComponent<Text>();
        powerlevel = transform.Find("Powerlevel").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void updateDungeonInfo(string dungeonName)
    { 
        SqlCommand comm = new SqlCommand("select * from dungeon_Info(@DungeonName)", conn.getConnection());
        SqlParameter dName = comm.Parameters.Add("@DungeonName", System.Data.SqlDbType.VarChar);
        dName.Value = dungeonName;
        SqlDataReader reader = comm.ExecuteReader();
        string nameStr = "";
        string locationStr = "";
        string descriptionStr = "";
        string powerlevelStr = "";
        if (reader.Read())
        {
            nameStr = reader["DungeonName"].ToString();
            locationStr = reader["Location"].ToString();
            descriptionStr = reader["Description"].ToString();
            powerlevelStr = reader["Powerlevel"].ToString();
        }
        reader.Close();
        dungeonNameStr.text = nameStr;
        location.text = locationStr;
        description.text = descriptionStr;
        powerlevel.text = powerlevelStr;
        conn.SetDungeon(int.Parse(powerlevelStr));
        conn.SetDungeonName(nameStr);
    }

    public void clear()
    {
        dungeonNameStr.text = "";
        location.text = "";
        description.text = "";
        powerlevel.text = "";
    }
}
