using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour {

	void Start () {
        Cursor.visible = false;
        Invoke("BackWellCome", 15f);

        if (SceneManager.GetActiveScene().name == "Success")
        {
            AudioManager.BGM_ES.Trigger("Victory");
        }
        if (SceneManager.GetActiveScene().name == "Fail")
        {
            AudioManager.BGM_ES.Trigger("Lose");
        }

	}
	
    void BackWellCome()
    {
        SceneManager.LoadSceneAsync("Wellcome");
    }
}
