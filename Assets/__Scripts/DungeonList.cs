using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data.SqlClient;

public class DungeonList : MonoBehaviour {
    public GameObject dungeonInfo;
    public GameObject MonsterInfo;
    public GameObject EquipmentInfo;

    private Dropdown dropdown;
    private DBConnection dbService;
    private Transform label;
    private DungeonInfo info;
    private MonsterInfo monster;
    private EquipmentInfo equipment;
    // Use this for initialization
    void Start()
    {
        dropdown = GetComponent<Dropdown>();
        dbService = GameObject.Find("Connection").GetComponent<DBConnection>();
        label = dropdown.transform.Find("Label");
        GetDungeons();
        info = dungeonInfo.transform.Find("Values").GetComponent<DungeonInfo>();
        monster = MonsterInfo.transform.Find("Viewport").Find("Content").GetComponent<MonsterInfo>();
        equipment = EquipmentInfo.transform.Find("Viewport").Find("Content").GetComponent<EquipmentInfo>();
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

    private void GetDungeons()
    {
        dropdown.options.Clear();
        SqlCommand comm = new SqlCommand("select DungeonName from Dungeon", dbService.getConnection());
        SqlDataReader reader = comm.ExecuteReader();
        List<string> dungeons = new List<string>();
        dungeons.Add("None");
        while (reader.Read())
        {
            dungeons.Add(reader["DungeonName"].ToString());
        }
        reader.Close();
        foreach (string str in dungeons)
        {
            dropdown.options.Add(new Dropdown.OptionData(str));
        }
    }

    private void UpdateDisplay()
    {
        if(dropdown.options[dropdown.value].text.CompareTo("None") == 0)
        {
            info.clear();
            monster.Clear();
            equipment.Clear();
        }
        else
        {
            info.updateDungeonInfo(dropdown.options[dropdown.value].text);
            monster.Clear();
            equipment.Clear();
            monster.updateMonsterInformation(dropdown.options[dropdown.value].text);
            equipment.updateEquipmentInformation(dropdown.options[dropdown.value].text);
        }
    }
}
