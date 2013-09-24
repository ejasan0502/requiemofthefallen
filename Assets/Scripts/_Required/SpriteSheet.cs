using UnityEngine;
using System.Collections;

public class SpriteSheet : MonoBehaviour {
	public Texture2D sheet;
	public float frameRate = 60.0f;
	public int columns = 1;
	public int rows = 1;
	public int startFrame = 0;
	public int endFrame = 1;
}
