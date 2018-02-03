using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Video;

public class WellcomeControl : MonoBehaviour {

    public VideoPlayer _playerA;
    public VideoPlayer _playerB;
    public GameObject obj;

    private void Start()
    {
        Cursor.visible = false;

        _playerB.loopPointReached += delegate {
            SceneManager.LoadSceneAsync("Game 1");
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _playerA.Pause();
            obj.SetActive(false);
            _playerB.gameObject.SetActive(true);
        }
    }
}
