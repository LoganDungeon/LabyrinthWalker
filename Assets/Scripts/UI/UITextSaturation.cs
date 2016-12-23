using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITextSaturation : MonoBehaviour {

    private Text _text;

    private void Start() {
        _text = GetComponent<Text>();
    }

    private void Update() {
        _text.text = "Saturation:\n" + WorldController.Instance.Player.Saturation;
    }
}
