using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TextMesh))]
[RequireComponent(typeof(Renderer))]
public class CountdownTextAnimation : MonoBehaviour {

    private TextMesh textMesh;
    private Renderer textRenderer;
    private IEnumerator currentLoop;
    private float initialDistance;
    private float animTime;
    private string finalMessage;


    public float distance;

    public void Awake()
    {
        textRenderer = GetComponent<Renderer>();
        textRenderer.enabled = false;
        textMesh = GetComponent<TextMesh>();
        initialDistance = this.transform.localPosition.z;
        currentLoop = null;
    }

    public void Update()
    {
        if (currentLoop != null)
        {
            animTime += Time.deltaTime * 2;
            if (animTime > 1)
            {
                animTime = 1;
            }
            Vector3 newPosition = transform.localPosition;
            newPosition.z = initialDistance + Mathf.Lerp(0, distance, animTime);
            this.transform.localPosition = newPosition;
            this.transform.localScale = new Vector3(1, 1, 1) * Mathf.Lerp(0, 1, animTime);
        }
    }

    public void Countdown(int seconds, string finalMessage)
    {
        if (currentLoop == null)
        {
            this.finalMessage = finalMessage;
            currentLoop = CountdownLoop(seconds);
            StartCoroutine(currentLoop);
        }
    }

    private IEnumerator CountdownLoop(int numberOfSeconds)
    {
        for(int count = numberOfSeconds - 1; count > 0; count--)
        {
            textRenderer.enabled = true;
            textMesh.text = "" + count;
            animTime = 0.0f;
            yield return new WaitForSeconds(1);
            yield return new WaitForEndOfFrame();
            textRenderer.enabled = false;
        }
        textRenderer.enabled = true;
        textMesh.text = finalMessage;
        animTime = 0.0f;
        yield return new WaitForSeconds(1);
        yield return new WaitForEndOfFrame();

        currentLoop = null;
        textRenderer.enabled = false;
    }
}
