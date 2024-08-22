using System.Collections;
using UnityEngine;

public class Sky : MonoBehaviour
{
    // OSC
    public OSC osc;
    public string oscSky;

    // Variable
    public Material[] sky;
    private float rotation;
    private float rotationStart;
    private float rotationSpeed;
    // Exposure
    private bool safety;
    private float exposureTimein;
    private float exposureTimeout;
    private float exposureLow;
    private float exposureHigh;

    void Start()
    {
        // OSC
        if (osc) {
            osc.SetAddressHandler(oscSky, setSky);
        }

        // Rotation
        rotation = 0f;
        rotationStart = 120f;
        rotationSpeed = 0.01f;

        // Exposure
        exposureTimein = 3f;
        exposureTimeout = 3f;
        exposureLow = 0f;
        exposureHigh = 0.85f;
    }

    void Update()
    {
        // Rotation
        rotation += rotationSpeed;
        RenderSettings.skybox.SetFloat("_Rotation", rotation);
    }

    private void setSky(OscMessage skyIndex) {
        int index = skyIndex.GetInt(0);
        if (!safety && index >= 0 && index < sky.Length) {
            StartCoroutine(setExposureCoroutine(sky[index]));
        }
    }

    private IEnumerator setExposureCoroutine(Material sky) {
        // Safety
        safety = true;

        // Exposure Decrease
        yield return StartCoroutine(setExposureCoroutineB(exposureLow, exposureTimein));

        // Set Skybox
        RenderSettings.skybox = sky;
        RenderSettings.skybox.SetFloat("_Exposure", exposureLow);
        RenderSettings.skybox.SetFloat("_Rotation", rotationStart);

        // Exposure Increase
        yield return StartCoroutine(setExposureCoroutineB(exposureHigh, exposureTimeout));

        // Safety
        safety = false;
    }

    private IEnumerator setExposureCoroutineB(float exposureTarget, float duration) {
        // Variable
        float time = 0f;
        float lerp = 0f;
        float exposure = 0f;
        float exposureStart = RenderSettings.skybox.GetFloat("_Exposure");

        // Exposure
        while (time < duration) {
            time += Time.deltaTime;
            lerp = time / duration;
            exposure = Mathf.Lerp(exposureStart, exposureTarget, lerp);
            RenderSettings.skybox.SetFloat("_Exposure", exposure);
            yield return null;
        }
        RenderSettings.skybox.SetFloat("_Exposure", exposureTarget);
    }
}
