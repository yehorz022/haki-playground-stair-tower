using UnityEngine;

public class CameraController : MonoBehaviour {

	[SerializeField] float dampTime = 0.15f;

	public float zoom;
	public bool isZoomed;
	public Vector2 zoomRange;
	[HideInInspector] public Camera webCam;

	bool isSwicted;
	Vector3 lastPos;
	Quaternion lastRot;

	public static CameraController o;
	void Awake () {
		o = this;
    }

    void Start () {
        transform.position = transform.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.4f, zoom));
    }

	void FixedUpdate () {
		if (isZoomed)
			Zooming ();
	}

	public void Rotate (Vector2 angle)  {
		transform.RotateAround (Vector3.zero, Vector3.up, angle.x);
		transform.RotateAround (Vector3.zero, transform.right, angle.y);
		if (transform.position.y < 0)
			transform.RotateAround (Vector3.zero, transform.right, -angle.y);
	}

    public void Move (Vector2 axis) {
		transform.position = new Vector3(transform.position.x - axis.x, transform.position.y, transform.position.z - axis.y);
	}

	Vector3 velocity = Vector3.zero;
	private void Zooming ()  {
		Vector3 destination = transform.position - Camera.main.ViewportToWorldPoint (new Vector3 (0.5f, 0.4f, zoom));
		transform.position = Vector3.SmoothDamp (transform.position, destination, ref velocity, dampTime);
		if (transform.position == destination)
			isZoomed = false;
	}
}
