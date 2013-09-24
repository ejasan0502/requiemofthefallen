using UnityEngine;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public static class Main {
	#region Variables
	private static Console console;			// Console Object
	private static ArrayList tempObjects;	// List of Temporary Game Objects
	private static SoundManager sM;			// Sound Manager Object
	private static ArrayList scenes;		// List of Scenes
	private static TouchListener touch;		// TouchListener Object
	#endregion
	#region Default
	// Required Methods
	public static void Initialize(){
		console = GameObject.FindWithTag("console").GetComponent<Console>();
		log ("Initializing Main");
		
		sM = GameObject.FindWithTag("sound manager").GetComponent<SoundManager>();
		touch = GameObject.FindWithTag("touch listener").GetComponent<TouchListener>();
		
		LoadData();
	}
	public static GUIStyle GetGUIStyle ()
	{
		GUIStyle g = new GUIStyle ();
		g.fontSize = Mathf.RoundToInt ((Screen.height * 0.1f) / 2.5f);
		return g;	
	}
	public static TouchListener GetTouch(){
		return touch;	
	}
	public static SoundManager GetSoundManager(){
		return sM;	
	}
	
	// Console Logs
	public static void log(string n){
		console.log (n);
	}
	public static void log (Vector3 v){
		console.log ("("+v.x.ToString()+","+v.y.ToString()+","+v.z.ToString()+")");	
	}
	public static void log (int i){
		console.log (i.ToString());	
	}
	public static void log(float f){
		console.log (f.ToString ());	
	}
	public static void log (double d){
		console.log (d.ToString ());	
	}
	public static void Log(string n){
		console.log (n);
	}
	public static void Log (Vector3 v){
		console.log ("("+v.x.ToString()+","+v.y.ToString()+","+v.z.ToString()+")");	
	}
	public static void Log (int i){
		console.log (i.ToString());	
	}
	public static void Log(float f){
		console.log (f.ToString ());	
	}
	public static void Log (double d){
		console.log (d.ToString ());	
	}
	
	// Load Scene
	public static void LoadScene(string n){
		foreach (Scene s in scenes){
			if (s.name.ToLower () == n.ToLower ()){
				// Reset variables
				tempObjects = new ArrayList();
				
				// Destroy all objects in current scene
				foreach (GameObject g in GameObject.FindObjectsOfType(typeof(GameObject))){
					if (Application.platform == RuntimePlatform.Android){
						GameObject.DestroyImmediate(g);
					} else {
						GameObject.Destroy (g);
					}
				}
				
				// Create Objects
				foreach (SavedObject so in s.objects){
					GameObject o = GameObject.Instantiate(Resources.Load ("Prefabs/Menus/"+so.name)) as GameObject;	
					o.name = so.name;
				}
			}
		}
	}
	public static void LoadScene(int i){
		// Reset variables
		tempObjects = new ArrayList();
		
		// Destroy all objects in current scene
		foreach (GameObject g in GameObject.FindObjectsOfType(typeof(GameObject))){
			if (Application.platform == RuntimePlatform.Android){
				GameObject.DestroyImmediate(g);
			} else {
				GameObject.Destroy (g);
			}
		}
		
		// Create Objects
		foreach (SavedObject so in ((Scene)scenes[i]).objects){
			GameObject o = GameObject.Instantiate(Resources.Load ("Prefabs/Menus/"+so.name)) as GameObject;	
			o.name = so.name;
		}
	}
	public static void LoadSceneAdditive(string n){
		foreach (Scene s in scenes){
			if (s.name.ToLower () == n.ToLower ()){
				// Clear tempObjects array
				if (tempObjects.Count > 0){
					foreach (GameObject o in tempObjects){
						if (Application.platform == RuntimePlatform.Android){
							GameObject.DestroyImmediate(o);	
						} else {
							GameObject.Destroy (o);
						}
					}
				}
				tempObjects = new ArrayList();
				
				// Create Objects
				foreach (SavedObject so in s.objects){
					GameObject o = GameObject.Instantiate(Resources.Load ("Prefabs/Menus/"+so.name)) as GameObject;	
					o.name = so.name;
					tempObjects.Add (o);
				}
			}
		}
	}
	public static void LoadSceneAdditive(int i){
		// Clear tempObjects array
		if (tempObjects.Count > 0){
			foreach (GameObject o in tempObjects){
				if (Application.platform == RuntimePlatform.Android){
					GameObject.DestroyImmediate(o);	
				} else {
					GameObject.Destroy (o);
				}
			}
		}
		tempObjects = new ArrayList();
		
		// Create Objects
		foreach (SavedObject so in ((Scene)scenes[i]).objects){
			GameObject o = GameObject.Instantiate(Resources.Load ("Prefabs/Menus/"+so.name)) as GameObject;	
			o.name = so.name;
			tempObjects.Add (o);
		}
	}
	public static void RemoveAdditiveScene(){
		if (tempObjects.Count > 0){
			foreach (GameObject o in tempObjects){
				if (Application.platform == RuntimePlatform.Android){
					GameObject.DestroyImmediate(o);	
				} else {
					GameObject.Destroy (o);	
				}
			}
		}
	}
	#endregion
	#region Data
	// Data
	public static void SaveData(){
		log ("Saving Data");
	}
	private static void LoadData(){
		log ("Loading Data");
		LoadSceneData();
	}
	private static void LoadSceneData(){
		log ("Loading Scenes Data");
		
		// Initialize ArrayList scenes
		scenes = new ArrayList ();
		
		// Begin loading data from xml
		XmlDocument xmlDoc = new XmlDocument ();
		
		// Locate Data
		if (File.Exists (Application.persistentDataPath + "/SceneData.xml")) {
			xmlDoc.Load (Application.persistentDataPath + "/SceneData.xml");
		} else {
			TextAsset xml = (TextAsset)Resources.Load ("Data/SceneData", typeof(TextAsset));
			xmlDoc.LoadXml (xml.text);
		}
			
		// Begin loading scene data
		XmlNodeList sceneList = xmlDoc.GetElementsByTagName ("scene");
		
		foreach (XmlNode s in sceneList) {
			XmlNodeList content = s.ChildNodes;
			string[] args = s.Attributes ["name"].Value.Split ('_');
			ArrayList o = new ArrayList ();
			
			foreach (XmlNode i in content) {
				string n = "";
				Vector3 p = Vector3.zero;
				Vector3 ss = Vector3.zero;;
				if (i.Name == "object"){
					n = i.Attributes["name"].Value;
					foreach (XmlNode ii in i.ChildNodes){
						if (ii.Name == "position")
							p = new Vector3(float.Parse(ii.Attributes["x"].Value),float.Parse(ii.Attributes["y"].Value),float.Parse(ii.Attributes["z"].Value));
						else if (ii.Name == "scale"){
							ss = new Vector3(float.Parse(ii.Attributes["x"].Value),float.Parse(ii.Attributes["y"].Value),float.Parse(ii.Attributes["z"].Value));	
						}
					}
					o.Add (new SavedObject(n,p,ss));
				}
			}
			
			// Add Scene
			scenes.Add (new Scene (int.Parse (args [0]), args [1], o));
		}
		
		// Organize Scene Data based on index
		Scene[] temp = new Scene[scenes.Count];
		foreach (Scene s in scenes){
			temp[s.index] = s;	
		}
		scenes.Clear();
		foreach (Scene s in temp){
			scenes.Add(s);	
		}
	}
	#endregion
}