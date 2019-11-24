using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamSystem : MonoBehaviour {
    [SerializeField]
    FlowField flowField;
    [SerializeField]
    RoomManager roomManager;
	// Use this for initialization
	public void GenerateWorld() {
        roomManager.GenerateMap();
        flowField.UpdateFlowField(0,0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
