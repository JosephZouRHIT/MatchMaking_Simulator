using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data.SqlClient;
using UnityEngine.UI;

public class MonsterInfo : MonoBehaviour {

    public GameObject entry;
    private DBConnection dbservice;
    private List<GameObject> entries = new List<GameObject>();
	// Use this for initialization
	void Start () {
        dbservice = GameObject.Find("Connection").GetComponent<DBConnection>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void updateMonsterInformation(string dungeonName)
    {
        SqlCommand comm = new SqlCommand("select * from dungeon_Monster(@DungeonName) order by MonsterPowerlevel desc", dbservice.getConnection());
        SqlParameter dName = comm.Parameters.Add("@DungeonName", System.Data.SqlDbType.VarChar);
        dName.Value = dungeonName;
        SqlDataReader reader = comm.ExecuteReader();
        while (reader.Read())
        {
            GameObject temp = Instantiate(entry, GameObject.Find("Canvas").transform.Find("MonsterInfo").Find("Viewport").Find("Content"));
            //temp.transform.SetParent(GameObject.Find("Canvas").transform.Find("MonsterInfo").Find("Viewport").Find("content"));
            temp.transform.Find("Name").GetComponent<Text>().text = reader["MonsterName"].ToString();
            temp.transform.Find("Rarity").GetComponent<Text>().text = reader["MonsterRarity"].ToString();
            temp.transform.Find("Class").GetComponent<Text>().text = reader["MonsterClass"].ToString();
            temp.transform.Find("Powerlevel").GetComponent<Text>().text = reader["MonsterPowerlevel"].ToString();
            temp.transform.Find("Amount").GetComponent<Text>().text = reader["Amount"].ToString();
            entries.Add(temp);
        }
        reader.Close();
    }

    public void Clear()
    {
        foreach(GameObject obj in entries)
        {
            Destroy(obj);
        }
        entries = new List<GameObject>();
    }
}
