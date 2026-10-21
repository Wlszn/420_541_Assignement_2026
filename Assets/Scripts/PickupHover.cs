using UnityEngine;

public class PickupHover : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 90f;
    [SerializeField] private float hoverHeight = 0.25f;
    [SerializeField] private float hoverSpeed = 2f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.localPosition;
    }

    private void Update()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f, Space.World);

        float offset = Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        transform.localPosition = startPosition + new Vector3(0f, offset, 0f);
    }
}
