using UnityEngine;
using System.Collections;

public struct Scene
{
	public int index;
	public string name;
	public ArrayList objects;

	public Scene (int i, string n, ArrayList o)
	{
		index = i;
		name = n;
		objects = o;	
	}
}
