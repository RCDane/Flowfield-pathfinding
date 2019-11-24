using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour {
	private static DebugManager instance;

	public Dictionary<string, bool> debugDict = new Dictionary<string,bool>();
	// Use this for initialization
	void Start () {
		if(instance == null)
		{
			instance = this;
		}
		else if(instance != this)
		{
			Destroy(this.gameObject);
		}
	}
	bool AskingForDebug = false;
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.O) && Input.GetKey(KeyCode.LeftAlt))
		{
			AskingForDebug = true;
		}
	}
	public static void AddDebugName(string text, bool val = false)
	{
		if(instance == null && Application.isPlaying)
		{
			GameObject debugManager = new GameObject("DebugManager");
			debugManager.AddComponent(typeof(DebugManager));
			instance = debugManager.GetComponent<DebugManager>();
		}

		if(instance.debugDict.ContainsKey(text))
			return;

		instance.debugDict[text] = val;
	}
	public static bool Check(string text){
		if(instance == null && Application.isPlaying)
		{
			GameObject debugManager = new GameObject("DebugManager");
			debugManager.AddComponent(typeof(DebugManager));
			instance = debugManager.GetComponent<DebugManager>();
		}
		else if (instance == null && !Application.isPlaying)
		{
			return false;
		}
		if(instance.debugDict.ContainsKey(text))
			return  instance.debugDict[text];
		return false;
	}
	string debugMessage;
	bool debugList;
	/// <summary>
	/// OnGUI is called for rendering and handling GUI events.
	/// This function can be called multiple times per frame (one call per event).
	/// </summary>
	void OnGUI()
	{
		if(AskingForDebug)
		{
			Rect r = new Rect(5,5, 100,30);
			
			if (Event.current.Equals (Event.KeyboardEvent ("return"))) 
			{
				// enter = true;
				if(debugDict.ContainsKey(debugMessage))
				{
					debugDict[debugMessage] = !debugDict[debugMessage];
					debugMessage = "";
				}
				AskingForDebug = false;
			}
			GUI.SetNextControlName("Text");
			debugMessage = GUI.TextField(r, debugMessage);

            Rect button = new Rect(125, 5, 30, 30);
            debugList = GUI.Toggle(button, debugList, "");
            List<string> values = new List<string>();
            List<bool> b = new List<bool>();
            if (debugList)
            {
                int gap = 10;
                int count = 1;
                foreach (string text in debugDict.Keys)
                {
                    Rect Label = new Rect(10, (30 + gap) * count++, 100, 30);
                    GUI.Label(Label, text);
                    Vector2 pos = Label.position;
                    pos.x += 80;
                    Label.position = pos;
                    bool val = GUI.Toggle(Label, debugDict[text],"");
                    if (val != debugDict[text])
                    {
                        values.Add(text);
                        b.Add(val);
                    }

                }
            }
            for (int i = 0; i < values.Count; i++)
            {
                debugDict[values[i]] = b[i];
            }
        }
	}
}
