using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectAll : MonoBehaviour {

    private Button btn;
    private bool toggle;
	// Use this for initialization
	void Start () {
        btn = GetComponent<Button>();
        toggle = true;
        btn.onClick.AddListener(ToggleAll);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void ToggleAll()
    {
        GetEquipment get = GameObject.Find("Canvas").transform.Find("EquipmentDetail").Find("Viewport").Find("Content").GetComponent<GetEquipment>();
        List<GameObject> entries = get.GetEntries();
        foreach(GameObject obj in entries)
        {
            Toggle temp = obj.GetComponent<Toggle>();
            temp.isOn = toggle;
        }
        toggle = !toggle;
    }
}
