using UnityEngine;
using System.Collections;

public class TouchListener : MonoBehaviour {
	
	private Object2D selectedObject = null;
	private Vector2 startMousePosition = Vector2.zero;
	
	public bool zoom;							// Does the game zoom?
	
	void Start () {
	
	}
	void Update () {
		if (Application.isEditor){
			EditorUpdate();
		} else {
			AndroidUpdate();
		}
	}
	private void EditorUpdate(){
		if (Input.GetMouseButtonDown(0)){
			selectedObject = FindTouchedObject();
			startMousePosition = Input.mousePosition;
			if (selectedObject != null)
				selectedObject.OnTouch();
		} else
		if (Input.GetMouseButton(0) && (Vector2)Input.mousePosition == startMousePosition){
			if (selectedObject != null)
				selectedObject.OnHold();
		} else
		if (Input.GetMouseButton(0) && (Vector2)Input.mousePosition != startMousePosition){
			if (selectedObject != null)
				selectedObject.OnMove();
		} else
		if (Input.GetMouseButtonUp(0)){
			if (selectedObject != null)
				selectedObject.OnEnd();
			selectedObject = null;
		}
	}
	private void AndroidUpdate(){
		if (Input.touchCount == 1){
			if (Input.touches[0].phase == TouchPhase.Began){
				selectedObject = FindTouchedObject();
				if (selectedObject != null)
					selectedObject.OnTouch();
			} else
			if (Input.touches[0].phase == TouchPhase.Stationary){
				if (selectedObject != null)
					selectedObject.OnHold();
			} else
			if (Input.touches[0].phase == TouchPhase.Moved){
				if (selectedObject != null)
					selectedObject.OnMove();
			} else
			if (Input.touches[0].phase == TouchPhase.Ended){
				if (selectedObject != null)
					selectedObject.OnEnd();
				selectedObject = null;
			}
		} else if (Input.touchCount == 2 && zoom){
			if (Input.touches[0].phase == TouchPhase.Began && Input.touches[1].phase == TouchPhase.Began){
				
			} else
			if ((Input.touches[0].phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Moved) ||
				(Input.touches[0].phase == TouchPhase.Moved && Input.touches[1].phase == TouchPhase.Moved)){
				
			} else
			if ((Input.touches[0].phase == TouchPhase.Ended || Input.touches[1].phase == TouchPhase.Ended) ||
				(Input.touches[0].phase == TouchPhase.Ended && Input.touches[1].phase == TouchPhase.Ended)){
				
			}
		}
	}
	private Object2D FindTouchedObject(){
		foreach (Object2D o in GameObject.FindObjectsOfType(typeof(Object2D))){
			if (o.gameObject.renderer.bounds.Contains(Camera.mainCamera.ScreenToWorldPoint(Input.mousePosition))){
				return o;	
			}
		}
		return null;
	}
}
