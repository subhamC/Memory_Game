using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{

    [HideInInspector]public CardType cType;
    [HideInInspector] public CardScriptableObject card;
    [HideInInspector] public int CardID = -1;
    private bool isFlipped = false , isturning = false;
    public Image artwork = null;
    private Sprite assignedArtwork = null, assignedBackFace = null;
    public delegate void CardSelection();

    private void OnEnable()
    {
        Initialize();
    }

    public void Initialize()
    {
        GetComponent<Button>().onClick.AddListener(() => CardBtn());
        isFlipped = true;
        isturning = false;
    }

    public void InitializeCardProperties(Sprite Default)
    {
        if(card != null)
        {
            cType = card.type;
            artwork.sprite = card.artwork;
            assignedArtwork = card.artwork;
        }
        else
        {
            artwork.sprite = Default;
            assignedBackFace = Default;
        }
      
    }

    public void SetCardScriptableObject(CardScriptableObject card)
    {
        this.card = card;
    }

    public void Flip(CardSelection selection = null, float startDelay = 0)
    {
        isturning = true;
        StartCoroutine(AnimateCard( 0.25f, true, startDelay, selection));
    }

    public void DelayedFlipBack()
    {
        Flip(startDelay: .5f);
    }

    private IEnumerator AnimateCard( float time, bool changeSprite, float startDelay, CardSelection selection)
    {
        if(startDelay > 0)
        {
            yield return new WaitForSeconds(startDelay);
        }
    
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = transform.rotation * Quaternion.Euler(new Vector3(0, 90, 0));
        float rate = 1.0f / time;
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * rate;
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }
        if(selection != null)
        {
            selection();
        }
        //change sprite and flip another 90degree
         
        if (changeSprite)
        {
            isFlipped = !isFlipped;
            ChangeSprite();
            if(this.gameObject.activeInHierarchy == true)
            {

                StartCoroutine(AnimateCard(time, false,0, null));
            }
        }
        else
            isturning = false;
        ResetRotation();


    }

    public void ResetRotation()
    {
        if (!isFlipped)
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        
    }

    private void ChangeSprite()
    {
        if (isFlipped)
            artwork.sprite = assignedArtwork;
        else
        {
            artwork.sprite = assignedBackFace;
           
        }
    }

    public void DisableCard()
    {
        Invoke("Disable",.5f);
        
    }

    public void Disable()
    {
        StopAllCoroutines();
        this.gameObject.SetActive(false);
    }

    public void CardBtn()
    {
        if (isFlipped || isturning) return;
       CardSelection selection = SelectionEvent;
        Flip(selection);
    }


    private void SelectionEvent()
    {
        Events.instance.Raise(new CardSelectionGameEvent(cType, CardID));
     
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}