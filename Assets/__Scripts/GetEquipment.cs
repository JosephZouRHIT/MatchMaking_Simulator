using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data.SqlClient;
using Random = System.Random;

public class GetEquipment : MonoBehaviour {

    public GameObject entryPrefab;

    private DBConnection dbService;

    private List<GameObject> entries;

	// Use this for initialization
	void Start () {
        dbService = GameObject.Find("Connection").GetComponent<DBConnection>();
        SqlCommand comm = new SqlCommand("select Class from Player where username = @username", dbService.getConnection());
        SqlParameter username = comm.Parameters.Add("@username", System.Data.SqlDbType.VarChar);
        username.Value = dbService.GetUsername();
        string className = "";
        SqlDataReader reader = comm.ExecuteReader();
        if (reader.Read())
        {
            className = reader["Class"].ToString();
        }
        reader.Close();

        comm = new SqlCommand("select dr.EquipmentID, dr.Rate from DropRate dr join Dungeon d on d.DungeonID = dr.DungeonID join Equipment e on e.EquipmentID = dr.EquipmentID where DungeonName = @DungeonName and Class = @ClassName", dbService.getConnection());
        SqlParameter classNameStr = comm.Parameters.Add("@ClassName", System.Data.SqlDbType.VarChar);
        SqlParameter dName = comm.Parameters.Add("@DungeonName", System.Data.SqlDbType.VarChar);
        classNameStr.Value = className;
        dName.Value = dbService.GetDungeonName();
        reader = comm.ExecuteReader();
        List<string> equipmentIDs = new List<string>();
        Random random = new Random();
        while (reader.Read())
        {
            float rate = 1 - float.Parse(reader["Rate"].ToString());
            if(random.NextDouble() >= rate)
            {
                equipmentIDs.Add(reader["EquipmentID"].ToString());
            }
        }
        reader.Close();

        comm = new SqlCommand("select * from Equipment where EquipmentID = cast(@EquipmentID as uniqueidentifier)", dbService.getConnection());
        SqlParameter equipmentID = comm.Parameters.Add("@EquipmentID", System.Data.SqlDbType.VarChar);
        entries = new List<GameObject>();
        foreach(string id in equipmentIDs)
        {
            equipmentID.Value = id;
            reader = comm.ExecuteReader();
            if (reader.Read())
            {
                GameObject temp = Instantiate(entryPrefab, GameObject.Find("Canvas").transform.Find("EquipmentDetail").Find("Viewport").Find("Content"));
                //temp.transform.SetParent(GameObject.Find("Canvas").transform.Find("MonsterInfo").Find("Viewport").Find("content"));
                temp.transform.Find("Label").Find("Name").GetComponent<Text>().text = reader["Name"].ToString();
                temp.transform.Find("Label").Find("Rarity").GetComponent<Text>().text = reader["Rarity"].ToString();
                temp.transform.Find("Label").Find("Class").GetComponent<Text>().text = reader["Class"].ToString();
                temp.transform.Find("Label").Find("PowerLevel").GetComponent<Text>().text = reader["Powerlevel"].ToString();
                temp.transform.Find("Label").Find("Part").GetComponent<Text>().text = reader["part"].ToString();
                entries.Add(temp);
            }
            reader.Close();
        }    
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public List<GameObject> GetEntries()
    {
        return entries;
    }
}
