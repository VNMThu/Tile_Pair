using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Sprite[] icons;
    private Sprite selectedIcon;


    //Start is called before the first frame update
    void Start()
    {
       
        selectedIcon = icons[Random.Range(0, icons.Length)];
        gameObject.GetComponentsInChildren<SpriteRenderer>()[1].sprite = selectedIcon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void MoveTo(Vector2Int vector2Int, float moveDuration)
    {
        StartCoroutine(MoveToCoroutine(vector2Int, moveDuration));
    }

    private IEnumerator MoveToCoroutine(Vector2Int vector2Int, float moveDuration)
    {
        Vector2 startPos = transform.position;
        Vector2 endPos = new Vector2(vector2Int.x, vector2Int.y);
        float time = 0f;

        while (time < moveDuration)
        {
            time += Time.deltaTime;
            transform.position = Vector2.Lerp(startPos, endPos, time / moveDuration);
            yield return null;
        }

        transform.position = endPos;
    }
}






