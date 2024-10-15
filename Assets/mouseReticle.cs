using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseReticle : MonoBehaviour {
    public static mouseReticle instance;

    Camera cameraRef;
    public LayerMask groundLayer;
    Vector3 desiredPosition;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start() {
        cameraRef = Camera.main;
    }

    // Update is called once per frame
    void Update() {
        if (Physics.Raycast(cameraRef.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 600.0f, groundLayer.value))
        {
            desiredPosition = hit.point + Vector3.up * 0.15f;
        }
        transform.Rotate(0.0f, Time.deltaTime * 28.0f, 0.0f);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 10.0f);
    }
}
