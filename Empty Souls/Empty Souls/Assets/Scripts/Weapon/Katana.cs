using UnityEngine;

public class KatanaWeapon : MonoBehaviour
{
    public GameObject wavePrefab;
    public float fireInterval = 1f;
    float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= fireInterval)
        {
            timer = 0f;
            FireWave();
        }
    }

    void FireWave()
    {
        // ����� ����� ����� �� ����������� ������, ��� �� 3 ������������ ��� ������� �����
        Instantiate(wavePrefab, transform.position, transform.rotation);
    }
}