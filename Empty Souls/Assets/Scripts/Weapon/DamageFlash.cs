using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    public SpriteRenderer sr;
    public Color flashColor = Color.red;
    public float flashTime = 0.1f;
    private Color originalColor;

    void Awake()
    {
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    private System.Collections.IEnumerator FlashRoutine()
    {
        sr.color = flashColor;
        yield return new WaitForSeconds(flashTime);
        sr.color = originalColor;
    }
}