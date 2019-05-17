using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Text;
using System.Security.Cryptography;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour {
    [Header("Set in Inspector")]
    public InputField username;
    public InputField password;

    private Button login;
    private DBConnection dbService;
    // Use this for initialization
    void Start () {
        dbService = GameObject.Find("Connection").GetComponent<DBConnection>();
        login = GetComponent<Button>();
        login.onClick.AddListener(CallLogin);
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    private void CallLogin()
    {
            SqlCommand comm = new SqlCommand("getHash", dbService.getConnection());
            comm.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter usernameStr = comm.Parameters.Add("@username", System.Data.SqlDbType.VarChar);
            usernameStr.Value = username.text;
            string saltStr = "";
            string passHash = "";
            SqlDataReader reader = comm.ExecuteReader();
            if (reader.Read())
            {
                saltStr = reader["Salt"].ToString();
                passHash = reader["PassHash"].ToString();
            }
            reader.Close();
            if(passHash.CompareTo(dbService.HashPassword(saltStr, password.text)) != 0)
            {
                print("Login Failed");
            }
            else
            {
                print("Login Success");
                DontDestroyOnLoad(dbService);
            dbService.SetUsername(username.text);
            DontDestroyOnLoad(dbService);
            SceneManager.LoadScene("PlayerInfo");
            }
        
    }
}
