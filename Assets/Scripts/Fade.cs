using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
     // OSC
    public OSC osc;
    public string oscFade;

    // Coroutine
    private bool safety;

    // Exposure
    private float fadeinTime;
    private float fadeoutTime;

    void Start()
    {
        // OSC
        if (osc) {
            osc.SetAddressHandler(oscFade, setFade);
        }

        // Fade
        fadeinTime = 6f;
        fadeoutTime = 6f;
    }

    private void setFade(OscMessage fade) {
        int type = fade.GetInt(0);
        if (!safety) {
            if (type == 0) {
                StartCoroutine(setFadeCoroutine(0.85f, fadeinTime));
            } else {
                StartCoroutine(setFadeCoroutine(0f, fadeoutTime));
            }
        }
    }

    private IEnumerator setFadeCoroutine(float exposureTarget, float duration) {
        // Safety
        safety = true;

        // Variable
        float time = 0f;
        float lerp = 0f;
        float exposure = 0f;
        float exposureStart = RenderSettings.skybox.GetFloat("_Exposure");

        // Increase Bloom
        while (time < duration) {
            time += Time.deltaTime;
            lerp = time / duration;
            exposure = Mathf.Lerp(exposureStart, exposureTarget, lerp);
            RenderSettings.skybox.SetFloat("_Exposure", exposure);
            yield return null;
        }

        // Safety
        safety = false;
    }

    // private IEnumerator setBloomCoroutine(float bloomTarget, float duration) {
    //     // Safety
    //     safety = true;

    //     // Variable
    //     float time = 0f;
    //     float lerp = 0f;
    //     float bloomStart = bloom.intensity.value;

    //     // Increase Bloom
    //     while (time < duration) {
    //         time += Time.deltaTime;
    //         lerp = time / duration;
    //         bloom.intensity.value = Mathf.Lerp(bloomStart, bloomTarget, lerp);
    //         yield return null;
    //     }

    //     // Safety
    //     safety = false;
    // }

    // private IEnumerator setExposureCoroutine(float exposureTarget, float duration) {
    //     // Variable
    //     float time = 0f;
    //     float lerp = 0f;
    //     float exposure = 0f;
    //     float exposureStart = RenderSettings.skybox.GetFloat("_Exposure");

    //     // Exposure
    //     while (time < duration) {
    //         time += Time.deltaTime;
    //         lerp = time / duration;
    //         exposure = Mathf.Lerp(exposureStart, exposureTarget, lerp);
    //         RenderSettings.skybox.SetFloat("_Exposure", exposure);
    //         yield return null;
    //     }
    //     RenderSettings.skybox.SetFloat("_Exposure", exposureTarget);
    // }
}
