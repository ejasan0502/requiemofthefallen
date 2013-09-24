using UnityEngine;
using System.Collections;

public struct SavedObject {
	public string name;
	public Vector3 position;
	public Vector3 scale;
	
	public SavedObject(string n, Vector3 pos){
		name = n;
		position = pos;
		scale = new Vector3(0,0,0);
	}
	public SavedObject(string n, Vector3 pos, Vector3 s){
		name = n;
		position = pos;
		scale = s;
	}
}
