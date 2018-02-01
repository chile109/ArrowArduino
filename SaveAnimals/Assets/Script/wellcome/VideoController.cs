using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoController : MonoBehaviour {

    public VideoPlayer _playerA;
    public VideoPlayer _playerB;

    private void Start()
    {
        Cursor.visible = false;

        _playerB.loopPointReached += delegate {
            SceneManager.LoadSceneAsync("Game 1");
        };
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            _playerA.Pause();
            _playerB.gameObject.SetActive(true);
        }
    }
}