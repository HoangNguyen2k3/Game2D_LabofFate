using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escape_Menu : MonoBehaviour {

	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)) UnityEngine.SceneManagement.SceneManager.LoadScene(PlayerPrefs.GetString("_LastScene"));
	}
}
