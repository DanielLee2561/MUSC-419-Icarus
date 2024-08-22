using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void addGraphCreation(int id, Material m);

public struct Pair
{
    public int x;
    public int y;
    public Pair(int x, int y) {
        this.x = x;
        this.y = y;
    }
}

public class GraphManager : MonoBehaviour
{
    // OSC
    public OSC osc;
    public string oscDelete;
    public string oscAdd;
    public string oscAction;

    // Variable
    private HashSet<GameObject> graphs;
    List<addGraphCreation> creation;
    public Material white;
    public Material red;
    public Material green;
    public Material blue;
    public Material yellow;
    public Material purple;
    public Material teal;
    private float nodeTiny;
    private float nodeSmall;
    private float nodeMedium;
    private float nodeLarge;
    private float nodeMassive;
    private float edgeTiny;
    private float edgeSmall;
    private float edgeMedium;
    private float randomScale;

    void Start()
    {
        // OSC
        if (osc) {
            osc.SetAddressHandler(oscDelete, deleteOsc);
            osc.SetAddressHandler(oscAdd, addGraphOsc);
            osc.SetAddressHandler(oscAction, action);
        }

        // Variable
        graphs = new HashSet<GameObject>();
        initializeCreation();
        nodeTiny = 0.3f;
        nodeSmall = 0.6f;
        nodeMedium = 1.2f;
        nodeLarge = 3.6f;
        nodeMassive = 15f;
        edgeTiny = 0.02f;
        edgeSmall = 0.04f;
        edgeMedium = 0.06f;
        randomScale = 10f;
    }

    private float random(float n, float scale) {
        return Random.Range(n - scale, n + scale);
    }

    private void deleteOsc(OscMessage graph) {
        int id = graph.GetInt(0);
        if (id == 0) {
            deleteAll();
        }
        else {
            GameObject g = lookup(id);
            delete(g);
        }
    }

    private void deleteAll() {
        Graph gScript;
        foreach (GameObject g in graphs) {
            gScript = g.GetComponent<Graph>();
            gScript.delete();
        }
        graphs.Clear();
    }

    private void delete(GameObject graph) {
        if (graph != null) {
            Graph gScript = graph.GetComponent<Graph>();
            gScript.delete();
            graphs.Remove(graph);
        }
    }

    private GameObject lookup(int id)
    {
        Graph gScript;
        foreach (GameObject g in graphs) {
            gScript = g.GetComponent<Graph>();
            if (id == gScript.getId()) {
                return g;
            }
        }
        return null;
    }

    private Material materialRandom()
    {
        int random = Random.Range(0, 6);
        switch (random) {
            case 0:
                return red;
            case 1:
                return green;
            case 2:
                return blue;
            case 3:
                return yellow;
            case 4:
                return purple;
            case 5:
                return teal;
            default:
                return white;
        }
    }

    private void addGraphExplorationNode(int id)
    {
        // Graph
        Graph graphScript = addGraph(id);

        // Create List
        List<Pair> pairs = new List<Pair>();
        for (int x = 0; x < 5; x++) {
            for (int y = 0; y < 5; y++) {
                pairs.Add(new Pair(x * 20, y * 20));
            }
        }

        // Shuffle List
        pairs.Sort((a, b) => Random.Range(-1, 2));

        // Create Node
        int xRandom, yRandom;
        for (int i = 0; i < pairs.Count; i++) {
            for (int j = 0; j < 3; j++) {
                xRandom = Random.Range(pairs[i].x, pairs[i].x + 20);
                yRandom = Random.Range(pairs[i].y, pairs[i].y + 20);
                graphScript.addNode(false, nodeTiny, xRandom, yRandom, white);

                Debug.Log("x " + pairs[i].x + "," + "y " + pairs[i].y);
            }
        }
    }

    private void addGraphExplorationEdge(int id)
    {
        // Graph
        Graph graphScript = addGraph(id);

        // Create List
        List<Pair> pairs = new List<Pair>();
        for (int x = 0; x < 5; x++) {
            for (int y = 0; y < 5; y++) {
                pairs.Add(new Pair(x * 20, y * 20));
            }
        }

        // Shuffle List
        pairs.Sort((a, b) => Random.Range(-1, 2));

        // Create Node
        int xRandom, yRandom;
        GameObject nodeCurrent, nodePrevious;

        // Node 0
        xRandom = Random.Range(pairs[0].x, pairs[0].x + 20);
        yRandom = Random.Range(pairs[0].y, pairs[0].y + 20);
        nodePrevious = graphScript.addNode(false, nodeTiny, xRandom, yRandom, white);

        // Node 1+
        for (int i = 1; i < pairs.Count; i++) {
            // Node
            xRandom = Random.Range(pairs[i].x, pairs[i].x + 20);
            yRandom = Random.Range(pairs[i].y, pairs[i].y + 20);
            nodeCurrent = graphScript.addNode(false, nodeTiny, xRandom, yRandom, white);

            // Edge
            graphScript.addEdge(false, edgeTiny, nodePrevious, nodeCurrent, white);
            nodePrevious = nodeCurrent;
        }
    }

    private void addGraphDesignHorizontal(int id)
    {
        // Graph
        Graph graphScript = addGraph(id);

        // Node
        GameObject lineA1 = graphScript.addNode(false, nodeSmall, 0f, 100f, red);
        GameObject lineA2 = graphScript.addNode(false, nodeSmall, 100f, 100f, red);
        GameObject lineB1 = graphScript.addNode(false, nodeSmall, 0f, 80f, red);
        GameObject lineB2 = graphScript.addNode(false, nodeSmall, 100f, 80f, red);
        GameObject lineC1 = graphScript.addNode(false, nodeSmall, 0f, 60f, red);
        GameObject lineC2 = graphScript.addNode(false, nodeSmall, 100f, 60f, red);
        GameObject lineD1 = graphScript.addNode(false, nodeSmall, 0f, 40f, red);
        GameObject lineD2 = graphScript.addNode(false, nodeSmall, 100f, 40f, red);
        GameObject lineE1 = graphScript.addNode(false, nodeSmall, 0f, 20f, red);
        GameObject lineE2 = graphScript.addNode(false, nodeSmall, 100f, 20f, red);
        GameObject lineF1 = graphScript.addNode(false, nodeSmall, 0f, 0f, red);
        GameObject lineF2 = graphScript.addNode(false, nodeSmall, 100f, 0f, red);

        // Edge
        graphScript.addEdge(false, edgeSmall, lineA1, lineA2, red);
        graphScript.addEdge(false, edgeSmall, lineB1, lineB2, red);
        graphScript.addEdge(false, edgeSmall, lineC1, lineC2, red);
        graphScript.addEdge(false, edgeSmall, lineD1, lineD2, red);
        graphScript.addEdge(false, edgeSmall, lineE1, lineE2, red);
        graphScript.addEdge(false, edgeSmall, lineF1, lineF2, red);
    }

    private void addGraphDesignVertical(int id)
    {
        // Graph
        Graph graphScript = addGraph(id);

        // Node
        GameObject lineA1 = graphScript.addNode(false, nodeSmall, 0f, 100f, green);
        GameObject lineA2 = graphScript.addNode(false, nodeSmall, 0f, 0f, green);
        GameObject lineB1 = graphScript.addNode(false, nodeSmall, 20f, 100f, green);
        GameObject lineB2 = graphScript.addNode(false, nodeSmall, 20f, 0f, green);
        GameObject lineC1 = graphScript.addNode(false, nodeSmall, 40f, 100f, green);
        GameObject lineC2 = graphScript.addNode(false, nodeSmall, 40f, 0f, green);
        GameObject lineD1 = graphScript.addNode(false, nodeSmall, 60f, 100f, green);
        GameObject lineD2 = graphScript.addNode(false, nodeSmall, 60f, 0f, green);
        GameObject lineE1 = graphScript.addNode(false, nodeSmall, 80f, 100f, green);
        GameObject lineE2 = graphScript.addNode(false, nodeSmall, 80f, 0f, green);
        GameObject lineF1 = graphScript.addNode(false, nodeSmall, 100f, 100f, green);
        GameObject lineF2 = graphScript.addNode(false, nodeSmall, 100f, 0f, green);

        // Edge
        graphScript.addEdge(false, edgeSmall, lineA1, lineA2, green);
        graphScript.addEdge(false, edgeSmall, lineB1, lineB2, green);
        graphScript.addEdge(false, edgeSmall, lineC1, lineC2, green);
        graphScript.addEdge(false, edgeSmall, lineD1, lineD2, green);
        graphScript.addEdge(false, edgeSmall, lineE1, lineE2, green);
        graphScript.addEdge(false, edgeSmall, lineF1, lineF2, green);
    }

    private void addGraphDesignHorizontalCross(int id)
    {
        // Graph
        Graph graphScript = addGraph(id);

        // Node
        GameObject lineA1 = graphScript.addNode(false, nodeSmall, 0f, 100f, blue);
        GameObject lineA2 = graphScript.addNode(false, nodeSmall, 100f, 100f, blue);
        GameObject lineB1 = graphScript.addNode(false, nodeSmall, 0f, 80f, blue);
        GameObject lineB2 = graphScript.addNode(false, nodeSmall, 100f, 80f, blue);
        GameObject lineC1 = graphScript.addNode(false, nodeSmall, 0f, 60f, blue);
        GameObject lineC2 = graphScript.addNode(false, nodeSmall, 100f, 60f, blue);
        GameObject lineD1 = graphScript.addNode(false, nodeSmall, 0f, 40f, blue);
        GameObject lineD2 = graphScript.addNode(false, nodeSmall, 100f, 40f, blue);
        GameObject lineE1 = graphScript.addNode(false, nodeSmall, 0f, 20f, blue);
        GameObject lineE2 = graphScript.addNode(false, nodeSmall, 100f, 20f, blue);
        GameObject lineF1 = graphScript.addNode(false, nodeSmall, 0f, 0f, blue);
        GameObject lineF2 = graphScript.addNode(false, nodeSmall, 100f, 0f, blue);

        // Edge
        graphScript.addEdge(false, edgeSmall, lineA1, lineB2, blue);
        graphScript.addEdge(false, edgeSmall, lineB1, lineC2, blue);
        graphScript.addEdge(false, edgeSmall, lineC1, lineD2, blue);
        graphScript.addEdge(false, edgeSmall, lineD1, lineE2, blue);
        graphScript.addEdge(false, edgeSmall, lineE1, lineF2, blue);

        graphScript.addEdge(false, edgeSmall, lineA2, lineB1, blue);
        graphScript.addEdge(false, edgeSmall, lineB2, lineC1, blue);
        graphScript.addEdge(false, edgeSmall, lineC2, lineD1, blue);
        graphScript.addEdge(false, edgeSmall, lineD2, lineE1, blue);
        graphScript.addEdge(false, edgeSmall, lineE2, lineF1, blue);
    }

    private void addGraphDesignVerticalCross(int id)
    {
        // Graph
        Graph graphScript = addGraph(id);

        // Node
        GameObject lineA1 = graphScript.addNode(false, nodeSmall, 0f, 100f, red);
        GameObject lineA2 = graphScript.addNode(false, nodeSmall, 0f, 0f, red);
        GameObject lineB1 = graphScript.addNode(false, nodeSmall, 20f, 100f, red);
        GameObject lineB2 = graphScript.addNode(false, nodeSmall, 20f, 0f, red);
        GameObject lineC1 = graphScript.addNode(false, nodeSmall, 40f, 100f, red);
        GameObject lineC2 = graphScript.addNode(false, nodeSmall, 40f, 0f, red);
        GameObject lineD1 = graphScript.addNode(false, nodeSmall, 60f, 100f, red);
        GameObject lineD2 = graphScript.addNode(false, nodeSmall, 60f, 0f, red);
        GameObject lineE1 = graphScript.addNode(false, nodeSmall, 80f, 100f, red);
        GameObject lineE2 = graphScript.addNode(false, nodeSmall, 80f, 0f, red);
        GameObject lineF1 = graphScript.addNode(false, nodeSmall, 100f, 100f, red);
        GameObject lineF2 = graphScript.addNode(false, nodeSmall, 100f, 0f, red);

        // Edge
        graphScript.addEdge(false, edgeSmall, lineA1, lineB2, red);
        graphScript.addEdge(false, edgeSmall, lineB1, lineC2, red);
        graphScript.addEdge(false, edgeSmall, lineC1, lineD2, red);
        graphScript.addEdge(false, edgeSmall, lineD1, lineE2, red);
        graphScript.addEdge(false, edgeSmall, lineE1, lineF2, red);

        graphScript.addEdge(false, edgeSmall, lineA2, lineB1, red);
        graphScript.addEdge(false, edgeSmall, lineB2, lineC1, red);
        graphScript.addEdge(false, edgeSmall, lineC2, lineD1, red);
        graphScript.addEdge(false, edgeSmall, lineD2, lineE1, red);
        graphScript.addEdge(false, edgeSmall, lineE2, lineF1, red);
    }

    private void addGraphDesignBox(int id)
    {
        // Graph
        Graph graphScript = addGraph(id);

        // Node
        GameObject boxA1 = graphScript.addNode(false, nodeSmall, 0f, 0f, green);
        GameObject boxA2 = graphScript.addNode(false, nodeSmall, 0f, 100f, green);
        GameObject boxA3 = graphScript.addNode(false, nodeSmall, 100f, 100f, green);
        GameObject boxA4 = graphScript.addNode(false, nodeSmall, 100f, 0f, green);

        GameObject boxB1 = graphScript.addNode(false, nodeSmall, 20f, 20f, green);
        GameObject boxB2 = graphScript.addNode(false, nodeSmall, 20f, 80f, green);
        GameObject boxB3 = graphScript.addNode(false, nodeSmall, 80f, 80f, green);
        GameObject boxB4 = graphScript.addNode(false, nodeSmall, 80f, 20f, green);

        GameObject boxC1 = graphScript.addNode(false, nodeSmall, 40f, 40f, green);
        GameObject boxC2 = graphScript.addNode(false, nodeSmall, 40f, 60f, green);
        GameObject boxC3 = graphScript.addNode(false, nodeSmall, 60f, 60f, green);
        GameObject boxC4 = graphScript.addNode(false, nodeSmall, 60f, 40f, green);

        // Edge
        graphScript.addEdge(false, edgeSmall, boxA1, boxA2, green);
        graphScript.addEdge(false, edgeSmall, boxA2, boxA3, green);
        graphScript.addEdge(false, edgeSmall, boxA3, boxA4, green);
        graphScript.addEdge(false, edgeSmall, boxA4, boxA1, green);

        graphScript.addEdge(false, edgeSmall, boxB1, boxB2, green);
        graphScript.addEdge(false, edgeSmall, boxB2, boxB3, green);
        graphScript.addEdge(false, edgeSmall, boxB3, boxB4, green);
        graphScript.addEdge(false, edgeSmall, boxB4, boxB1, green);

        graphScript.addEdge(false, edgeSmall, boxC1, boxC2, green);
        graphScript.addEdge(false, edgeSmall, boxC2, boxC3, green);
        graphScript.addEdge(false, edgeSmall, boxC3, boxC4, green);
        graphScript.addEdge(false, edgeSmall, boxC4, boxC1, green);
    }

    private void addGraphDesignGrid(int id)
    {
        // Graph
        Graph graphScript = addGraph(id);

        // Node
        GameObject lineA1 = graphScript.addNode(false, nodeSmall, 0f, 100f, blue);
        GameObject lineA2 = graphScript.addNode(false, nodeSmall, 20f, 100f, blue);
        GameObject lineA3 = graphScript.addNode(false, nodeSmall, 40f, 100f, blue);
        GameObject lineA4 = graphScript.addNode(false, nodeSmall, 60f, 100f, blue);
        GameObject lineA5 = graphScript.addNode(false, nodeSmall, 80f, 100f, blue);
        GameObject lineA6 = graphScript.addNode(false, nodeSmall, 100f, 100f, blue);

        GameObject lineB1 = graphScript.addNode(false, nodeSmall, 0f, 80f, blue);
        GameObject lineB2 = graphScript.addNode(false, nodeSmall, 20f, 80f, blue);
        GameObject lineB3 = graphScript.addNode(false, nodeSmall, 40f, 80f, blue);
        GameObject lineB4 = graphScript.addNode(false, nodeSmall, 60f, 80f, blue);
        GameObject lineB5 = graphScript.addNode(false, nodeSmall, 80f, 80f, blue);
        GameObject lineB6 = graphScript.addNode(false, nodeSmall, 100f, 80f, blue);

        GameObject lineC1 = graphScript.addNode(false, nodeSmall, 0f, 60f, blue);
        GameObject lineC2 = graphScript.addNode(false, nodeSmall, 20f, 60f, blue);
        GameObject lineC3 = graphScript.addNode(false, nodeSmall, 40f, 60f, blue);
        GameObject lineC4 = graphScript.addNode(false, nodeSmall, 60f, 60f, blue);
        GameObject lineC5 = graphScript.addNode(false, nodeSmall, 80f, 60f, blue);
        GameObject lineC6 = graphScript.addNode(false, nodeSmall, 100f, 60f, blue);

        GameObject lineD1 = graphScript.addNode(false, nodeSmall, 0f, 40f, blue);
        GameObject lineD2 = graphScript.addNode(false, nodeSmall, 20f, 40f, blue);
        GameObject lineD3 = graphScript.addNode(false, nodeSmall, 40f, 40f, blue);
        GameObject lineD4 = graphScript.addNode(false, nodeSmall, 60f, 40f, blue);
        GameObject lineD5 = graphScript.addNode(false, nodeSmall, 80f, 40f, blue);
        GameObject lineD6 = graphScript.addNode(false, nodeSmall, 100f, 40f, blue);

        GameObject lineE1 = graphScript.addNode(false, nodeSmall, 0f, 20f, blue);
        GameObject lineE2 = graphScript.addNode(false, nodeSmall, 20f, 20f, blue);
        GameObject lineE3 = graphScript.addNode(false, nodeSmall, 40f, 20f, blue);
        GameObject lineE4 = graphScript.addNode(false, nodeSmall, 60f, 20f, blue);
        GameObject lineE5 = graphScript.addNode(false, nodeSmall, 80f, 20f, blue);
        GameObject lineE6 = graphScript.addNode(false, nodeSmall, 100f, 20f, blue);

        GameObject lineF1 = graphScript.addNode(false, nodeSmall, 0f, 0f, blue);
        GameObject lineF2 = graphScript.addNode(false, nodeSmall, 20f, 0f, blue);
        GameObject lineF3 = graphScript.addNode(false, nodeSmall, 40f, 0f, blue);
        GameObject lineF4 = graphScript.addNode(false, nodeSmall, 60f, 0f, blue);
        GameObject lineF5 = graphScript.addNode(false, nodeSmall, 80f, 0f, blue);
        GameObject lineF6 = graphScript.addNode(false, nodeSmall, 100f, 0f, blue);

        // Edge
        graphScript.addEdge(false, edgeSmall, lineA1, lineA2, blue);
        graphScript.addEdge(false, edgeSmall, lineA2, lineA3, blue);
        graphScript.addEdge(false, edgeSmall, lineA3, lineA4, blue);
        graphScript.addEdge(false, edgeSmall, lineA4, lineA5, blue);
        graphScript.addEdge(false, edgeSmall, lineA5, lineA6, blue);

        graphScript.addEdge(false, edgeSmall, lineA1, lineB1, blue);
        graphScript.addEdge(false, edgeSmall, lineA2, lineB2, blue);
        graphScript.addEdge(false, edgeSmall, lineA3, lineB3, blue);
        graphScript.addEdge(false, edgeSmall, lineA4, lineB4, blue);
        graphScript.addEdge(false, edgeSmall, lineA5, lineB5, blue);
        graphScript.addEdge(false, edgeSmall, lineA6, lineB6, blue);

        graphScript.addEdge(false, edgeSmall, lineB1, lineB2, blue);
        graphScript.addEdge(false, edgeSmall, lineB2, lineB3, blue);
        graphScript.addEdge(false, edgeSmall, lineB3, lineB4, blue);
        graphScript.addEdge(false, edgeSmall, lineB4, lineB5, blue);
        graphScript.addEdge(false, edgeSmall, lineB5, lineB6, blue);

        graphScript.addEdge(false, edgeSmall, lineB1, lineC1, blue);
        graphScript.addEdge(false, edgeSmall, lineB2, lineC2, blue);
        graphScript.addEdge(false, edgeSmall, lineB3, lineC3, blue);
        graphScript.addEdge(false, edgeSmall, lineB4, lineC4, blue);
        graphScript.addEdge(false, edgeSmall, lineB5, lineC5, blue);
        graphScript.addEdge(false, edgeSmall, lineB6, lineC6, blue);

        graphScript.addEdge(false, edgeSmall, lineC1, lineC2, blue);
        graphScript.addEdge(false, edgeSmall, lineC2, lineC3, blue);
        graphScript.addEdge(false, edgeSmall, lineC3, lineC4, blue);
        graphScript.addEdge(false, edgeSmall, lineC4, lineC5, blue);
        graphScript.addEdge(false, edgeSmall, lineC5, lineC6, blue);

        graphScript.addEdge(false, edgeSmall, lineC1, lineD1, blue);
        graphScript.addEdge(false, edgeSmall, lineC2, lineD2, blue);
        graphScript.addEdge(false, edgeSmall, lineC3, lineD3, blue);
        graphScript.addEdge(false, edgeSmall, lineC4, lineD4, blue);
        graphScript.addEdge(false, edgeSmall, lineC5, lineD5, blue);
        graphScript.addEdge(false, edgeSmall, lineC6, lineD6, blue);

        graphScript.addEdge(false, edgeSmall, lineD1, lineD2, blue);
        graphScript.addEdge(false, edgeSmall, lineD2, lineD3, blue);
        graphScript.addEdge(false, edgeSmall, lineD3, lineD4, blue);
        graphScript.addEdge(false, edgeSmall, lineD4, lineD5, blue);
        graphScript.addEdge(false, edgeSmall, lineD5, lineD6, blue);

        graphScript.addEdge(false, edgeSmall, lineD1, lineE1, blue);
        graphScript.addEdge(false, edgeSmall, lineD2, lineE2, blue);
        graphScript.addEdge(false, edgeSmall, lineD3, lineE3, blue);
        graphScript.addEdge(false, edgeSmall, lineD4, lineE4, blue);
        graphScript.addEdge(false, edgeSmall, lineD5, lineE5, blue);
        graphScript.addEdge(false, edgeSmall, lineD6, lineE6, blue);

        graphScript.addEdge(false, edgeSmall, lineE1, lineE2, blue);
        graphScript.addEdge(false, edgeSmall, lineE2, lineE3, blue);
        graphScript.addEdge(false, edgeSmall, lineE3, lineE4, blue);
        graphScript.addEdge(false, edgeSmall, lineE4, lineE5, blue);
        graphScript.addEdge(false, edgeSmall, lineE5, lineE6, blue);

        graphScript.addEdge(false, edgeSmall, lineE1, lineF1, blue);
        graphScript.addEdge(false, edgeSmall, lineE2, lineF2, blue);
        graphScript.addEdge(false, edgeSmall, lineE3, lineF3, blue);
        graphScript.addEdge(false, edgeSmall, lineE4, lineF4, blue);
        graphScript.addEdge(false, edgeSmall, lineE5, lineF5, blue);
        graphScript.addEdge(false, edgeSmall, lineE6, lineF6, blue);

        graphScript.addEdge(false, edgeSmall, lineF1, lineF2, blue);
        graphScript.addEdge(false, edgeSmall, lineF2, lineF3, blue);
        graphScript.addEdge(false, edgeSmall, lineF3, lineF4, blue);
        graphScript.addEdge(false, edgeSmall, lineF4, lineF5, blue);
        graphScript.addEdge(false, edgeSmall, lineF5, lineF6, blue);
    }

    private void addGraphCreationMoon(int id, Material m)
    {
        // Graph
        Graph graphScript = addGraph(id);

        // Node
        GameObject leftA = graphScript.addNode(false, nodeMedium, 30f, 10f, m);
        GameObject leftB = graphScript.addNode(false, nodeMedium, 38f, 30f, m);
        GameObject leftC = graphScript.addNode(false, nodeMedium, 40f, 50f, m);
        GameObject leftD = graphScript.addNode(false, nodeMedium, 38f, 70f, m);
        GameObject leftE = graphScript.addNode(false, nodeMedium, 30f, 90f, m);

        GameObject rightA = graphScript.addNode(false, nodeMedium, 45f, 80f, m);
        GameObject rightB = graphScript.addNode(false, nodeMedium, 53f, 70f, m);
        GameObject rightC = graphScript.addNode(false, nodeMedium, 57f, 60f, m);
        GameObject rightD = graphScript.addNode(false, nodeMedium, 60f, 50f, m);
        GameObject rightE = graphScript.addNode(false, nodeMedium, 57f, 40f, m);
        GameObject rightF = graphScript.addNode(false, nodeMedium, 53f, 30f, m);
        GameObject rightG = graphScript.addNode(false, nodeMedium, 45f, 20f, m);

        // Edge
        graphScript.addEdge(false, edgeMedium, leftA, leftB, m);
        graphScript.addEdge(false, edgeMedium, leftB, leftC, m);
        graphScript.addEdge(false, edgeMedium, leftC, leftD, m);
        graphScript.addEdge(false, edgeMedium, leftD, leftE, m);
        graphScript.addEdge(false, edgeMedium, leftE, rightA, m);

        graphScript.addEdge(false, edgeMedium, rightA, rightB, m);
        graphScript.addEdge(false, edgeMedium, rightB, rightC, m);
        graphScript.addEdge(false, edgeMedium, rightC, rightD, m);
        graphScript.addEdge(false, edgeMedium, rightD, rightE, m);
        graphScript.addEdge(false, edgeMedium, rightE, rightF, m);
        graphScript.addEdge(false, edgeMedium, rightF, rightG, m);
        graphScript.addEdge(false, edgeMedium, rightG, leftA, m);
    }

    private void addGraphCreationDiamond(int id, Material m)
    {
        // Graph
        Graph graphScript = addGraph(id);

        // Node
        GameObject nodeA = graphScript.addNode(false, nodeMedium, 50f, 0f, m);
        GameObject nodeB = graphScript.addNode(false, nodeMedium, 25f, 50f, m);
        GameObject nodeC = graphScript.addNode(false, nodeMedium, 50f, 100f, m);
        GameObject nodeD = graphScript.addNode(false, nodeMedium, 75f, 50f, m);
        GameObject nodeE = graphScript.addNode(false, nodeMedium, 45f, 45f, m);

        // Edge
        graphScript.addEdge(false, edgeMedium, nodeA, nodeB, m);
        graphScript.addEdge(false, edgeMedium, nodeB, nodeC, m);
        graphScript.addEdge(false, edgeMedium, nodeC, nodeD, m);
        graphScript.addEdge(false, edgeMedium, nodeD, nodeA, m);

        graphScript.addEdge(false, edgeMedium, nodeE, nodeA, m);
        graphScript.addEdge(false, edgeMedium, nodeE, nodeB, m);
        graphScript.addEdge(false, edgeMedium, nodeE, nodeC, m);
        graphScript.addEdge(false, edgeMedium, nodeE, nodeD, m);
    }

    private void addGraphCreationFan(int id, Material m)
    {
        // Graph
        Graph graphScript = addGraph(id);

        // Node
        GameObject nodeA = graphScript.addNode(false, nodeMedium, 0f, 50f, m);
        GameObject nodeB = graphScript.addNode(false, nodeMedium, 25f, 75f, m);
        GameObject nodeC = graphScript.addNode(false, nodeMedium, 50f, 100f, m);
        GameObject nodeD = graphScript.addNode(false, nodeMedium, 75f, 75f, m);
        GameObject nodeE = graphScript.addNode(false, nodeMedium, 100f, 50f, m);
        GameObject nodeF = graphScript.addNode(false, nodeMedium, 75f, 25f, m);
        GameObject nodeG = graphScript.addNode(false, nodeMedium, 50f, 0f, m);
        GameObject nodeH = graphScript.addNode(false, nodeMedium, 25f, 25f, m);

        // Edge
        graphScript.addEdge(false, edgeMedium, nodeA, nodeB, m);
        graphScript.addEdge(false, edgeMedium, nodeB, nodeC, m);
        graphScript.addEdge(false, edgeMedium, nodeC, nodeD, m);
        graphScript.addEdge(false, edgeMedium, nodeD, nodeE, m);
        graphScript.addEdge(false, edgeMedium, nodeE, nodeF, m);
        graphScript.addEdge(false, edgeMedium, nodeF, nodeG, m);
        graphScript.addEdge(false, edgeMedium, nodeG, nodeH, m);
        graphScript.addEdge(false, edgeMedium, nodeH, nodeA, m);

        graphScript.addEdge(false, edgeMedium, nodeE, nodeA, m);
        graphScript.addEdge(false, edgeMedium, nodeE, nodeB, m);
        graphScript.addEdge(false, edgeMedium, nodeE, nodeC, m);
        graphScript.addEdge(false, edgeMedium, nodeE, nodeD, m);
        graphScript.addEdge(false, edgeMedium, nodeE, nodeE, m);
        graphScript.addEdge(false, edgeMedium, nodeE, nodeF, m);
        graphScript.addEdge(false, edgeMedium, nodeE, nodeG, m);
        graphScript.addEdge(false, edgeMedium, nodeE, nodeH, m);
    }

    private void addGraphCreationWheel(int id, Material m)
    {
        // Graph
        Graph graphScript = addGraph(id);

        // Node
        GameObject nodeA = graphScript.addNode(false, nodeMedium, 0f, 50f, m);
        GameObject nodeB = graphScript.addNode(false, nodeMedium, 15f, 85f, m);
        GameObject nodeC = graphScript.addNode(false, nodeMedium, 50f, 100f, m);
        GameObject nodeD = graphScript.addNode(false, nodeMedium, 85f, 85f, m);
        GameObject nodeE = graphScript.addNode(false, nodeMedium, 100f, 50f, m);
        GameObject nodeF = graphScript.addNode(false, nodeMedium, 85f, 15f, m);
        GameObject nodeG = graphScript.addNode(false, nodeMedium, 50f, 0f, m);
        GameObject nodeH = graphScript.addNode(false, nodeMedium, 15f, 15f, m);

        GameObject nodeI = graphScript.addNode(false, nodeMedium, 50f, 50f, m);

        // Edge
        graphScript.addEdge(false, edgeMedium, nodeA, nodeB, m);
        graphScript.addEdge(false, edgeMedium, nodeB, nodeC, m);
        graphScript.addEdge(false, edgeMedium, nodeC, nodeD, m);
        graphScript.addEdge(false, edgeMedium, nodeD, nodeE, m);
        graphScript.addEdge(false, edgeMedium, nodeE, nodeF, m);
        graphScript.addEdge(false, edgeMedium, nodeF, nodeG, m);
        graphScript.addEdge(false, edgeMedium, nodeG, nodeH, m);
        graphScript.addEdge(false, edgeMedium, nodeH, nodeA, m);

        graphScript.addEdge(false, edgeMedium, nodeI, nodeA, m);
        graphScript.addEdge(false, edgeMedium, nodeI, nodeB, m);
        graphScript.addEdge(false, edgeMedium, nodeI, nodeC, m);
        graphScript.addEdge(false, edgeMedium, nodeI, nodeD, m);
        graphScript.addEdge(false, edgeMedium, nodeI, nodeE, m);
        graphScript.addEdge(false, edgeMedium, nodeI, nodeF, m);
        graphScript.addEdge(false, edgeMedium, nodeI, nodeG, m);
        graphScript.addEdge(false, edgeMedium, nodeI, nodeH, m);
    }

    private void addGraphCreationSword(int id, Material m)
    {
        // Graph
        Graph graphScript = addGraph(id);

        // Node
        GameObject baseA = graphScript.addNode(false, nodeMedium, 45f, 20f, m);
        GameObject baseB = graphScript.addNode(false, nodeMedium, 45f, 0f, m);
        GameObject baseC = graphScript.addNode(false, nodeMedium, 55f, 0f, m);
        GameObject baseD = graphScript.addNode(false, nodeMedium, 55f, 20f, m);

        GameObject hiltA = graphScript.addNode(false, nodeMedium, 30f, 20f, m);
        GameObject hiltB = graphScript.addNode(false, nodeMedium, 30f, 25f, m);
        GameObject hiltC = graphScript.addNode(false, nodeMedium, 45f, 25f, m);
        GameObject hiltD = graphScript.addNode(false, nodeMedium, 70f, 20f, m);
        GameObject hiltE = graphScript.addNode(false, nodeMedium, 70f, 25f, m);
        GameObject hiltF = graphScript.addNode(false, nodeMedium, 55f, 25f, m);

        GameObject bladeA = graphScript.addNode(false, nodeMedium, 45f, 90f, m);
        GameObject bladeB = graphScript.addNode(false, nodeMedium, 50f, 100f, m);
        GameObject bladeC = graphScript.addNode(false, nodeMedium, 55f, 90f, m);

        // Edge
        graphScript.addEdge(false, edgeMedium, baseA, baseB, m);
        graphScript.addEdge(false, edgeMedium, baseB, baseC, m);
        graphScript.addEdge(false, edgeMedium, baseC, baseD, m);

        graphScript.addEdge(false, edgeMedium, baseA, hiltA, m);
        graphScript.addEdge(false, edgeMedium, hiltA, hiltB, m);
        graphScript.addEdge(false, edgeMedium, hiltB, hiltC, m);
        graphScript.addEdge(false, edgeMedium, baseD, hiltD, m);
        graphScript.addEdge(false, edgeMedium, hiltD, hiltE, m);
        graphScript.addEdge(false, edgeMedium, hiltE, hiltF, m);

        graphScript.addEdge(false, edgeMedium, hiltC, bladeA, m);
        graphScript.addEdge(false, edgeMedium, bladeA, bladeB, m);
        graphScript.addEdge(false, edgeMedium, hiltF, bladeC, m);
        graphScript.addEdge(false, edgeMedium, bladeC, bladeB, m);
    }

    private void addGraphCreationSmile(int id, Material m)
    {
        // Graph
        Graph graphScript = addGraph(id);

        // Node
        GameObject eyeA = graphScript.addNode(false, nodeMedium, 30f, 70f, m);
        GameObject eyeB = graphScript.addNode(false, nodeMedium, 70f, 70f, m);

        GameObject mouthA = graphScript.addNode(false, nodeMedium, 15f, 45f, m);
        GameObject mouthB = graphScript.addNode(false, nodeMedium, 30f, 35f, m);
        GameObject mouthC = graphScript.addNode(false, nodeMedium, 45f, 30f, m);
        GameObject mouthD = graphScript.addNode(false, nodeMedium, 55f, 30f, m);
        GameObject mouthE = graphScript.addNode(false, nodeMedium, 70f, 35f, m);
        GameObject mouthF = graphScript.addNode(false, nodeMedium, 85f, 45f, m);

        // Edge
        graphScript.addEdge(false, edgeMedium, mouthA, mouthB, m);
        graphScript.addEdge(false, edgeMedium, mouthB, mouthC, m);
        graphScript.addEdge(false, edgeMedium, mouthC, mouthD, m);
        graphScript.addEdge(false, edgeMedium, mouthD, mouthE, m);
        graphScript.addEdge(false, edgeMedium, mouthE, mouthF, m);
    }

    private void addGraphCreationSad(int id, Material m)
    {
        // Graph
        Graph graphScript = addGraph(id);

        // Node
        GameObject eyeA = graphScript.addNode(false, nodeMedium, 30f, 70f, m);
        GameObject eyeB = graphScript.addNode(false, nodeMedium, 70f, 70f, m);

        GameObject mouthA = graphScript.addNode(false, nodeMedium, 15f, 20f, m);
        GameObject mouthB = graphScript.addNode(false, nodeMedium, 30f, 30f, m);
        GameObject mouthC = graphScript.addNode(false, nodeMedium, 45f, 35f, m);
        GameObject mouthD = graphScript.addNode(false, nodeMedium, 55f, 35f, m);
        GameObject mouthE = graphScript.addNode(false, nodeMedium, 70f, 30f, m);
        GameObject mouthF = graphScript.addNode(false, nodeMedium, 85f, 20f, m);

        // Edge
        graphScript.addEdge(false, edgeMedium, mouthA, mouthB, m);
        graphScript.addEdge(false, edgeMedium, mouthB, mouthC, m);
        graphScript.addEdge(false, edgeMedium, mouthC, mouthD, m);
        graphScript.addEdge(false, edgeMedium, mouthD, mouthE, m);
        graphScript.addEdge(false, edgeMedium, mouthE, mouthF, m);
    }

    private void addGraphCreationStar(int id, Material m)
    {
        // Graph
        Graph graphScript = addGraph(id);

        // Node
        GameObject nodeA = graphScript.addNode(false, nodeMedium, 20f, 0f, m);
        GameObject nodeE = graphScript.addNode(false, nodeMedium, 50f, 100f, m);
        GameObject nodeB = graphScript.addNode(false, nodeMedium, 80f, 0f, m);
        GameObject nodeC = graphScript.addNode(false, nodeMedium, 0f, 60f, m);
        GameObject nodeD = graphScript.addNode(false, nodeMedium, 100f, 60f, m);

        // Edge
        graphScript.addEdge(false, edgeMedium, nodeA, nodeE, m);
        graphScript.addEdge(false, edgeMedium, nodeE, nodeB, m);
        graphScript.addEdge(false, edgeMedium, nodeB, nodeC, m);
        graphScript.addEdge(false, edgeMedium, nodeC, nodeD, m);
        graphScript.addEdge(false, edgeMedium, nodeD, nodeA, m);
    }

    private void addGraphFragment6(int id)
    {
        // Graph
        Graph graphScript = addGraph(id);

        // Node
        graphScript.addNode(false, nodeLarge, random(20f, randomScale), random(20f, randomScale), white);
        graphScript.addNode(false, nodeLarge, random(20f, randomScale), random(50f, randomScale), white);
        graphScript.addNode(false, nodeLarge, random(20f, randomScale), random(80f, randomScale), white);
        graphScript.addNode(false, nodeLarge, random(80f, randomScale), random(20f, randomScale), white);
        graphScript.addNode(false, nodeLarge, random(80f, randomScale), random(50f, randomScale), white);
        graphScript.addNode(false, nodeLarge, random(80f, randomScale), random(80f, randomScale), white);
    }

    private void addGraphFragment5(int id)
    {
        // Graph
        Graph graphScript = addGraph(id);

        // Node
        graphScript.addNode(false, nodeLarge, random(20f, randomScale), random(20f, randomScale), white);
        graphScript.addNode(false, nodeLarge, random(20f, randomScale), random(80f, randomScale), white);
        graphScript.addNode(false, nodeLarge, random(50f, randomScale), random(50f, randomScale), white);
        graphScript.addNode(false, nodeLarge, random(80f, randomScale), random(20f, randomScale), white);
        graphScript.addNode(false, nodeLarge, random(80f, randomScale), random(80f, randomScale), white);
    }

    private void addGraphFragment4(int id)
    {
        // Graph
        Graph graphScript = addGraph(id);

        // Node
        graphScript.addNode(false, nodeLarge, random(25f, randomScale), random(25f, randomScale), white);
        graphScript.addNode(false, nodeLarge, random(25f, randomScale), random(75f, randomScale), white);
        graphScript.addNode(false, nodeLarge, random(75f, randomScale), random(25f, randomScale), white);
        graphScript.addNode(false, nodeLarge, random(75f, randomScale), random(75f, randomScale), white);
    }

    private void addGraphFragment3(int id)
    {
        // Graph
        Graph graphScript = addGraph(id);

        // Node
        graphScript.addNode(false, nodeLarge, random(20f, randomScale), random(20f, randomScale), white);
        graphScript.addNode(false, nodeLarge, random(50f, randomScale), random(75f, randomScale), white);
        graphScript.addNode(false, nodeLarge, random(80f, randomScale), random(20f, randomScale), white);
    }

    private void addGraphFragment2(int id)
    {
        // Graph
        Graph graphScript = addGraph(id);

        // Node
        graphScript.addNode(false, nodeLarge, random(30f, randomScale), random(30f, randomScale), white);
        graphScript.addNode(false, nodeLarge, random(70f, randomScale), random(70f, randomScale), white);
    }

    private void addGraphFragment1(int id)
    {
        // Graph
        Graph graphScript = addGraph(id);
        graphScript.setBloom(1f, 90f, 10f, 10f);
        graphScript.setTime(5f, 5f);

        // Node
        graphScript.addNode(true, nodeMassive, 50f, 50f, white);
    }

    private void addGraphCreationZodiacA(int id, Material m)
    {
        // Graph
        Graph graphScript = addGraph(id);

        // Node
        GameObject nodeA = graphScript.addNode(false, nodeMedium, 0f, 65f, m);
        GameObject nodeB = graphScript.addNode(false, nodeMedium, 60f, 50f, m);
        GameObject nodeC = graphScript.addNode(false, nodeMedium, 95f, 40f, m);
        GameObject nodeD = graphScript.addNode(false, nodeMedium, 100f, 25f, m);

        graphScript.addEdge(false, edgeMedium, nodeA, nodeB, m);
        graphScript.addEdge(false, edgeMedium, nodeB, nodeC, m);
        graphScript.addEdge(false, edgeMedium, nodeC, nodeD, m);
    }

    private void addGraphCreationZodiacB(int id, Material m)
    {
        // Graph
        Graph graphScript = addGraph(id);

        // Graph Variable
        GameObject nodeA = graphScript.addNode(false, nodeMedium, -8f, 0f, m);
        GameObject nodeB = graphScript.addNode(false, nodeMedium, -2f, 2f, m);
        GameObject nodeC = graphScript.addNode(false, nodeMedium, 0f, 10f, m);
        GameObject nodeD = graphScript.addNode(false, nodeMedium, 10f, 4f, m);
        GameObject nodeE = graphScript.addNode(false, nodeMedium, 8f, -6f, m);
        GameObject nodeF = graphScript.addNode(false, nodeMedium, 0.5f, -8f, m);
        GameObject nodeG = graphScript.addNode(false, nodeMedium, 1f, -10f, m);

        graphScript.addEdge(false, edgeMedium, nodeA, nodeB, m);
        graphScript.addEdge(false, edgeMedium, nodeB, nodeC, m);
        graphScript.addEdge(false, edgeMedium, nodeC, nodeD, m);
        graphScript.addEdge(false, edgeMedium, nodeD, nodeE, m);
        graphScript.addEdge(false, edgeMedium, nodeE, nodeF, m);
        graphScript.addEdge(false, edgeMedium, nodeF, nodeG, m);

        graphScript.addEdge(false, edgeMedium, nodeC, nodeE, m);
    }

    private void addGraphCreationZodiacC(int id, Material m)
    {
        // Graph
        Graph graphScript = addGraph(id);

        // Graph Variable
        GameObject nodeA = graphScript.addNode(false, nodeMedium, -8f, 10f, m);
        GameObject nodeB = graphScript.addNode(false, nodeSmall, -1f, 2f, m);
        GameObject nodeC = graphScript.addNode(false, nodeMedium, -0.5f, -2f, m);
        GameObject nodeD = graphScript.addNode(false, nodeMedium, -1f, -10f, m);
        GameObject nodeE = graphScript.addNode(false, nodeSmall, 9f, -10f, m);

        graphScript.addEdge(false, edgeMedium, nodeA, nodeB, m);
        graphScript.addEdge(false, edgeMedium, nodeB, nodeC, m);
        graphScript.addEdge(false, edgeMedium, nodeC, nodeD, m);

        graphScript.addEdge(false, edgeMedium, nodeC, nodeE, m);
    }

    private void initializeCreation() {
        creation = new List<addGraphCreation>();
        creation.Add(addGraphCreationMoon);
        creation.Add(addGraphCreationDiamond);
        creation.Add(addGraphCreationFan);
        creation.Add(addGraphCreationWheel);
        creation.Add(addGraphCreationSword);
        creation.Add(addGraphCreationSmile);
        creation.Add(addGraphCreationSad);
        creation.Add(addGraphCreationStar);
        creation.Add(addGraphCreationZodiacA);
    }

    private void addGraphCreationRandom(int id) {
        // Variable
        int random = Random.Range(0, creation.Count);
        Material m = materialRandom();

        // Graph
        creation[random](id, m);
    }

    private Graph addGraph(int id) {
        // Graph
        GameObject graph = new GameObject("Graph " + id);
        graphs.Add(graph);

        // Graph Script
        graph.AddComponent<Graph>();
        Graph graphScript = graph.GetComponent<Graph>();
        graphScript.initialize(id);
        return graphScript;
    }

    private void addGraphOsc(OscMessage graph) {
        // Variable
        int id = graph.GetInt(0);

        // Graph
        switch (id) {
            case 11:
                addGraphExplorationNode(11);
                break;
            case 12:
                addGraphExplorationEdge(12);
                break;
            case 21:
                addGraphDesignHorizontal(21);
                break;
            case 22:
                addGraphDesignVertical(22);
                break;
            case 23:
                addGraphDesignHorizontalCross(23);
                break;
            case 24:
                addGraphDesignVerticalCross(24);
                break;
            case 25:
                addGraphDesignBox(25);
                break;
            case 26:
                addGraphDesignGrid(26);
                break;
            case 31:
                addGraphCreationRandom(31);
                break;
            case 41:
                addGraphFragment1(41);
                break;
        }
    }

    private void action(OscMessage graph) {
        // Variable
        int id = graph.GetInt(1);
        int count = graph.GetInt(2);
        GameObject g = lookup(id);

        // Action
        if (g != null) {
            Graph gScript = g.GetComponent<Graph>();
            if (gScript.getState() == State.Incomplete) {
                osc.Send(graph);
                for (int i = 0; i < count; i++) {
                    if (gScript.getState() == State.Incomplete) {
                        gScript.action();
                    }
                    else {
                        break;
                    }
                }
            }
            else if (gScript.getState() == State.Complete && count > 0) {
                osc.Send(graph); // TODO: TEST
                delete(g);
                // AddGraphCreationRandom Automate
                if (id == 31) {
                    addGraphCreationRandom(31);
                }
            }
            else { // State.Delete
                //
            }
        }
    }
}
