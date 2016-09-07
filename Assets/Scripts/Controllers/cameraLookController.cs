using UnityEngine;
using System.Collections;

public class cameraLookController : MonoBehaviour {

    public float sensitivity = 5.0f;
    public float smoothing = 2.0f;

    bool mouseLocked;
    Vector2 mouseLook;
    Vector2 smoothV;
    GameObject character;

    void Start () {
        character = this.transform.parent.gameObject;
        Cursor.lockState = CursorLockMode.Locked;
        mouseLocked = true;
    }
	
	// Update is called once per frame
	void Update () {
        Vector2 md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1 / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1 / smoothing);
        mouseLook += smoothV;
        mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);

        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);

        if (Input.GetKeyDown("escape") && mouseLocked == true) {
            Cursor.lockState = CursorLockMode.None;
            mouseLocked = false;
            Debug.Log("unlocked");
        }
        else if(Input.GetKeyDown("escape") && mouseLocked == false) {
            Cursor.lockState = CursorLockMode.Locked;
            mouseLocked = true;
            Debug.Log("locked");
        }
    }
}
