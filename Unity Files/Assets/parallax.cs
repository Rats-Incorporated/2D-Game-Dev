using UnityEngine;

public class parallax : MonoBehaviour
{
    public Camera cam;
    public Transform subject;

    Vector2 startPosition;
    Vector2 camStartPosition;
    float startZ;
    float startY; // disable y parallax



    Vector2 travel => (Vector2)cam.transform.position - camStartPosition;

    float distanceFromSubject => transform.position.z - subject.position.z;
    float distanceFromCamera => transform.position.z - cam.transform.position.z;

    float clippingPlane => (distanceFromCamera > 0 ? cam.farClipPlane : cam.nearClipPlane);

    float parallaxFactor => Mathf.Abs(distanceFromCamera / clippingPlane);







    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
        camStartPosition = cam.transform.position;
        startZ = transform.position.z;
        startY = transform.position.y;

    }



    // Update is called once per frame
    void LateUpdate()
    {
        Vector2 newPos = transform.position = startPosition + travel * parallaxFactor;
        transform.position = new Vector3(newPos.x, startY, startZ);

    }
}
