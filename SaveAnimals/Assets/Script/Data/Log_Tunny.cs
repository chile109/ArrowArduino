using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Text;

public class Log_Tunny : MonoBehaviour
{
	// Use this for initialization
	void OnEnable()
	{
		//Application.logMessageReceived += HandleLog;
		Application.logMessageReceivedThreaded += Application_logMessageReceivedThreaded;
	}

	void OnDisable()
	{
		//Application.logMessageReceived -= HandleLog;
		Application.logMessageReceivedThreaded -= Application_logMessageReceivedThreaded;

	}
	public bool isShowingLogConsole = false;
	private Vector2 scrollPosition;
	private float doubleClickDelay = 0.15f;
	private float previousClickTime;
	private StringBuilder logBuilder = new StringBuilder();


	public float updateInterval = 0.5F;
	private float accum = 0; // FPS accumulated over the interval
	private int frames = 0; // Frames drawn over the interval
	private float timeleft; // Left time for current interval
	private string format = "";
	private float fps = 0f;
	void Start()
	{
		scrollPosition = Vector2.zero;
	}
	// Update is called once per frame
	void Update()
	{
		bool flag = false;
		if (Input.GetMouseButtonDown(0))
		{
			if (Time.time - previousClickTime >= doubleClickDelay)
			{
				previousClickTime = Time.time;
			}
			else
			{
				flag = true;
			}
		}
		if (Input.touchCount == 2 || flag)
		{
			isShowingLogConsole = !isShowingLogConsole;
		}
		if (isShowingLogConsole)
		{
			timeleft -= Time.deltaTime;
			accum += Time.timeScale / Time.deltaTime;
			++frames;

			// Interval ended - update GUI text and start new interval
			if (timeleft <= 0.0)
			{
				// display two fractional digits (f2 format)
				fps = accum / frames;
				format = System.String.Format("FPS: {0:F2}", fps);
				timeleft = updateInterval;
				accum = 0.0F;
				frames = 0;
			}
		}
	}

	private void HandleLog(string logString, string stackTrace, LogType type)
	{
		logBuilder.AppendFormat("{0} \n", logString);
	}
	private void Application_logMessageReceivedThreaded(string condition, string stackTrace, LogType type)
	{
		logBuilder.AppendFormat("{0}  \n", condition);
	}
	void OnGUI()
	{
		if (isShowingLogConsole)
		{
			GUI.skin.button.margin = new RectOffset(0, 0, 10, 0);
			GUI.skin.button.stretchWidth = true;
			GUI.skin.button.fixedHeight = ((float)((!isShowingLogConsole ? 30 : 70)));
			GUI.skin.button.wordWrap = false;
			if (Screen.orientation == ScreenOrientation.Portrait)
			{
				GUI.skin.label.fontSize = 20;
			}
			else GUI.skin.label.fontSize = 25;
			GUILayout.Window(0, new Rect(0, 0, Screen.width, Screen.height), DoMyWindow, "My Log", GUILayout.Width(Screen.width), GUILayout.Height(Screen.height));
		}
	}
	void DoMyWindow(int windowID)
	{
		GUIStyle style1 = new GUIStyle();
		style1.fontSize = 20;
		if (fps < 30) style1.normal.textColor = Color.yellow;
		else if (fps < 10) style1.normal.textColor = Color.red;
		else style1.normal.textColor = Color.green;
		GUILayout.Label(format, style1);
		scrollPosition = GUILayout.BeginScrollView(new Vector2(0,1000000), new GUILayoutOption[0]);
		GUILayout.Label(logBuilder.ToString(), new GUILayoutOption[0]);
		GUILayout.EndScrollView();
		if (GUILayout.Button("Clear Console", new GUILayoutOption[0]))
		{
			logBuilder.Remove(0, logBuilder.Length);
		}
	}
}
