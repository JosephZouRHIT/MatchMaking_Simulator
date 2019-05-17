using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Data.SqlClient;

public class UpdateEquipment : MonoBehaviour {

    private Button btn;
    private DBConnection dbService;
	// Use this for initialization
	void Start () {
        btn = GetComponent<Button>();
        dbService = GameObject.Find("Connection").GetComponent<DBConnection>();
        btn.onClick.AddListener(GrabEquipment);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void GrabEquipment()
    {
        GetEquipment get = GameObject.Find("Canvas").transform.Find("EquipmentDetail").Find("Viewport").Find("Content").GetComponent<GetEquipment>();
        List<GameObject> entries = get.GetEntries();
        SqlCommand comm = new SqlCommand("add_Owns", dbService.getConnection());
        comm.CommandType = System.Data.CommandType.StoredProcedure;
        SqlParameter username = comm.Parameters.Add("@Username", System.Data.SqlDbType.VarChar);
        username.Value = dbService.GetUsername();
        SqlParameter equipmentName = comm.Parameters.Add("@EquipmentName", System.Data.SqlDbType.VarChar);
        SqlParameter rarity = comm.Parameters.Add("@Rarity", System.Data.SqlDbType.VarChar);
        foreach(GameObject obj in entries)
        {
            Toggle temp = obj.GetComponent<Toggle>();
            if (temp.isOn)
            {
                equipmentName.Value = obj.transform.Find("Label").Find("Name").GetComponent<Text>().text;
                rarity.Value = obj.transform.Find("Label").Find("Rarity").GetComponent<Text>().text;
                comm.ExecuteNonQuery();
            }
        }
        comm = new SqlCommand("Level_Up", dbService.getConnection());
        comm.CommandType = System.Data.CommandType.StoredProcedure;
        username = comm.Parameters.Add("@Username", System.Data.SqlDbType.VarChar);
        username.Value = dbService.GetUsername();
        comm.ExecuteNonQuery();
        dbService.SetDungeonName("");
        DontDestroyOnLoad(dbService);
        SceneManager.LoadScene("DisplayDungeon");
    }
}
