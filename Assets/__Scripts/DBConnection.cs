using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

public class DBConnection : MonoBehaviour {

    public TextAsset config;

    private SqlConnection conn;
    private RNGCryptoServiceProvider random;
    private SHA256 hasher;
    private string username = "";
    private string dungeonName = "";
    private int teamID = -1;
    private string serverName = "";
    private int dungeonPower = 0;
    // Use this for initialization
    void Awake () {
        string temp = config.text;
        temp = temp.Replace("\r\n", ";");
        string[] tempArr = temp.Split(';');
        string connection = "";
        for(int i = 0; i < tempArr.Length; i++)
        {
            if(i == tempArr.Length - 1)
            {
                connection += @tempArr[i];
                break;
            }
            connection += tempArr[i] + ";";
        }
        conn = new SqlConnection(connection);
        random = new RNGCryptoServiceProvider();
        hasher = SHA256.Create();
        try
        {
            conn.Open();
            print("Connection OK");
        }
        catch (SqlException ex)
        {
            System.Windows.Forms.MessageBox.Show("Connection Failed");
            print(ex.StackTrace);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public SqlConnection getConnection()
    {
        return conn;
    }

    public RNGCryptoServiceProvider getRandomizer()
    {
        return random;
    }

    public string HashPassword(string saltStr, string password)
    {
        byte[] original = Encoding.UTF8.GetBytes(string.Concat(password, saltStr));
        byte[] hashed = hasher.ComputeHash(original);
        return Encoding.ASCII.GetString(hashed);
    }

    private void OnApplicationQuit()
    {
        conn.Close();
    }

    public void SetUsername(string usernameStr)
    {
        username = usernameStr;
    }

    public string GetUsername()
    {
        return username;
    }

    public void SetDungeon(int dungeon)
    {
        dungeonPower = dungeon;
    }

    public void SetTeam(int teamID)
    {
        this.teamID = teamID;
    }

    public int GetTeam()
    {
        return teamID;
    }

    public int GetDungeon()
    {
        return dungeonPower;
    }

    public void SetServerName(string serverName)
    {
        this.serverName = serverName;
    }

    public string GetServerName()
    {
        return this.serverName;
    }

    public void SetDungeonName(string dungeonName)
    {
        this.dungeonName = dungeonName;
    }

    public string GetDungeonName()
    {
        return this.dungeonName;
    }
}
