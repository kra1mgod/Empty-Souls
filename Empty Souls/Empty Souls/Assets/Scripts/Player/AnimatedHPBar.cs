using UnityEngine;
using UnityEngine.UI;

public class AnimatedHPBar : MonoBehaviour
{
    public Image backBar;   // ����� ���
    public Image fillBar;   // ������ (������� HP)
    public Image lossBar;   // ������� (�������� ������ HP)

    public float lossBarLerpSpeed = 2.5f; // �������� �������� ������� �������
    public float shakeDuration = 0.2f;
    public float shakeStrength = 8f;

    private float targetFill = 1f;
    private float currentShake = 0f;
    private Vector3 originalPos;

    void Start()
    {
        originalPos = transform.localPosition;
        fillBar.fillAmount = 1f;
        lossBar.fillAmount = 1f;
    }

    public void SetHP(float hp, float maxHp)
    {
        float normalized = Mathf.Clamp01(hp / maxHp);
        if (normalized < fillBar.fillAmount)
        {
            // ������� ����
            fillBar.fillAmount = normalized;
            currentShake = shakeDuration;
            lossBar.color = Color.red;
        }
        else
        {
            // ������� � ��� ������� �����
            fillBar.fillAmount = normalized;
            lossBar.fillAmount = normalized;
        }
        targetFill = normalized;
    }

    void Update()
    {
        // �������� ������� �������
        if (lossBar.fillAmount > fillBar.fillAmount)
        {
            lossBar.fillAmount = Mathf.MoveTowards(lossBar.fillAmount, fillBar.fillAmount, lossBarLerpSpeed * Time.unscaledDeltaTime);
            if (Mathf.Abs(lossBar.fillAmount - fillBar.fillAmount) < 0.001f)
                lossBar.color = new Color(0.8f, 0, 0, 0); // ������ ������� ������� ����������
        }
        else
        {
            lossBar.fillAmount = fillBar.fillAmount;
        }

        // ������ HP Bar
        if (currentShake > 0)
        {
            transform.localPosition = originalPos + (Vector3)Random.insideUnitCircle * shakeStrength;
            currentShake -= Time.unscaledDeltaTime;
            if (currentShake <= 0)
                transform.localPosition = originalPos;
        }
    }
}