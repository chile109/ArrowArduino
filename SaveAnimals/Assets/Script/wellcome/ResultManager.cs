using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour {

	void Start () {
        Cursor.visible = false;
        Invoke("BackWellCome", 15f);
	}
	
    void BackWellCome()
    {
        SceneManager.LoadSceneAsync("Wellcome");
    }
}
