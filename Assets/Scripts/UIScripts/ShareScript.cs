using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareScript : MonoBehaviour
{
    private Texture2D _screenshot;

    void Start()
    {
        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        yield return new WaitForSeconds(2f);
        _screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        _screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        _screenshot.Apply();

        StartCoroutine(RandomScreenshots(25f));
    }

    public void Share()
    {
        StartCoroutine(TakeScreenshotAndShare());
    }

    private IEnumerator RandomScreenshots(float repeatAfterSeconds)
    {
        while (gameObject.activeSelf)
        {
            if (Time.timeScale > 0)
            {
                _screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
                _screenshot.Apply();
                yield return new WaitForSeconds(repeatAfterSeconds);
            }
        }
    }

    private IEnumerator TakeScreenshotAndShare()
    {
        yield return new WaitForEndOfFrame();

        string filePath = System.IO.Path.Combine(Application.temporaryCachePath, "shared img.png");
        System.IO.File.WriteAllBytes(filePath, _screenshot.EncodeToPNG());

        Destroy(_screenshot);

        new NativeShare().AddFile(filePath)
            .SetSubject("Insearcher is a cool game!").SetText("Look at this picture from Insearcher game!" )
            .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
            .Share();

    }
}
