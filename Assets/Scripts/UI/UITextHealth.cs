using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITextHealth : MonoBehaviour
{

    private Text _text;

    private void Start()
    {
        _text = GetComponent<Text>();
    }

    private void Update()
    {
        _text.text = "Health:\n" + WorldController.Instance.World.Player.Health;
    }
}
