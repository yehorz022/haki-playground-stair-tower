using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selected : MonoBehaviour {

	public bool hangable;
	public bool pullable;
	public bool sellable;
	public Transform item;
	public bool isOverlaping;
	public Collider collider;

	Vector3 origPos;
	string redColor = "FF000068";
	string greenColor = "00FF0237";
	Renderer renderer;

	public static Selected o;
	void Awake () {
		o = this;
	}

	void Start () {
		renderer = GetComponent <Renderer> ();
        transform.position = new Vector3(1, 2, 3);
	}

//	void OnGUI () {
//		GUI.color = Color.black;
//		GUILayout.Label ("\n\n"+ isOverlaping);
//	}

	void OnTriggerStay (Collider other) {
		if (!isOverlaping && item && other.transform != item && other.name != "Floor" && other.name != "walls") {
			renderer.material.color = Graphik.HexToColor (redColor);
			isOverlaping = true;
		}
	}

	void OnTriggerExit (Collider other) {
		if (isOverlaping && item && other.transform != item && other.name != "Floor" && other.name != "walls") {
			renderer.material.color = Graphik.HexToColor (greenColor);
			isOverlaping = false;
		}
	}

	public void Select (Transform newTrans) {
		item = newTrans;
		if (item) {
			collider = item.GetComponent <Collider> ();
			if (item.GetComponent <Item> ()) {
				Item itm = item.GetComponent <Item> ();
				hangable = itm.hangable;
				pullable = itm.pullable;
				sellable = itm.sellable;
				//if (sellable && itm.orderId < 0)
				//	Shop.o.sellBtn.SetActive (itm);
			}
            transform.rotation = item.rotation;
            transform.position = collider.bounds.center;
			item.rotation = Quaternion.identity;
            transform.localScale = collider.bounds.size + Vector3.one / 5;
			item.rotation = transform.rotation;
		} else {
            transform.localScale = Vector3.zero;
			//Shop.o.sellBtn.SetActive (false);
		}
	}

	public void Pick () {
		if (item)
			origPos = item.position;
	}

	public void Move (Vector2 axis) {
		item.position += CameraController.o.transform.right * axis.x + CameraController.o.transform.up * axis.y;
        //CheckBoundaries (); 
  //      if (!hangable) {
		//	if (!pullable)
		//		item.position = new Vector3 (item.position.x, (item.position.y - collider.bounds.min.y) - Room.o.extent.y, item.position.z);
		//}
        transform.position = collider.bounds.center;
	}

	public void Rotate (Vector2 axis) {
		item.RotateAround (Vector3.up, axis.x / 7 - axis.y / 7);
        transform.position = collider.bounds.center;
        transform.rotation = item.rotation;
	}

	public void Drop () {
		if (item) {
			if (!hangable && pullable)
				Arrange ();
			StartCoroutine (RePosition ());
		}
	}

	IEnumerator RePosition () {
		yield return new WaitForFixedUpdate ();
		if (isOverlaping)
			item.position = origPos;
        transform.position = collider.bounds.center;
	}

	void Arrange () {
		item.gameObject.layer = 2;
		RaycastHit hit;
		if (Physics.Raycast (item.position, Vector3.down, out hit)) {
			float extent = item.position.y - collider.bounds.min.y;
			item.position = new Vector3 (item.position.x, hit.point.y + extent + .2f, item.position.z);
            transform.position = collider.bounds.center;
//			Debug.DrawLine (item.position, new Vector3 (item.position.x, collider.bounds.min.y, item.position.z), Color.blue, 55);
		}
		item.gameObject.layer = 0;
	}

	void CheckBoundaries () {
		Bounds bounds = collider.bounds;
		Vector3 minExtent = item.position - bounds.min;
		Vector3 maxExtent = bounds.max - item.position;
		//if (item.position.y + maxExtent.y > Room.o.extent.y)
		//	item.position = new Vector3 (item.position.x, Room.o.extent.y - maxExtent.y, item.position.z);

		//if (item.position.y - minExtent.y < -Room.o.extent.y)
		//	item.position = new Vector3 (item.position.x, -Room.o.extent.y + minExtent.y, item.position.z);

		//if (item.position.x + maxExtent.x > Room.o.extent.x)
		//	item.position = new Vector3 (Room.o.extent.x - maxExtent.x, item.position.y, item.position.z);

		//if (item.position.x - minExtent.x < -Room.o.extent.x)
		//	item.position = new Vector3 (-Room.o.extent.x + minExtent.x, item.position.y, item.position.z);

		//if (item.position.z + maxExtent.z > Room.o.extent.z)
		//	item.position = new Vector3 (item.position.x, item.position.y, Room.o.extent.z - maxExtent.z);

		//if (item.position.z - minExtent.z < -Room.o.extent.z)
		//	item.position = new Vector3 (item.position.x, item.position.y, -Room.o.extent.z + minExtent.z);
	}
}
