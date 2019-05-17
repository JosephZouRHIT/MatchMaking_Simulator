using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Data.SqlClient;


public class CSVParsing : MonoBehaviour {


    private Button btn;
    private DBConnection dbService;
	// Use this for initialization
	void Start () {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(CSVParse);
        dbService = GameObject.Find("Connection").GetComponent<DBConnection>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void CSVParse()
    {
        using (StreamReader reader = new StreamReader(@"C:\Users\zous\Desktop\Server.csv"))
        {
            List<string> serverName = new List<string>();
            List<string> ipAddr = new List<string>();
            reader.ReadLine();
            while (!reader.EndOfStream)
            {
                string[] values = reader.ReadLine().Split(',');
                serverName.Add(values[0]);
                ipAddr.Add(values[1]);
            }
            print("parsing server done");
            for (int i = 0; i < serverName.Count; i++)
            {
                SqlCommand comm = new SqlCommand("Insert_Server", dbService.getConnection());
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter serverNameStr = comm.Parameters.Add("@name", System.Data.SqlDbType.VarChar);
                SqlParameter ipAddrStr = comm.Parameters.Add("@ip", System.Data.SqlDbType.VarChar);
                serverNameStr.Value = serverName[i];
                ipAddrStr.Value = ipAddr[i];
                comm.ExecuteNonQuery();
            }
            print("insert server done");
        }
        using (StreamReader reader = new StreamReader(@"C:\Users\zous\Desktop\DropRate.csv"))
        {
            List<string[]> equipment = new List<string[]>();
            List<string[]> dungeon = new List<string[]>();
            List<float> droprate = new List<float>();
            reader.ReadLine();
            while (!reader.EndOfStream)
            {
                string[] values = reader.ReadLine().Split(',');
                equipment.Add(new string[] { values[0], values[1], values[2], values[3], values[4] });
                dungeon.Add(new string[] { values[5], values[6], values[7] });
                droprate.Add(float.Parse(values[8]));
            }
            print("parsing dropdate done");
            for (int i = 0; i < equipment.Count; i++)
            {
                SqlCommand comm = new SqlCommand("Insert_Equipment", dbService.getConnection());
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter equipmentNameStr = comm.Parameters.Add("@EquipmentName", System.Data.SqlDbType.VarChar);
                SqlParameter rarityStr = comm.Parameters.Add("@Rarity", System.Data.SqlDbType.VarChar);
                SqlParameter classStr = comm.Parameters.Add("@Class", System.Data.SqlDbType.VarChar);
                SqlParameter partStr = comm.Parameters.Add("@part", System.Data.SqlDbType.VarChar);
                SqlParameter powerLevelStr = comm.Parameters.Add("@Powerlevel", System.Data.SqlDbType.Int);
                equipmentNameStr.Value = equipment[i][0];
                rarityStr.Value = equipment[i][1];
                classStr.Value = equipment[i][2];
                partStr.Value = equipment[i][3];
                powerLevelStr.Value = int.Parse(equipment[i][4]);
                comm.ExecuteNonQuery();
            }
            print("insert equipment done");
            for (int i = 0; i < equipment.Count; i++)
            {
                SqlCommand comm = new SqlCommand("Insert_Dungeon", dbService.getConnection());
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter nameStr = comm.Parameters.Add("@Name", System.Data.SqlDbType.VarChar);
                SqlParameter locationStr = comm.Parameters.Add("@Location", System.Data.SqlDbType.VarChar);
                SqlParameter descriptionStr = comm.Parameters.Add("@Description", System.Data.SqlDbType.Text);
                nameStr.Value = dungeon[i][0];
                locationStr.Value = dungeon[i][1];
                descriptionStr.Value = dungeon[i][2];
                comm.ExecuteNonQuery();
            }
            print("insert dungeon done");
            for (int i = 0; i < equipment.Count; i++)
            {
                SqlCommand comm = new SqlCommand("add_Drops", dbService.getConnection());
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter dungeonStr = comm.Parameters.Add("@Dungeon", System.Data.SqlDbType.VarChar);
                SqlParameter equipmentNameStr = comm.Parameters.Add("@EquipmentName", System.Data.SqlDbType.VarChar);
                SqlParameter rarityStr = comm.Parameters.Add("@Rarity", System.Data.SqlDbType.VarChar);
                SqlParameter rateStr = comm.Parameters.Add("@Rate", System.Data.SqlDbType.Decimal);
                dungeonStr.Value = dungeon[i][0];
                equipmentNameStr.Value = equipment[i][0];
                rarityStr.Value = equipment[i][1];
                rateStr.Value = (decimal)droprate[i];
                comm.ExecuteNonQuery();
            }
            print("insert droprate done");
        }
        using (StreamReader reader = new StreamReader(@"C:\Users\zous\Desktop\Contains.csv"))
        {
            List<string[]> monster = new List<string[]>();
            List<string> dungeon = new List<string>();
            List<int> amount = new List<int>();
            reader.ReadLine();
            while (!reader.EndOfStream)
            {
                string[] values = reader.ReadLine().Split(',');
                monster.Add(new string[] { values[0], values[1], values[2], values[3]});
                dungeon.Add(values[4]);
                amount.Add(int.Parse(values[7]));
            }
            print("parsing contains done");
            for (int i = 0; i < monster.Count; i++)
            {
                SqlCommand comm = new SqlCommand("Insert_Monster", dbService.getConnection());
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter nameStr = comm.Parameters.Add("@Name", System.Data.SqlDbType.VarChar);
                SqlParameter rarityStr = comm.Parameters.Add("@Rarity", System.Data.SqlDbType.VarChar);
                SqlParameter classStr = comm.Parameters.Add("@Class", System.Data.SqlDbType.VarChar);
                SqlParameter powerLevelStr = comm.Parameters.Add("@Powerlevel", System.Data.SqlDbType.Int);
                nameStr.Value = monster[i][0];
                rarityStr.Value = monster[i][1];
                classStr.Value = monster[i][3];
                powerLevelStr.Value = int.Parse(monster[i][2]);
                comm.ExecuteNonQuery();
            }
            print("insert monster done");
            for (int i = 0; i < monster.Count; i++)
            {
                SqlCommand comm = new SqlCommand("add_Contains", dbService.getConnection());
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter dungeonStr = comm.Parameters.Add("@Dungeon", System.Data.SqlDbType.VarChar);
                SqlParameter rarityStr = comm.Parameters.Add("@Rarity", System.Data.SqlDbType.VarChar);
                SqlParameter nameStr = comm.Parameters.Add("@MonsterName", System.Data.SqlDbType.VarChar);
                SqlParameter amountStr = comm.Parameters.Add("@amount", System.Data.SqlDbType.Int);
                nameStr.Value = monster[i][0];
                rarityStr.Value = monster[i][1];
                dungeonStr.Value = dungeon[i];
                amountStr.Value = amount[i];
                comm.ExecuteNonQuery();            }
        }
    }
}
