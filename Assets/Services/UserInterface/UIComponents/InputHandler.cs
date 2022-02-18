using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    bool isDragged;
    float lastZoom;   //last Zoom value
    Vector2 startPos;
    bool isDown;

    int touchCount;

    void Update() {
        touchCount = Input.touchCount;

#if UNITY_EDITOR
        if (isDown)
            touchCount = 1;
        else
            touchCount = 0;
#endif

  //      if (Selected.o.item) {
		//	switch (touchCount) {
		//	case 1:
		//		Selected.o.Move (Inpute.Drag() * 14.5f); break;
		//	case 2:
		//		Selected.o.Rotate (Inpute.Drag() * 100); break;
		//	}
		//} else {
			switch (touchCount) {
			case 1:
                //if (Input.GetMouseButton(0))
                //    CameraController.o.Rotate (Inpute.Drag() * 100);
                //else
                //    CameraController.o.Move(Inpute.Drag() * 14.5f);
                break;
            case 2:
				Zoom (); break;
			//}
		}
    }

    public void OnDown() {
        isDown = true;
        Inpute.OnInputDown();
    }

    public void OnUp() {
        isDown = false;
        Inpute.OnInputUp();
        //if (Inpute.InputType() == Click.Tap)
        //    Select(Input.mousePosition);
    }

    void Zoom () {
#if !UNITY_EDITOR
		float currZoom = Vector2.Distance (Input.touches [0].position, Input.touches [1].position);
		if (lastZoom != 0) {
			CameraController.o.isZoomed = true;
			CameraController.o.zoom -= (currZoom - lastZoom) / 10;
		} 
		lastZoom = currZoom;
#else
        CameraController.o.zoom += Input.GetAxis ("Mouse ScrollWheel") * 5;
        CameraController.o.isZoomed = true;
#endif
        CameraController.o.zoom = CameraController.o.zoom < CameraController.o.zoomRange.x ? CameraController.o.zoomRange.x : CameraController.o.zoom > CameraController.o.zoomRange.y ? CameraController.o.zoomRange.y : CameraController.o.zoom;
	}

	RaycastHit hit;
	void Select (Vector3 mousePosition) {
		if (Selected.o.item)
			Selected.o.item.gameObject.layer = 2;
		bool gotHit = Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit);

        if (Selected.o.item)
			Selected.o.item.gameObject.layer = 0;
        if (gotHit) {
			if (Selected.o.item)
				Selected.o.Select (null);
			else if (hit.collider.tag == "Respawn")
				Selected.o.Select (hit.collider.transform);
		}
	}
}
