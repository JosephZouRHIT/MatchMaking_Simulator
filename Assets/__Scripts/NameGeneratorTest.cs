using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = System.Random;
using System.Data.SqlClient;
using System.Text;

public class NameGeneratorTest : MonoBehaviour {

    public Text display;
    private Button btn;
    private List<string> names = new List<string>();
    private List<string> GuildNames = new List<string>();
    private List<string> serverNames = new List<string>();
    private DBConnection dbService;
    private Random random = new Random();
    //equipments takes class name first, then part name, then use int for pool identification
    private Dictionary<string, Dictionary<string, List<string[]>>> equipments = new Dictionary<string, Dictionary<string, List<string[]>>>();
	// Use this for initialization
	void Start () {
        dbService = GameObject.Find("Connection").GetComponent<DBConnection>();
        btn = GetComponent<Button>();
        PoolPrepare();
        PrepareServerList();
        btn.onClick.AddListener(InsertRandomPlayer);
        StartCoroutine(GetRandomName());
        print("prepare done");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void PrepareServerList()
    {
        for(int i = 0; i < 5; i++)
        {
            serverNames.Add("NA" + (i + 1).ToString());
            serverNames.Add("SA" + (i + 1).ToString());
            serverNames.Add("EU" + (i + 1).ToString());
        }
        serverNames.Add("EU6");
        print("ServerList Created");
    }

    private void PoolPrepare()
    {
        Dictionary<string, List<string[]>> saber = new Dictionary<string, List<string[]>>();
        List<string[]> saberweapon = new List<string[]>();
        saberweapon.Add(new string[] { "Crappy Sword", "Dagger", "Raiper" });
        saberweapon.Add(new string[] { "Sword", "Dagger", "Raper" });
        saberweapon.Add(new string[] { "Sword", "Rapper" });
        saberweapon.Add(new string[] { "Phantomlight", "Matsukura", "Ex-Caliber" });
        saber.Add("Weapon", saberweapon);

        List<string[]> saberhelmet = new List<string[]>();
        saberhelmet.Add(new string[] { "Chain Helm" });
        saberhelmet.Add(new string[] { "Chain Helm" });
        saberhelmet.Add(new string[] { "Chain Helm" });
        saberhelmet.Add(new string[] { "Legacy Of Truth" });
        saber.Add("Helmet", saberhelmet);

        List<string[]> saberarmor = new List<string[]>();
        saberarmor.Add(new string[] { "Chainmail" });
        saberarmor.Add(new string[] { "Chainmail" });
        saberarmor.Add(new string[] { "Chainmail" });
        saberarmor.Add(new string[] { "Trinity Plate" });
        saber.Add("Armor", saberarmor);
        equipments.Add("Saber", saber);

        Dictionary<string, List<string[]>> archer = new Dictionary<string, List<string[]>>();
        List<string[]> archerweapon = new List<string[]>();
        archerweapon.Add(new string[] { "Crappy Bow" });
        archerweapon.Add(new string[] { "Bow" });
        archerweapon.Add(new string[] { "Bow" });
        archerweapon.Add(new string[] { "The Amazing Plutonium Powered Laser Blaster" });
        archer.Add("Weapon", archerweapon);

        List<string[]> archerhelmet = new List<string[]>();
        archerhelmet.Add(new string[] { "Aimer" });
        archerhelmet.Add(new string[] { "Aimer" });
        archerhelmet.Add(new string[] { "Aimer" });
        archerhelmet.Add(new string[] { "Visotron" });
        archer.Add("Helmet", archerhelmet);

        List<string[]> archerarmor = new List<string[]>();
        archerarmor.Add(new string[] { "Leather Cloth" });
        archerarmor.Add(new string[] { "Leather Cloth" });
        archerarmor.Add(new string[] { "Leather Cloth" });
        archerarmor.Add(new string[] { "Mechcoat" });
        archer.Add("Armor", archerarmor);
        equipments.Add("Archer", archer);

        Dictionary<string, List<string[]>> lancer = new Dictionary<string, List<string[]>>();
        List<string[]> lancerweapon = new List<string[]>();
        lancerweapon.Add(new string[] { "Crappy Spear"});
        lancerweapon.Add(new string[] { "Spear" });
        lancerweapon.Add(new string[] { "Spear" });
        lancerweapon.Add(new string[] { "Piercer" });
        lancer.Add("Weapon", lancerweapon);

        List<string[]> lancerhelmet = new List<string[]>();
        lancerhelmet.Add(new string[] { "Damaged Helmet" });
        lancerhelmet.Add(new string[] { "Cool Helmet" });
        lancerhelmet.Add(new string[] { "Cool Helmet" });
        lancerhelmet.Add(new string[] { "Awesome Helmet" });
        lancer.Add("Helmet", lancerhelmet);

        List<string[]> lancerarmor = new List<string[]>();
        lancerarmor.Add(new string[] { "Not Really Awesome Jacket" });
        lancerarmor.Add(new string[] { "Awesome Jacket" });
        lancerarmor.Add(new string[] { "Awesome Jacket" });
        lancerarmor.Add(new string[] { "Bright Cool Diggling Jacket" });
        lancer.Add("Armor", lancerarmor);
        equipments.Add("Lancer", lancer);

        Dictionary<string, List<string[]>> ruler = new Dictionary<string, List<string[]>>();
        List<string[]> rulerweapon = new List<string[]>();
        rulerweapon.Add(new string[] { "Crappy Banner" });
        rulerweapon.Add(new string[] { "Banner" });
        rulerweapon.Add(new string[] { "Banner" });
        rulerweapon.Add(new string[] { "Banner of Justice", "Wildfire" });
        ruler.Add("Weapon", rulerweapon);

        List<string[]> rulerhelmet = new List<string[]>();
        rulerhelmet.Add(new string[] { "Damaged Mask" });
        rulerhelmet.Add(new string[] { "Mask" });
        rulerhelmet.Add(new string[] { "DOGE" });
        rulerhelmet.Add(new string[] { "Aiming Eyes" });
        ruler.Add("Helmet", rulerhelmet);

        List<string[]> rulerarmor = new List<string[]>();
        rulerarmor.Add(new string[] { "Steel Armor" });
        rulerarmor.Add(new string[] { "Killer" });
        rulerarmor.Add(new string[] { "Forbidden Ring", "Glory of Hole", "Defender" });
        rulerarmor.Add(new string[] { "Don't Touch", "Light of the seven" });
        ruler.Add("Armor", rulerarmor);
        equipments.Add("Ruler", ruler);
    }

    private void InsertRandomPlayer()
    {
        SqlTransaction transaction = dbService.getConnection().BeginTransaction();
        try
        {
            using (SqlCommand comm = new SqlCommand("Register", dbService.getConnection(), transaction))
            {
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                //SqlParameter returnValue = comm.Parameters.Add("@returnValue", System.Data.SqlDbType.Int);
                //returnValue.Direction = System.Data.ParameterDirection.ReturnValue;
                SqlParameter usernameStr = comm.Parameters.Add("@Username", System.Data.SqlDbType.VarChar);
                SqlParameter passSalt = comm.Parameters.Add("@Salt", System.Data.SqlDbType.VarChar);
                SqlParameter passHash = comm.Parameters.Add("@PassHash", System.Data.SqlDbType.VarChar);
                for (int i = 0; i < 3200; i++)
                {
                    byte[] salt = new byte[32];
                    dbService.getRandomizer().GetNonZeroBytes(salt);

                    string saltStr = Encoding.ASCII.GetString(salt);
                    passSalt.Value = saltStr;
                    usernameStr.Value = names[i];
                    passHash.Value = dbService.HashPassword(saltStr, RandomPassword());
                    comm.ExecuteNonQuery();
                }
            }
            print("User Registered");
            List<List<PlayerPair>> nameByServer = new List<List<PlayerPair>>();
            List<string[]> guildByServer = new List<string[]>();
            for (int i = 0; i < 16; i++)
            {
                string[] tempguild = GuildNames.GetRange(i * 5, 5).ToArray();
                guildByServer.Add(tempguild);

                string[] tempname = names.GetRange(i * 200, 200).ToArray();
                List<PlayerPair> tempPair = new List<PlayerPair>();
                for (int j = 0; j < 200; j++)
                {
                    if (j < 50)
                    {
                        tempPair.Add(new PlayerPair(tempname[j], "Saber"));
                    }
                    else if (j < 100)
                    {
                        tempPair.Add(new PlayerPair(tempname[j], "Archer"));
                    }
                    else if (j < 150)
                    {
                        tempPair.Add(new PlayerPair(tempname[j], "Lancer"));
                    }
                    else
                    {
                        tempPair.Add(new PlayerPair(tempname[j], "Ruler"));
                    }
                }
                tempPair = ShuffleList<PlayerPair>(tempPair);
                nameByServer.Add(tempPair);
            }
            print("Name distributed & Shuffled");
            using (SqlCommand comm = new SqlCommand("Insert_Guild", dbService.getConnection(), transaction))
            {
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter nameStr = comm.Parameters.Add("@Name", System.Data.SqlDbType.VarChar);
                SqlParameter serverNameStr = comm.Parameters.Add("@ServerName", System.Data.SqlDbType.VarChar);
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        nameStr.Value = guildByServer[i][j];
                        serverNameStr.Value = serverNames[i];
                        comm.ExecuteNonQuery();
                    }
                }
                print("Insert Guild Complete");
            }
            using (SqlCommand comm = new SqlCommand("new_Player", dbService.getConnection(), transaction))
            {
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter usernameStr = comm.Parameters.Add("@username", System.Data.SqlDbType.VarChar);
                SqlParameter classStr = comm.Parameters.Add("@Class", System.Data.SqlDbType.VarChar);
                SqlParameter serverNameStr = comm.Parameters.Add("@ServerName", System.Data.SqlDbType.VarChar);
  
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 200; j++)
                    {
                        PlayerPair temp = nameByServer[i][j];
                        usernameStr.Value = temp.name;
                        classStr.Value = temp.className;
                        serverNameStr.Value = serverNames[i];
                        comm.ExecuteNonQuery();
                    }
                    nameByServer[i] = ShuffleList<PlayerPair>(nameByServer[i]);
                }
            }
            print("Create Player Complete");
            using (SqlCommand join = new SqlCommand("join_Guild", dbService.getConnection(), transaction))
            {
                join.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter usernameStrJoin = join.Parameters.Add("@username", System.Data.SqlDbType.VarChar);
                SqlParameter guildNameStrJoin = join.Parameters.Add("@GuildName", System.Data.SqlDbType.VarChar);
                SqlParameter returnValue = join.Parameters.Add("@returnValue", System.Data.SqlDbType.Int);
                returnValue.Direction = System.Data.ParameterDirection.ReturnValue;
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 150; j++)
                    {
                        PlayerPair temp = nameByServer[i][j];
                        usernameStrJoin.Value = temp.name;
                        if (j < 50)
                        {
                            guildNameStrJoin.Value = guildByServer[i][0];
                        }
                        else if (j < 90)
                        {
                            guildNameStrJoin.Value = guildByServer[i][1];
                        }
                        else if (j < 120)
                        {
                            guildNameStrJoin.Value = guildByServer[i][2];
                        }
                        else if (j < 140)
                        {
                            guildNameStrJoin.Value = guildByServer[i][3];
                        }
                        else if (j < 150)
                        {
                            guildNameStrJoin.Value = guildByServer[i][4];
                        }
                        join.ExecuteNonQuery();
                        if ((int)returnValue.Value != 0)
                        {
                            print(returnValue.Value.ToString());
                            SqlCommand tempComm = new SqlCommand("Select Name from Player p join GameServer g on g.ServerId = p.ServerID where Username = '" + usernameStrJoin.Value.ToString() + "'", dbService.getConnection(), transaction);
                            SqlDataReader reader = tempComm.ExecuteReader();
                            if (reader.Read())
                            {
                                print(reader["Name"].ToString());
                            }
                            reader.Close();
                            tempComm = new SqlCommand("select gs.Name from Guild g join GameServer gs on g.ServerID = gs.ServerID where GuildName = '" + guildNameStrJoin.Value.ToString() + "'", dbService.getConnection(),transaction);
                            reader = tempComm.ExecuteReader();
                            if (reader.Read())
                            {
                                print(reader["Name"].ToString());
                            }
                            print(guildNameStrJoin.Value.ToString());
                            transaction.Rollback();
                            return;
                        }
                    }
                    nameByServer[i] = ShuffleList<PlayerPair>(nameByServer[i]);
                }
            }
            print("Assign Guild Complete");
            using (SqlCommand comm = new SqlCommand("update Player set Lvl = @level where Username = @name", dbService.getConnection(), transaction))
            {
                SqlParameter level = comm.Parameters.Add("@level", System.Data.SqlDbType.Int);
                SqlParameter usernameStr = comm.Parameters.Add("@name", System.Data.SqlDbType.VarChar);
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 200; j++)
                    {
                        int levelvalue = 1;
                        if (j < 100)
                        {
                            levelvalue = (int)(random.NextDouble() * 10 + 30);
                            level.Value = levelvalue;
                        }
                        else if (j < 170)
                        {
                            levelvalue = (int)(random.NextDouble() * 10 + 50);
                            level.Value = levelvalue;
                        }
                        else
                        {
                            levelvalue = (int)(random.NextDouble() * 10 + 100);
                            level.Value = levelvalue;
                        }
                        usernameStr.Value = nameByServer[i][j].name;
                        comm.ExecuteNonQuery();
                    }
                }
            }
            print("Player level assigned");
            using (SqlCommand comm = new SqlCommand("add_Owns", dbService.getConnection(), transaction))
            {
                string[] parts = new string[] { "Weapon", "Helmet", "Armor" };
                string[] rarity = new string[] { "Common", "Rare", "Legendary", "Exotic" };
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter usernameStr = comm.Parameters.Add("@Username", System.Data.SqlDbType.VarChar);
                SqlParameter equipmentNameStr = comm.Parameters.Add("@EquipmentName", System.Data.SqlDbType.VarChar);
                SqlParameter rarityStr = comm.Parameters.Add("@Rarity", System.Data.SqlDbType.VarChar);
                SqlParameter returnValue = comm.Parameters.Add("@returnValue", System.Data.SqlDbType.Int);
                returnValue.Direction = System.Data.ParameterDirection.ReturnValue;
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 200; j++)
                    {
                        PlayerPair temp = nameByServer[i][j];
                        usernameStr.Value = temp.name;
                        int pool = 0;
                        if (j < 100)
                        {
                            pool = (int)(random.NextDouble() * 1);
                        }
                        else if (j < 170)
                        {
                            pool = (int)(random.NextDouble() * 1 + 1);
                        }
                        else
                        {
                            pool = (int)(random.NextDouble() * 1 + 2);
                        }
                        foreach (string part in parts)
                        {
                            List<string[]> tempEquipmentList = equipments[temp.className][part];
                            rarityStr.Value = rarity[pool];
                            equipmentNameStr.Value = tempEquipmentList[pool][(int)(random.NextDouble() * (tempEquipmentList[pool].Length - 1))];
                            comm.ExecuteNonQuery();
                            if ((int)returnValue.Value != 0)
                            {
                                print(returnValue.Value.ToString());
                                transaction.Rollback();
                                return;
                            }
                        }
                    }
                }
                print("Equipment Assigned");
                transaction.Commit();
            }
        }
        catch(Exception ex)
        {
            print(ex.Message + ex.StackTrace);
            transaction.Rollback();
        }
    }

    IEnumerator GetRandomName()
    {
        UnityWebRequest myWr = UnityWebRequest.Get("http://names.drycodes.com/3200");
        yield return myWr.SendWebRequest();
        //if (myWr.isNetworkError || myWr.isHttpError)
        //{
        //    Debug.Log(myWr.error);
        //}
        string temp = myWr.downloadHandler.text.Substring(1, myWr.downloadHandler.text.Length - 2);
        temp = Regex.Replace(temp, "\"", String.Empty);
        string[] tempArr = temp.Split(',');
        foreach(string str in tempArr)
        {
            names.Add(str);
        }
        print("GirlName Added");

        //myWr = UnityWebRequest.Get("http://names.drycodes.com/1600");
        //yield return myWr.SendWebRequest();
        ////if(myWr.isNetworkError || myWr.isHttpError)
        ////{
        ////    Debug.Log(myWr.error);
        ////}
        //temp = myWr.downloadHandler.text.Substring(1, myWr.downloadHandler.text.Length - 2);
        //temp = Regex.Replace(temp, "\"", String.Empty);
        //tempArr = temp.Split(',');
        //foreach (string str in tempArr)
        //{
        //    names.Add(str);
        //}
        //print("BoyName Added");
        //names = ShuffleList<string>(names);
        //print("Shuffle Completed");

        myWr = UnityWebRequest.Get("http://names.drycodes.com/80");
        yield return myWr.SendWebRequest();
        //if(myWr.isNetworkError || myWr.isHttpError)
        //{
        //    Debug.Log(myWr.error);
        //}
        temp = myWr.downloadHandler.text.Substring(1, myWr.downloadHandler.text.Length - 2);
        temp = Regex.Replace(temp, "\"", String.Empty);
        tempArr = temp.Split(',');
        foreach (string str in tempArr)
        {
            GuildNames.Add(str);
        }
        print("GuildName added");
    }

    private List<E> ShuffleList<E>(List<E> inputList)
    {
        List<E> randomList = new List<E>();

        int randomIndex = 0;
        while (inputList.Count > 0)
        {
            randomIndex = random.Next(0, inputList.Count); //Choose a random object in the list
            randomList.Add(inputList[randomIndex]); //add it to the new, random list
            inputList.RemoveAt(randomIndex); //remove to avoid duplicates
        }

        return randomList; //return the new random list
    }

    public string RandomString(int size, bool lowerCase)
    {
        StringBuilder builder = new StringBuilder();
        char ch;
        for (int i = 0; i < size; i++)
        {
            ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
            builder.Append(ch);
        }
        if (lowerCase)
            return builder.ToString().ToLower();
        return builder.ToString();
    }

    public string RandomPassword()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(RandomString(8, true));
        builder.Append(random.Next(00000000, 99999999));
        builder.Append(RandomString(4, false));
        return builder.ToString();
    }

    private class PlayerPair
    {
        public string name;
        public string className;

        public PlayerPair(string name, string className)
        {
            this.name = name;
            this.className = className;
        }
    }
}
