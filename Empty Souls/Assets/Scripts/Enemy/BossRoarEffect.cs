using UnityEngine;

public class BossRoarEffect : MonoBehaviour
{
    public AudioClip roarClip; // Можешь назначить звук ревa в инспекторе
    public float roarDuration = 1.5f;

    public void PlayRoar()
    {
        // Проиграть звук, запустить анимацию или эффект
        Debug.Log("Boss Roar!");
        if (roarClip != null)
        {
            AudioSource.PlayClipAtPoint(roarClip, transform.position);
        }
        // Тут можешь запустить анимацию или визуальный эффект
    }
}