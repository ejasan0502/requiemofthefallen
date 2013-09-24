using UnityEngine;
using System.Collections;

public class Console : MonoBehaviour {
	
	public bool display;
	public bool enable;
	public string[] commands;
	private string logs = "";
	private int logsLength = 0;
	private GUIStyle style;
	private string text = "";
	private Vector2 scrollPosition = Vector2.zero;
	
	void Awake(){
		Main.Initialize();	
	}
	void Start () {
		style = Main.GetGUIStyle();
		style.normal.textColor = Color.white;
		
		ArrayList com = new ArrayList();
		for (int i = 0; i < commands.Length; i++){
			com.Add (commands[i].ToString ());	
		}
		com.Sort ();
		commands = new string[com.Count];
		for (int i = 0; i < com.Count; i++){
			commands[i] = com[i].ToString ();
		}
		
		log ("Platform: "+Application.platform);
	}
	void Update () {
		if (Input.GetKeyUp(KeyCode.Menu) && enable){
			display = !display;	
		}
	}
	void OnGUI(){
		if (display){
			GUI.Box (new Rect(Screen.width*0.225f,0,Screen.width*0.525f,Screen.height*0.9f),"");
			
			scrollPosition = GUI.BeginScrollView(new Rect(Screen.width*0.25f,0,Screen.width*0.5f,Screen.height*0.9f),
												 scrollPosition,
												 new Rect(0,0,Screen.width*0.45f,Screen.height*0.05f*logsLength));
				GUI.Label (new Rect(0,0,Screen.width*0.4f,Screen.height*0.05f*logsLength),logs,style);
			GUI.EndScrollView();
			
			GUI.Box (new Rect(Screen.width*0.225f,Screen.height*0.9f,Screen.width*0.525f,Screen.height*0.1f),"");
			text = GUI.TextField(new Rect(Screen.width*0.225f,Screen.height*0.9f,Screen.width*0.425f,Screen.height*0.1f),text,style);
			
			if (GUI.Button (new Rect(Screen.width*0.65f,Screen.height*0.9f,Screen.width*0.1f,Screen.height*0.1f),"Enter")){
				log (text);
				text = "";
			}
		}
	}
	public void log(string n){
		if (n != ""){
			logs += "\n " + n;
			logsLength++;
			if (n.StartsWith("/")){	
				switch(n){
				default:
					break;
				case "/help":
					for (int i = 0; i < commands.Length; i++){
						log("."+commands[i]);
					}
					break;
				case "/exit":
					display = false;
					break;
				}
			}
		}
	}
	public void textColor(Color c){
		style.normal.textColor = c;	
	}
}