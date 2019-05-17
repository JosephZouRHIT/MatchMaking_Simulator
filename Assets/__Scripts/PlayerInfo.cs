using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data.SqlClient;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerInfo : MonoBehaviour {

    public GameObject errorPrefab;

    private DBConnection dbService;
    private Text username;
    private Text className;
    private Text guildName;
    private Text serverName;
    private Text powerlevel;
	// Use this for initialization
	void Start () {
        dbService = GameObject.Find("Connection").GetComponent<DBConnection>();
        username = transform.Find("Name").GetComponent<Text>();
        className = transform.Find("Class").GetComponent<Text>();
        guildName = transform.Find("GuildName").GetComponent<Text>();
        serverName = transform.Find("ServerName").GetComponent<Text>();
        powerlevel = transform.Find("Powerlevel").GetComponent<Text>();
        if (CheckExistingPlayer(dbService.GetUsername()))
        {
            print("Player found");
        }
        else
        { 
            GameObject action = Instantiate(errorPrefab, GameObject.Find("Canvas").transform);
            action.transform.Find("ErrorMessage").GetComponent<Text>().text = "You don't have a player yet. Please create one";
            action.transform.Find("Action").GetComponentInChildren<Text>().text = "Go Create";
            action.transform.Find("Action").GetComponent<Button>().onClick.AddListener(LoadCreatePlayer);
            print("No Player found, need to create one");
        }
	}
	
	// Update is called once per frame
	public bool CheckExistingPlayer(string name)
    {
        string query = "select * from Player_Info(@username)";
        SqlCommand comm = new SqlCommand(query, dbService.getConnection());
        SqlParameter userName = comm.Parameters.Add("@username", System.Data.SqlDbType.VarChar);
        username.text = name;
        userName.Value = name;
        SqlDataReader reader = comm.ExecuteReader();
        if (reader.Read())
        {
            className.text = reader["Class"].ToString();
            serverName.text = reader["Name"].ToString();
            powerlevel.text = reader["Powerlevel"].ToString();
            dbService.SetServerName(serverName.text);
            if(reader["GuildName"] == null || reader["GuildName"].ToString() == "")
            {
                guildName.text = "Not in Guild";
            }
            else
            {
                guildName.text = reader["GuildName"].ToString();
            }
            reader.Close();
            return true;
        }
        reader.Close();
        return false;
    }

    public void Clear()
    {
        className.text = "";
        serverName.text = "";
        powerlevel.text = "";
        username.text = "";
        guildName.text = "";
    }

    private void LoadCreatePlayer()
    {
        DontDestroyOnLoad(dbService);
        SceneManager.LoadScene("PlayerCreation");
    }
}
