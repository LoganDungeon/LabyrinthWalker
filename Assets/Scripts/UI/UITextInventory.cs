﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITextInventory : MonoBehaviour {

    Text text;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

        text.text = "Inventory\nSpaces:\n" + WorldController.Instance.player.playerInventory.inventorySlots;
	}
}
