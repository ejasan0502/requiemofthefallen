using UnityEngine;
using System.Collections;
using System.Xml;
using System;

public class SaveSceneToXml : MonoBehaviour
{
	public string[] ignoredObjects;
	private GameObject[] objects;
	private string datapath;
	
	void Start ()
	{
		objects = FindObjectsOfType (typeof(GameObject)) as GameObject[];
		datapath = Application.dataPath + "/Resources/Data/SceneData.xml";
		
		if (objects == null) {
			Debug.Log ("GameObject objects Array is null!");	
		} else {
			Debug.Log ("GameObject objects Array is not null!");
		}
		SaveScene ();
	}
	
	private void SaveScene ()
	{
		XmlDocument xmlDoc = new XmlDocument ();
		
		xmlDoc.Load (datapath);
		XmlElement root = xmlDoc.DocumentElement;
			
		XmlNodeList levels = root.ChildNodes;
		foreach (XmlNode l in levels) {
			if (l.Attributes ["name"].Value == Application.loadedLevelName) {
				root.RemoveChild(l);
			}
		}
			
		XmlElement head = xmlDoc.CreateElement ("scene");
		head.SetAttribute ("name", Application.loadedLevelName);
		foreach (GameObject g in objects) {
			bool destroy = false;
			int i = 0;
			while (!destroy && i < ignoredObjects.Length){
				if (g.name == ignoredObjects[i]){
					destroy = true;	
				}
				i++;
			}
			if (!destroy){
				XmlElement obj = xmlDoc.CreateElement ("object");
				
				// Set Name of Object
				obj.SetAttribute("name",g.name);
				
				// Position
				XmlElement pos = xmlDoc.CreateElement("position");
				pos.SetAttribute("x",g.transform.position.x.ToString ());
				pos.SetAttribute("y",g.transform.position.y.ToString ());
				pos.SetAttribute("z",g.transform.position.z.ToString ());
				obj.AppendChild(pos);
				
				// Scale
				XmlElement scale = xmlDoc.CreateElement("scale");
				scale.SetAttribute("x",g.transform.localScale.x.ToString ());
				scale.SetAttribute("y",g.transform.localScale.y.ToString ());
				scale.SetAttribute("z",g.transform.localScale.z.ToString ());
				obj.AppendChild(scale);
				
				head.AppendChild (obj);
				root.AppendChild (head);
			} else {
				continue;		
			}
		}
		xmlDoc.Save (datapath);
		Destroy (this.gameObject);
	}
}
