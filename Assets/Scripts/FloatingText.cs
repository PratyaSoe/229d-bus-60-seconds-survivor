using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private TextMesh textMesh;
    private Color originalColor;
    private float fadeDuration;
    private float floatSpeed;

    public void StartFadeOut(float fadeDuration, float floatSpeed)
    {
        this.fadeDuration = fadeDuration;
        this.floatSpeed = floatSpeed;

        textMesh = GetComponent<TextMesh>();
        if (textMesh != null)
        {
            originalColor = textMesh.color;
            StartCoroutine(FadeOutCoroutine());
        }
    }

    private System.Collections.IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            // Fade out the text by reducing alpha
            float alpha = Mathf.Lerp(originalColor.a, 0, elapsedTime / fadeDuration);
            textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            // Move the text upwards
            transform.position += Vector3.up * floatSpeed * Time.deltaTime;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Destroy the object after fading out
        Destroy(gameObject);
    }
}