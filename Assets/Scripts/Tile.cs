using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public class IconInfo
{
    public Sprite sprite;
    public int index;
}

public class Tile : MonoBehaviour
{
    [SerializeField] public SpriteRenderer iconRenderer;
    //[SerializeField] private SpriteRenderer tileRenderer;

    private int originalSortingOrder;
    private string originalSortingLayerName;
    public int initialSortingOrder;

    public IconInfo[] icons;
    public IconInfo selectedIcon;
    private int selectedIconIndex;

    public Vector3 originalScale;
    public Vector3 originalPosition;

    public int x;
    public int y;

    public bool isSelected = false;
    public int index;


    //Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        originalScale = transform.localScale;
        initialSortingOrder = GetComponent<Renderer>().sortingOrder;
        selectedIcon = icons[Random.Range(0, icons.Length)];
        gameObject.GetComponentsInChildren<SpriteRenderer>()[1].sprite = selectedIcon.sprite;
        selectedIconIndex = Random.Range(0, icons.Length);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void SetIcon(IconInfo icon, int index)
    {
        selectedIcon = icon;
        selectedIconIndex = index;
        gameObject.GetComponentsInChildren<SpriteRenderer>()[1].sprite = selectedIcon.sprite;
    }
    
    public void IncreaseScale(float inscreaseValue, float increaseDuration)
    {
        //transform.DOScale(transform.localScale + new Vector3(inscreaseValue, inscreaseValue, inscreaseValue), increaseDuration);
        if (transform == null)
        {
            return;
        }
        transform.DOScale(transform.localScale + Vector3.one * 0.05f * inscreaseValue, increaseDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                if (transform != null)
                {
                    transform.DOScale(Vector3.one*0.42f, increaseDuration);
                }
            });
    }

    public GameObject GetAboveTile()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, 1f))
        {
            Tile tile = hit.collider.gameObject.GetComponent<Tile>();
            if (tile != null)
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }

    public void DecreaseScale(float scaleValue, float duration)
    {
        // Calculate the target scale
        Vector3 targetScale = originalScale / scaleValue;

        // Start decreasing the scale gradually
        StartCoroutine(GradualScale(targetScale, duration));
    }

    IEnumerator GradualScale(Vector3 targetScale, float duration)
    {
        // Calculate the step size for each frame
        float step = (originalScale.magnitude / targetScale.magnitude - 1f) / duration;

        // Decrease the scale gradually
        while (transform.localScale.magnitude > targetScale.magnitude)
        {
            transform.localScale -= Vector3.one * step * Time.deltaTime;

            // Make sure the scale doesn't go below the target scale
            if (transform.localScale.magnitude < targetScale.magnitude)
            {
                transform.localScale = targetScale;
            }

            yield return null;
        }
    }

}






