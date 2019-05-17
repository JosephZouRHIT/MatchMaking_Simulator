using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Text;
using System.Security.Cryptography;
using UnityEngine.SceneManagement;

public class Register : MonoBehaviour {
    [Header("Set in Inspector")]
    public InputField username;
    public InputField password;

    private Button register;
    private DBConnection dbService;
	// Use this for initialization
	void Start () {
        dbService = GameObject.Find("Connection").GetComponent<DBConnection>();
        register = GetComponent<Button>();
        register.onClick.AddListener(CallRegister);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void CallRegister()
    {
        try
        {
            SqlCommand comm = new SqlCommand("Register", dbService.getConnection());
            comm.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter returnValue = comm.Parameters.Add("@returnValue", System.Data.SqlDbType.Int);
            SqlParameter usernameStr = comm.Parameters.Add("@Username", System.Data.SqlDbType.VarChar);
            SqlParameter passSalt = comm.Parameters.Add("@Salt", System.Data.SqlDbType.VarChar);
            SqlParameter passHash = comm.Parameters.Add("@PassHash", System.Data.SqlDbType.VarChar);

            byte[] salt = new byte[32];
            dbService.getRandomizer().GetNonZeroBytes(salt);

            string saltStr = Encoding.ASCII.GetString(salt);
            passSalt.Value = saltStr;
            usernameStr.Value = username.text;
            passHash.Value = dbService.HashPassword(saltStr, password.text);
            returnValue.Direction = System.Data.ParameterDirection.ReturnValue;
            comm.ExecuteNonQuery();
            if ((int)returnValue.Value != 0)
            {
                print(string.Format("Register Failed, Error code %i", (int)returnValue.Value));
            }
            else
            {
                print("Register Success");
                dbService.SetUsername(username.text);
                DontDestroyOnLoad(dbService);
                SceneManager.LoadScene("PlayerCreation");
            }
        }catch(SqlException ex)
        {
            print(ex.StackTrace);
        }
    }
}
