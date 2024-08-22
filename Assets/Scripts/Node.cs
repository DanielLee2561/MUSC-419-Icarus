using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node : MonoBehaviour
{
    private bool state;
    private float size;
    private Renderer renderer;
    private Material material;
    private float timeout;
    private float timein;
    public event Action actionEvent;
    private Coroutine alphaCoroutine;
    public void initialize(bool state, float size, Material material, float timeout, float timein)
    {
        this.state = state;
        this.size = size;
        this.material = new Material(material);
        this.timeout = timeout;
        this.timein = timein;

        renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        // Initialize Material
        renderer.material = material;
        material.SetFloat("_Alpha", 0f);

        // Initialize Size
        transform.localScale = new Vector3(size, size, size);

        // Initialize State
        if (state)
            action();
    }

    public bool getState() {
        return state;
    }

    public void setState(bool state) {
        this.state = state;
    }

    public void delete() {
        if (alphaCoroutine != null) {
            StopCoroutine(alphaCoroutine);
            alphaCoroutine = StartCoroutine(setAlphaCoroutine(0f, timeout));
        }
        Destroy(gameObject, timeout);
    }

    public void action() {
        alphaCoroutine = StartCoroutine(setAlphaCoroutine(1f, timein));
        if (actionEvent != null) {
            actionEvent.Invoke();
        }
    }

     private IEnumerator setAlphaCoroutine(float alphaTarget, float duration) {
        // Variable
        float timeStart = Time.time;
        float time = 0f;
        float lerp = 0f;
        float alphaCurrent = material.GetFloat("_Alpha");

        // Set Alpha (Lerp)
        while (time < duration) {
            time = Time.time - timeStart;
            lerp = time / duration;
            material.SetFloat("_Alpha", Mathf.Lerp(alphaCurrent, alphaTarget, lerp));
            yield return null;
        }

        // Set Alpha (Exact)
        material.SetFloat("_Alpha", alphaTarget);
    }
}
