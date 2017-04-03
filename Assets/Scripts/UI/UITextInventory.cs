using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITextInventory : MonoBehaviour
{

    private Text _text;

    private void Start()
    {
        _text = GetComponent<Text>();
    }

    private void Update()
    {
        _text.text = "Inventory\nSlots:\n" + WorldController.Instance.World.Player.GetInventorySpaces();
    }
}
