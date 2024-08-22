using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour
{
    private bool state;
    private float width;
    private GameObject nodeA;
    private GameObject nodeB;
    private LineRenderer renderer;
    private Material material;
    private float timeout;
    private float timein;
    private Coroutine alphaCoroutine;

    public void initialize(bool state, float width, GameObject nodeA, GameObject nodeB, Material material, float timeout, float timein)
    {
        this.state = state;
        this.width = width;
        this.nodeA = nodeA;
        this.nodeB = nodeB;
        this.material = new Material(material);
        this.timeout = timeout;
        this.timein = timein;

        renderer = gameObject.AddComponent<LineRenderer>();
    }

    private void Start()
    {   
        // Variable
        Node aScript = nodeA.GetComponent<Node>();
        Node bScript = nodeB.GetComponent<Node>();

        // Subscribe
        aScript.actionEvent += action;
        bScript.actionEvent += action;

        // Initialize Material
        renderer.material = material;
        material.SetFloat("_Alpha", 0f);
        
        // Update Position
        renderer.positionCount = 2;
        renderer.startWidth = width;
        renderer.endWidth = width;
        renderer.SetPosition(0, nodeA.transform.position);
        renderer.SetPosition(1, nodeB.transform.position);
    }

    public void delete() {
        if (alphaCoroutine != null) {
            StopCoroutine(alphaCoroutine);
            alphaCoroutine = StartCoroutine(setAlphaCoroutine(0f, timeout));
        }
        Destroy(gameObject, timeout);
    }

    public void action() {
        Node aScript = nodeA.GetComponent<Node>();
        Node bScript = nodeB.GetComponent<Node>();
        if (aScript.getState() == true && bScript.getState() == true) {
            state = true;
            alphaCoroutine = StartCoroutine(setAlphaCoroutine(1f, timein));
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
