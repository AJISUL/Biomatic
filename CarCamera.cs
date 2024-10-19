using UnityEngine;

public class CarCamera : MonoBehaviour
{
    public Transform carTransform;
    public float distance = 10f;
    public float height = 5f;
    public float heightDamping = 2f;
    public float rotationDamping = 3f;

    void LateUpdate()
    {
        float wantedRotationAngle = carTransform.eulerAngles.y;
        float wantedHeight = carTransform.position.y + height;

        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        transform.position = carTransform.position;
        transform.position -= currentRotation * Vector3.forward * distance;

        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        transform.LookAt(carTransform);
    }
}
