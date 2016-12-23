using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITextStamina : MonoBehaviour {

    private Text _text;

    private void Start() {
        _text = GetComponent<Text>();
    }

    private void Update() {
        _text.text = "Stamina:\n" + WorldController.Instance.Player.Stamina;
    }
}
