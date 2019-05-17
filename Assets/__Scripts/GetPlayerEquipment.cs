using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data.SqlClient;

public class GetPlayerEquipment : MonoBehaviour {

    public GameObject entry;
    private DBConnection dbService;
	// Use this for initialization
	void Start () {
        dbService = GameObject.Find("Connection").GetComponent<DBConnection>();
        SqlCommand comm = new SqlCommand("select * from player_Equipments(@username)", dbService.getConnection());
        SqlParameter username = comm.Parameters.Add("@username", System.Data.SqlDbType.VarChar);
        username.Value = dbService.GetUsername();
        SqlDataReader reader = comm.ExecuteReader();
        while (reader.Read())
        {
            GameObject temp = Instantiate(entry, GameObject.Find("Canvas").transform.Find("EquipmentInfo").Find("Viewport").Find("Content"));
            temp.transform.Find("Name").GetComponent<Text>().text = reader["Name"].ToString();
            temp.transform.Find("Rarity").GetComponent<Text>().text = reader["Rarity"].ToString();
            temp.transform.Find("Class").GetComponent<Text>().text = reader["Class"].ToString();
            temp.transform.Find("Powerlevel").GetComponent<Text>().text = reader["Powerlevel"].ToString();
            temp.transform.Find("Part").GetComponent<Text>().text = reader["part"].ToString();
        }
        reader.Close();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
