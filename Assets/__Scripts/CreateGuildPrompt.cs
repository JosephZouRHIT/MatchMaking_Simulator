using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateGuildPrompt : MonoBehaviour {

    public GameObject prompt;

    private Button btn;
	// Use this for initialization
	void Start () {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(ShowPrompt);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void ShowPrompt()
    {
        Instantiate(prompt, GameObject.Find("Canvas").transform);
    }
}
