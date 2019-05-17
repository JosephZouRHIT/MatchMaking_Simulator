using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data.SqlClient;

public class ServerList : MonoBehaviour {

    private Dropdown dropdown;
    private DBConnection dbService;
    private List<string> servers;
    private Transform label;
	// Use this for initialization
	void Start () {
        dropdown = GetComponent<Dropdown>();
        dbService = GameObject.Find("Connection").GetComponent<DBConnection>();
        servers = new List<string>();
        label = dropdown.transform.Find("Label");
        UpdateServer();
    }
	
	// Update is called once per frame
	void Update () {
        if(dropdown.options.Capacity != 0)
        {
            label.GetComponent<Text>().text = dropdown.options[dropdown.value].text;
        }
	}

    private void UpdateServer()
    {
        dropdown.options.Clear();
        GetServers();
        foreach (string str in servers)
        {
            dropdown.options.Add(new Dropdown.OptionData(str));
        }
    }

    private void GetServers()
    {
        SqlCommand comm = new SqlCommand("select [Name] from GameServer", dbService.getConnection());
        SqlDataReader reader = comm.ExecuteReader();
        while (reader.Read())
        {
            servers.Add(reader["Name"].ToString());
        }
        reader.Close();
    }
}
