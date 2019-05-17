using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Logout : MonoBehaviour {


    private Button btn;
	// Use this for initialization
	void Start () {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(LoadLogin);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LoadLogin()
    {
        SceneManager.LoadScene("LoginPage");
    }
}
