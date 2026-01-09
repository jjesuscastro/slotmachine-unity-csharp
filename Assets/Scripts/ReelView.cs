using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ReelView : MonoBehaviour
{
    public UnityEvent onReelStop;

    [SerializeField]
    Transform symbolsParent;
    [SerializeField]
    ScrollRect scrollRect;

    bool resetScroll = false;
    bool stopOnTarget = false;
    int target;
    float scrollVelocity = 0;

    private void Update()
    {
        if (resetScroll)
        {
            scrollRect.verticalNormalizedPosition = 0;
            scrollRect.velocity = new Vector2(0, scrollVelocity);
            resetScroll = false;
        }

        if (scrollRect.verticalNormalizedPosition >= 0.999)
        {
            resetScroll = true;
            scrollVelocity = scrollRect.velocity.y;
        }

        if (stopOnTarget && scrollRect.velocity.y > -1000)
        {
            StopSpin();
        }
    }

    public void Spin(float velocity, int target)
    {
        scrollRect.velocity = new Vector2(0, -velocity);

        stopOnTarget = true;
        this.target = target;
    }

    public void StopSpin()
    {
        scrollRect.content.localPosition = scrollRect.GetSnapToPositionToBringChildIntoView((RectTransform)symbolsParent.GetChild(target));
        scrollRect.velocity = Vector2.zero;
        stopOnTarget = false;

        if (onReelStop != null)
            onReelStop.Invoke();
    }

    public void SetSymbols(int[] symbolArray)
    {
        DestroyAllChildren();
        symbolsParent.DetachChildren();

        SymbolsCatalog symbolsInstance = SymbolsCatalog.i;
        List<Symbol> symbols = symbolsInstance.GetSymbols();
        GameObject symbolPrefab = symbolsInstance.symbolPrefab;

        foreach (int s in symbolArray)
        {
            int index = Mathf.Clamp(s, 0, symbols.Count - 1);

            Image image = Instantiate(symbolPrefab, Vector3.zero, Quaternion.identity, symbolsParent).GetComponent<Image>();
            image.sprite = symbols[index].sprite;
        }

        //Duplicate first 3 for looping
        for (int i = 0; i < 3; i++)
        {
            Instantiate(symbolsParent.GetChild(i), Vector3.zero, Quaternion.identity, symbolsParent);
        }

        resetScroll = true;
    }

    void DestroyAllChildren()
    {
        foreach (Transform child in symbolsParent)
        {
            Destroy(child.gameObject);
        }
    }
}
