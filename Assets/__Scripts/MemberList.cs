using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data.SqlClient;

public class MemberList : MonoBehaviour
{


    private Dropdown dropdown;
    private DBConnection dbService;
    private PlayerInfo playerInfo;
    private Transform label;
    // Use this for initialization
    void Start()
    {
        dropdown = GetComponent<Dropdown>();
        label = dropdown.transform.Find("Label");
        dbService = GameObject.Find("Connection").GetComponent<DBConnection>();
        playerInfo = GameObject.Find("Canvas").transform.Find("PlayerInfo").Find("Values").GetComponent<PlayerInfo>();
        playerInfo.Clear();
        GetMembers();
        dropdown.onValueChanged.AddListener(delegate { UpdateDisplay(); });
    }

    // Update is called once per frame
    void Update()
    {
        if (dropdown.options.Capacity != 0)
        {
            label.GetComponent<Text>().text = dropdown.options[dropdown.value].text;
        }
    }

    private void GetMembers()
    {
        dropdown.options.Clear();
        playerInfo.Clear();
        SqlCommand comm = new SqlCommand("select Username from Player where TeamID = @TeamID", dbService.getConnection());
        SqlParameter teamID = comm.Parameters.Add("@TeamID", System.Data.SqlDbType.Int);
        teamID.Value = dbService.GetTeam();
        SqlDataReader reader = comm.ExecuteReader();
        List<string> members = new List<string>();
        members.Add("None");
        while (reader.Read())
        {
            members.Add(reader["Username"].ToString());
        }
        reader.Close();
        foreach (string str in members)
        {
            dropdown.options.Add(new Dropdown.OptionData(str));
        }
    }

    private void UpdateDisplay()
    {
        if (dropdown.options[dropdown.value].text.CompareTo("None") == 0)
        {
            playerInfo.Clear();
        }
        else
        {
            playerInfo.CheckExistingPlayer(dropdown.options[dropdown.value].text);
        }
    }
}
