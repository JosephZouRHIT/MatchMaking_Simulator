using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data.SqlClient;
using UnityEngine.SceneManagement;

public class CreatePlayer : MonoBehaviour {

    public Dropdown className;
    public Dropdown serverName;

    private DBConnection dbService;
    private Button btn;
	// Use this for initialization
	void Start () {
        dbService = GameObject.Find("Connection").GetComponent<DBConnection>();
        btn = GetComponent<Button>();
        btn.onClick.AddListener(NewPlayer);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void NewPlayer()
    {
        SqlCommand comm = new SqlCommand("new_Player", dbService.getConnection());
        comm.CommandType = System.Data.CommandType.StoredProcedure;
        SqlParameter returnValue = comm.Parameters.Add("@ReturnValue", System.Data.SqlDbType.Int);
        SqlParameter username = comm.Parameters.Add("@username", System.Data.SqlDbType.VarChar);
        SqlParameter classInput = comm.Parameters.Add("@Class", System.Data.SqlDbType.VarChar);
        SqlParameter serverInput = comm.Parameters.Add("@serverName", System.Data.SqlDbType.VarChar);

        returnValue.Direction = System.Data.ParameterDirection.ReturnValue;
        username.Value = dbService.GetUsername();
        string classStr = className.options[className.value].text;
        string serverStr = serverName.options[serverName.value].text;
        if (serverStr.CompareTo("None") == 0)
        {
            print("Need to Select a Server");
            return;
        }else if(classStr.CompareTo("None") == 0)
        {
            print("Need to Select a Class");
            return;
        }
        classInput.Value = classStr;
        serverInput.Value = serverStr;
        comm.ExecuteNonQuery();

        if((int) returnValue.Value == 4)
        {
            print("A player already exists for user");
        }else if((int) returnValue.Value == 0)
        {
            print("Player successfully created");
        }
        else
        {
            print("Failed to create Player");
        }

        DontDestroyOnLoad(dbService);
        SceneManager.LoadScene("PlayerInfo");
    }
}
