using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Key : MonoBehaviour
{
    [SerializeField] private AudioSource keySound;
    private SpriteRenderer spriteRenderer;
    private Collider2D collider2D;
    public delegate void KeyCollected();
    public static event KeyCollected keyCollectedInfo;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagManager.PLAYER_TAG))
        {
            keySound.Play();
            if (keyCollectedInfo != null){
                keyCollectedInfo();
            }
            StartCoroutine(FadeOutAndDestroy());
        }
    }

    private IEnumerator FadeOutAndDestroy()
    {
        // Disable collider and make key non-interactive
        collider2D.enabled = false;

        // Fade out effect
        float duration = keySound.clip.length;
        float currentTime = 0f;

        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / duration);
            spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}

