using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;

public class IntroSequence : MonoBehaviour 
{
	void Start ()
	{
	    var isIntro = State.Instance.IsIntro;
	    var player = FindObjectOfType<Player>();
	    var cameraMove = FindObjectOfType<CameraMove>();
        var introCameraPosition = GameObject.Find("IntroCameraPostion");

	    if (isIntro)
	    {
            // Enable Intro UI (should start enabled?)

            // Disable player movement
            // Move player to start spot (should already be working)
	        player.DisableControl();

            // TODO Tell player to sit maybe


            // Move the camera into the sky
	        cameraMove.Pause = true;
            cameraMove.SetCenterPosition(introCameraPosition.transform.position);

            var logos = GuiCanvas.Instance.GetTitleCards();


	        // ~~Start async here~~
            StartCoroutine(PlayIntroSequence(logos, cameraMove, player));

	        
	     
	        
	        // Enable player movement
	    }
	}

    IEnumerator PlayIntroSequence(List<GameObject> cards, CameraMove camera, Player player)
    {
        // Fade logos
        foreach (var card in cards)
        {
            Debug.Log(card.name);
            var image = card.GetComponent<Image>();

            if (image != null)
            {
                image.color = Color.white.SetAlpha(0f);
            }
            else
            {
                var childImage = card.GetComponentInChildren<Image>();
                if (childImage != null)
                {
                    childImage.color = Color.white.SetAlpha(0f);
                }
            }
        }

        

        // Fade logos
        foreach (var card in cards)
        {

            yield return new WaitForSeconds(2f);

            var image = card.GetComponent<Image>();
            
            if (image != null)
            {
                yield return StartCoroutine(FadeLogoIn(image));
                yield return new WaitForSeconds(2f);

                if (cards.Last() != card)
                {
                    yield return StartCoroutine(FadeLogo(image));
                }
            }
            else
            {
                var childImage = card.GetComponentInChildren<Image>();
                if (childImage != null)
                {
                    yield return StartCoroutine(FadeLogoIn(childImage));
                    yield return new WaitForSeconds(2f);

                    if (cards.Last() != card)
                    {
                        yield return StartCoroutine(FadeLogo(childImage));
                    }
                }
            }
        }

        // Pan down to player character
        camera.cameraCenter.transform.DOMove(player.transform.position, 1f);
        yield return new WaitForSeconds(1f);

        // TODO Show controls graphic

        player.EnableControl();
    }


    IEnumerator FadeLogoIn(Image image)
    {
        var totalTime = .5f;
        DOTween.ToAlpha(() => image.color, c => image.color = c, 1f, totalTime);
    
        yield return new WaitForSeconds(totalTime);

        yield return null;
    }

    IEnumerator FadeLogo(Image image)
    {
        var totalTime = .5f;
        DOTween.ToAlpha(() => image.color, c => image.color = c, 0f, totalTime);
    
        yield return new WaitForSeconds(totalTime);

        yield return null;
    }
}
