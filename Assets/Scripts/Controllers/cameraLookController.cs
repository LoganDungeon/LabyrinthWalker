using UnityEngine;
using System.Collections;

public class cameraLookController : MonoBehaviour
{

    public float Sensitivity = 5.0f;
    public float Smoothing = 2.0f;

    private bool _mouseLocked;
    private Vector2 _mouseLook;
    private Vector2 _smoothV;
    private GameObject _character;

    private void Start()
    {
        _character = this.transform.parent.gameObject;
        Cursor.lockState = CursorLockMode.Locked;
        _mouseLocked = true;
    }

    private void Update()
    {
        Vector2 md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        md = Vector2.Scale(md, new Vector2(Sensitivity * Smoothing, Sensitivity * Smoothing));
        _smoothV.x = Mathf.Lerp(_smoothV.x, md.x, 1 / Smoothing);
        _smoothV.y = Mathf.Lerp(_smoothV.y, md.y, 1 / Smoothing);
        _mouseLook += _smoothV;
        _mouseLook.y = Mathf.Clamp(_mouseLook.y, -90f, 90f);

        this.transform.localRotation = Quaternion.AngleAxis(-_mouseLook.y, Vector3.right);
        _character.transform.localRotation = Quaternion.AngleAxis(_mouseLook.x, _character.transform.up);

        if(Input.GetKeyDown("escape") && _mouseLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            _mouseLocked = false;
            Debug.Log("unlocked");
        }
        else if(Input.GetKeyDown("escape") && _mouseLocked == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            _mouseLocked = true;
            Debug.Log("locked");
        }
    }
}
