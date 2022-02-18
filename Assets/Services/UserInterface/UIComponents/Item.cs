using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Item : MonoBehaviour {

//	public int ID;
	public float price;
	public bool hangable;
	public bool pullable;
	public bool sellable;
	public int orderId = -1;
	TimeSpan timespan;
	Image clock;
	Text time;
	float second;

	void Start () {
		if (orderId >= 0)
			Ordered (orderId);
	}

	void Update () {
		second += Time.deltaTime;
		//if (orderId >= 0) {
		//	clock.transform.position = transform.position;
		//	if (second > 1) {
		//		float deliveryTime = Shop.o.orders [orderId].GetDeliveryTime ();
		//		timespan = TimeSpan.FromSeconds (deliveryTime);
		//		clock.fillAmount = deliveryTime / Order.shipmentTime;
		//		time.text = deliveryTime.ToString ("0") + "s";
		//		second = 0;

		//		if (deliveryTime <= 0) {
		//			if (GameUI.o.profile.activeSelf)
		//				GameUI.o.UI_Profile ();
		//			Transform item = Game.o.CreateItem (Shop.o.orders [orderId].item).transform;
		//			item.position = transform.position;
		//			item.rotation = transform.rotation;
		//			Selected.o.Select (item);
		//			Shop.o.OrderCompleted (orderId);
		//			Game.me.AddPoints (price / 10);
		//			GameUI.o.Load ();
		//			Destroy (clock.gameObject);
		//			Destroy (gameObject);
		//		}
		//		else
		//			Shop.o.ordersContents.GetChild (orderId).Find ("Panel/Time").GetComponent <Text>().text = (timespan.Hours + ":" + timespan.Minutes + ":" + timespan.Seconds);
		//	}
		//}
	}

	public void Ordered (int id) {
		orderId = id;
		//foreach (MeshRenderer ren in transform.GetComponentsInChildren <MeshRenderer> ())
		//	ren.material = Room.o.greyMaterial; 
		//clock = Instantiate (Game.o.clockPrefab, transform.position, Quaternion.identity).GetComponent <Image> ();
		//clock.transform.SetParent (Camara.o.transform);
		//clock.transform.localRotation = Quaternion.Euler (0, 180, 0);
		//time = clock.GetComponentInChildren <Text> ();
	}

	public void Write (int ID) {
		//		print (obj.name + ID);
		PlayerPrefs.SetFloat (ID + "_position_x", transform.position.x);
		PlayerPrefs.SetFloat (ID + "_position_y", transform.position.y);
		PlayerPrefs.SetFloat (ID + "_position_z", transform.position.z);
		PlayerPrefs.SetFloat (ID + "_rotation_x", transform.rotation.eulerAngles.x);
		PlayerPrefs.SetFloat (ID + "_rotation_y", transform.rotation.eulerAngles.y);
		PlayerPrefs.SetFloat (ID + "_rotation_z", transform.rotation.eulerAngles.z);
		PlayerPrefs.SetFloat (ID + "_scale_x", transform.localScale.x);
		PlayerPrefs.SetFloat (ID + "_scale_y", transform.localScale.y);
		PlayerPrefs.SetFloat (ID + "_scale_z", transform.localScale.z);
		PlayerPrefs.SetInt (ID + "_orderID", orderId);
	}

	public void Read (int ID) {
		//		print (trans.name + ID);
		transform.position = new Vector3 (PlayerPrefs.GetFloat (ID + "_position_x", transform.position.x), 
			PlayerPrefs.GetFloat (ID + "_position_y", transform.position.y),
			PlayerPrefs.GetFloat (ID + "_position_z", transform.position.z));
		transform.rotation = Quaternion.Euler (PlayerPrefs.GetFloat (ID + "_rotation_x", transform.rotation.eulerAngles.x), 
			PlayerPrefs.GetFloat (ID + "_rotation_y", transform.rotation.eulerAngles.y),
			PlayerPrefs.GetFloat (ID + "_rotation_z", transform.rotation.eulerAngles.z));
		transform.localScale = new Vector3 (PlayerPrefs.GetFloat (ID + "_scale_x", transform.localScale.x), 
			PlayerPrefs.GetFloat (ID + "_scale_y", transform.localScale.y),
			PlayerPrefs.GetFloat (ID + "_scale_z", transform.localScale.z));
		orderId = PlayerPrefs.GetInt (ID + "_orderID", -1);
	}
}









