using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Object2D : MonoBehaviour {
	private float w, h;			// Width and Height
	private float aspectRatio;	// Aspect Ratio
	private float startTime;	// Time for animations
	private int frame;			// Current frame of Sprite Sheet
	private bool fade;			// Fade the object
	private bool selected;		// Determines if object has been selected

	public SpriteSheet[] spriteSheets;			// Sprite Sheets
	public bool animateOnStart;
	public bool loop;							// Loop animation
	public bool destroyOnEnd;
	
	private SpriteSheet selectedSpriteSheet;	// Selected Sprite Sheet
	private bool stopAnimation = false;
	
	// Initialize
	protected virtual void Start() {
		// Initialize Variables
		w = 0;
		h = 0;
		frame = 0;
		
		// Get Height and Width
		h = 2 * Camera.main.orthographicSize;
		w = h * Camera.main.aspect;
		
		// Get Aspect Ratio
		aspectRatio = w / h;
		
		// Modify Object
		modifyPlane();
		
		// Start Animations if any
		if ((spriteSheets != null || spriteSheets.Length > 0)){
			InitializeAnimations();
		}
	}
	protected virtual void Update(){
		if (selectedSpriteSheet != null && !stopAnimation)
			Animate();
	}
	// Modify plane's position, width, and height to Aspect Ratio 3:2 or 2:3
	// Requirements:
	// 	1.) Camera must be orthographic
	// 	2.) Camera must have an orthographic size of 10
	// 	3.) Plane modified must be created on an aspect ratio of 3:2 or 2:3
	protected void modifyPlane (){
		if(aspectRatio != 1.5f) {
			GameObject obj = this.gameObject;
			h = 2 * Camera.main.orthographicSize;
			w = h * Camera.main.aspect;
			Vector3 center = obj.transform.position;
			Vector3 centerP = new Vector3();
			Vector3 centerN = new Vector3();
			
			centerP.x = (center.x + 30.0f*0.5f)/30.0f;
			centerP.y = (center.y + 20.0f*0.5f)/20.0f;
			
			centerN.x = w * centerP.x - w/2.0f;
			centerN.y = h * centerP.y - h/2.0f;
			centerN.z = center.z;
			
			Bounds b = ((MeshRenderer)obj.GetComponent("MeshRenderer")).bounds;
			float width = b.max[0] - b.min[0];
			float height = b.max[1] - b.min[1];
			
			float pWidth = width/30.0f;
			float pHeight = height/20.0f;
			
			float newWidth = pWidth*w;
			float newHeight = pHeight*h;
			
			float scaleFactorX = newWidth / width;
			float scaleFactorY = newHeight / height;
			
			Vector3 v = new Vector3( centerN.x + Camera.main.transform.position.x,
									 centerN.y + Camera.main.transform.position.y,
									 centerN.z);
			obj.transform.position = v;
			obj.transform.localScale = new Vector3(obj.transform.localScale.x * (scaleFactorX), 0, obj.transform.localScale.z * (scaleFactorY));
		}
	}
	
	#region 2D animation
	protected void Fade(){	
		fade = true;
	}
	protected void Animate(){
		if (Time.time - startTime >= 1.0f/selectedSpriteSheet.frameRate){
			Vector2 offset = renderer.material.mainTextureOffset;
			
			if (frame != selectedSpriteSheet.startFrame){
				offset.x += (1.0f/selectedSpriteSheet.columns);
			}
			if (frame%selectedSpriteSheet.columns == 0 && frame != selectedSpriteSheet.startFrame){
				offset.x = 0;
				offset.y -= 1.0f / selectedSpriteSheet.rows;
				if (offset.y < 0)
					offset.y = 0;
			}
			if (frame > selectedSpriteSheet.endFrame){
				if (loop){
					offset.x = (1.0f/selectedSpriteSheet.columns)*(selectedSpriteSheet.startFrame - (Mathf.FloorToInt(selectedSpriteSheet.startFrame/selectedSpriteSheet.columns)*selectedSpriteSheet.columns));
					offset.y = 1.0f - (Mathf.FloorToInt(selectedSpriteSheet.startFrame/selectedSpriteSheet.columns)*(1.0f/selectedSpriteSheet.rows) + (1.0f/selectedSpriteSheet.rows));
					frame = selectedSpriteSheet.startFrame;
				} else {
					StopAnimation();
				}
			}
			if (!stopAnimation){
				frame++;
				startTime = Time.time;
				renderer.material.SetTextureOffset("_MainTex",offset);
			} else if (destroyOnEnd){
				Destroy(this.gameObject);	
			}
		}	
		if (fade){
			Color c = this.renderer.material.color;
			c.a -= 0.01f;
			this.renderer.material.color = c;	
		}
	}
	protected void Animate(int i){
		// Set selected sprite sheet
		selectedSpriteSheet = spriteSheets[i];
		
		// Set Tiling
		Vector2 scale = new Vector2(1.0f/selectedSpriteSheet.columns,1.0f/selectedSpriteSheet.rows);
		this.renderer.material.SetTextureScale("_MainTex",scale);
		
		// Set Offset
		Vector2 offset = renderer.material.mainTextureOffset;
		offset.x = (1.0f/selectedSpriteSheet.columns)*(selectedSpriteSheet.startFrame - (Mathf.FloorToInt(selectedSpriteSheet.startFrame/selectedSpriteSheet.columns)*selectedSpriteSheet.columns));
		offset.y = 1.0f - (Mathf.FloorToInt(selectedSpriteSheet.startFrame/selectedSpriteSheet.columns)*(1.0f/selectedSpriteSheet.rows) + (1.0f/selectedSpriteSheet.rows));
		this.renderer.material.SetTextureOffset("_MainTex",offset);
		
		// Set frame
		frame = selectedSpriteSheet.startFrame;
		
		// Start animation
		StartAnimation();
	}
	protected void Animate(string state){
		for (int i = 0; i < spriteSheets.Length; i++){
			if (spriteSheets[i].name.ToLower().Contains(state.ToLower())){
				Animate(i);
				return;
			}
		}
	}
	protected void Animate(string state, bool b){
		for (int i = 0; i < spriteSheets.Length; i++){
			if (spriteSheets[i].name.ToLower().Contains(state.ToLower())){
				Animate(i);
				loop = b;
				return;
			}
		}
	}
	private void InitializeAnimations(){
		// Assumes Tiling and Offset is preset
		for (int i = 0; i < spriteSheets.Length; i++){
			if (spriteSheets[i].sheet == this.renderer.material.mainTexture){
				Animate(i);	
			}
		}
	}
	protected void StopAnimation(){
		stopAnimation = true;	
	}
	protected void StartAnimation(){
		stopAnimation = false;	
	}
	#endregion
	
	protected void Destroy(){
		if (Application.isEditor){
			Destroy(this.gameObject);	
		} else {
			DestroyImmediate(this.gameObject);	
		}
	}
	
	public virtual void OnTouch(){}
	public virtual void OnHold(){}
	public virtual void OnMove(){}
	public virtual void OnEnd(){}
}
