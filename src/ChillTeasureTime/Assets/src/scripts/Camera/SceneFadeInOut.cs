using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneFadeInOut : Singleton<SceneFadeInOut>
{
    public float fadeSpeed = 8f; 

    private Image _image;

    void Start()
    {
        _image = GetComponent<Image>();
    }

    public void OnLevelWasLoaded(int level)
    {
        StartScene();
    }

    IEnumerator FadeToClear()
    {
        yield return new WaitForSeconds(.5f);

        while (_image.color.a >= 0.05f)
        {
            // Lerp the colour of the texture between itself and transparent.
            _image.color = Color.Lerp(_image.color, Color.clear, fadeSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        _image.color = Color.clear;
    }

    IEnumerator FadeToBlack()
    {
        while (_image.color.a <= 0.95f)
        {
            // Lerp the colour of the texture between itself and black.
            _image.color = Color.Lerp(_image.color, Color.black, fadeSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        _image.color = Color.black;
    }

    public void StartScene()
    {
        // Fade the texture to clear.
        StartCoroutine(FadeToClear());
    }

    public void EndScene()
    {
        StartCoroutine(FadeToBlack());
    }
}