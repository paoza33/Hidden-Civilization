using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SphereLevitation : MonoBehaviour
{
    public GameObject sphere;
    public GameObject socle;
    private float sphereDistance = 0.45f; // distance between start position and the highest Y position
    private float startPosition; // The Y sphere's value when is down (lowest position)

    private Vector3 newPos;

    private float closeEnough = 0.01f;

    private bool isDown = true; // if panel control is inactive

    private Vector3 velocity = Vector3.up;

    public Camera seconcaryCamera;

    private TextMeshProUGUI textInteract;

    public static SphereLevitation instance;
    private void Awake()
    {
        textInteract = GameObject.FindGameObjectWithTag("UIInteract").GetComponent<TextMeshProUGUI>();
        enabled = false;
        startPosition = sphere.transform.position.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            textInteract.enabled = true;
            sphere.GetComponent<FlickeringEmissive>().isReverse = false;
            sphere.GetComponent<FlickeringEmissive>().enabled = true;
            socle.GetComponent<FlickeringEmissive>().isReverse = false;
            socle.GetComponent<FlickeringEmissive>().enabled = true;
            newPos = new Vector3(sphere.transform.position.x, startPosition + sphereDistance, sphere.transform.position.z);
            isDown = false;
            enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            textInteract.enabled = false;
            sphere.GetComponent<FlickeringEmissive>().isReverse = true;
            sphere.GetComponent<FlickeringEmissive>().enabled = true;
            socle.GetComponent<FlickeringEmissive>().isReverse = true;
            socle.GetComponent<FlickeringEmissive>().enabled = true;
            isDown = true;
            newPos = new Vector3(sphere.transform.position.x, startPosition, sphere.transform.position.z);
        }
    }

    private void Update()
    {
        if (!isDown && Input.GetButtonDown("Interact"))
        {
            textInteract.enabled = false;
            SetCameraView.instance.SetNewPosCamera(seconcaryCamera.transform.position, seconcaryCamera.transform.rotation, true, false);
            sphere.GetComponent<PanelControl>().enabled = true;
            PlayerMovement.instance.StopMovement();
            enabled = false;
        }
    }

    void FixedUpdate()
    {
        sphere.transform.position = Vector3.SmoothDamp(sphere.transform.position, newPos, ref velocity, 10f * Time.deltaTime);

        if (Vector3.Distance(sphere.transform.position, newPos) < closeEnough && isDown)
            enabled = false;
    }
}
