using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CancelGuildPropmt : MonoBehaviour {


    private InputField input;
    private Button btn;
	// Use this for initialization
	void Start () {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(CancelPrompt);
        input = transform.parent.Find("GuildName").GetComponent<InputField>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void CancelPrompt()
    {
        input.text = "";
        Destroy(transform.parent.gameObject);
    }
}
