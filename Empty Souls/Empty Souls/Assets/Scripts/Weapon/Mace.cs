using UnityEngine;

public class MaceWeapon : MonoBehaviour
{
    public Transform player;
    public float radius = 2f;
    public float speed = 180f; // градусов в сек.

    float angle;

    void Update()
    {
        angle += speed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;
        transform.position = player.position + new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;
    }
}