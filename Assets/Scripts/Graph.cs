using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;

public enum State
{
    Incomplete,
    Complete,
    Delete
}

public class Graph : MonoBehaviour
{
    // Variable
    private State state;
    private int id;
    private List<GameObject> nodes;
    private List<GameObject> edges;
    private float timeout;
    private float timein;
    // Static
    public static float scaleMin = -15f;
    public static float scaleMax = 15f;
    // Bloom
    public static Volume volume;
    public static Bloom bloom;
    public float bloomLow = 1f;
    public float bloomHigh = 15f;
    public float bloomTimein = 1f;
    public float bloomTimeout = 1f;
    public static bool safety;

    public void initialize(int id) {
        // Variable
        state = State.Complete;
        this.id = id;
        nodes = new List<GameObject>();
        edges = new List<GameObject>();
        timeout = 0.5f;
        timein = 1.5f;

        // Bloom
        volume = FindObjectOfType<Volume>();
        volume.profile.TryGet<Bloom>(out bloom);
    }

    public void setBloom(float bloomLow, float bloomHigh, float bloomTimein, float bloomTimeout) {
        this.bloomLow = bloomLow;
        this.bloomHigh = bloomHigh;
        this.bloomTimein = bloomTimein;
        this.bloomTimeout = bloomTimeout;
    }

    public void setTime(float timein, float timeout) {
        this.timein = timein;
        this.timeout = timeout;
    }

    public int getId() {
        return id;
    }

    public State getState() {
        return state;
    }

    public GameObject addNode(bool s, float size, float x, float y, Material material)
    {
        // Variable
        GameObject node = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Node nodeScript = node.AddComponent<Node>();
        nodeScript.initialize(s, size, material, timeout, timein);
        nodes.Add(node);

        // Update Position
        float xScale = Mathf.Lerp(scaleMin, scaleMax, x / 100f);
        float yScale = Mathf.Lerp(scaleMin, scaleMax, y / 100f);
        node.transform.position = new Vector3(xScale, yScale, 0);

        // Update State
        if (s == false) {
            state = State.Incomplete;
        }

        return node;
    }

    public void addEdge(bool state, float width, GameObject nodeA, GameObject nodeB, Material material)
    {
        GameObject edge = new GameObject("Edge");
        Edge edgeScript = edge.AddComponent<Edge>();

        edgeScript.initialize(state, width, nodeA, nodeB, material, timeout, timein);
        edges.Add(edge);
    }

    public void delete()
    {
        if (state != State.Delete) {
            state = State.Delete;
            StartCoroutine(deleteCoroutine());
        }
    }

    public void action() {
        int index;
        for (index = 0; index < nodes.Count; index++) {
            GameObject node = nodes[index];
            Node nodeScript = node.GetComponent<Node>();
            if (nodeScript.getState() == false) {
                nodeScript.setState(true);
                nodeScript.action();
                break;
            }
        }

        // Update State
        if (index == nodes.Count - 1) {
            state = State.Complete;
        }
    }

    private IEnumerator setBloomCoroutine(float bloomTarget, float duration) {
        // Safety
        safety = true;

        // Variable
        float time = 0f;
        float lerp = 0f;
        float bloomStart = bloom.intensity.value;

        // Increase Bloom
        while (time < duration) {
            time += Time.deltaTime;
            lerp = time / duration;
            bloom.intensity.value = Mathf.Lerp(bloomStart, bloomTarget, lerp);
            yield return null;
        }

        // Safety
        safety = false;
    }

    private IEnumerator deleteCoroutine() {
        // Bloom Increase
        if (!safety) {
            yield return StartCoroutine(setBloomCoroutine(bloomHigh, bloomTimein));
        } else {
            yield return new WaitForSeconds(bloomTimein);
        }
        
        // Bloom Decrease
        if (!safety) {
            yield return StartCoroutine(setBloomCoroutine(bloomLow, bloomTimeout));
        } else {
            yield return new WaitForSeconds(bloomTimeout);
        }

        // Delete
        foreach (GameObject n in nodes) {
            n.GetComponent<Node>().delete();
        }
        foreach (GameObject e in edges) {
            e.GetComponent<Edge>().delete();
        }
        Destroy(gameObject, timeout);
    }
}
