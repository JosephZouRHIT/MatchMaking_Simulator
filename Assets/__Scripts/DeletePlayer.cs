using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Data.SqlClient;

public class DeletePlayer : MonoBehaviour {

    private Button btn;
    private DBConnection dbService;
	// Use this for initialization
	void Start () {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(PlayerDeletion);
        dbService = GameObject.Find("Connection").GetComponent<DBConnection>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void PlayerDeletion()
    {
        SqlCommand comm = new SqlCommand("delete_player", dbService.getConnection());
        comm.CommandType = System.Data.CommandType.StoredProcedure;
        SqlParameter username = comm.Parameters.Add("@username", System.Data.SqlDbType.VarChar);
        username.Value = dbService.GetUsername();
        comm.ExecuteNonQuery();
        SceneManager.LoadScene("LoginPage");
    }
}
