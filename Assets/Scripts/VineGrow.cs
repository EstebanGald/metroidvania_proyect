using UnityEngine;
using System.Collections;

public class VineGrow : MonoBehaviour
{
    [Header("Vine Settings")]
    [Tooltip("Total number of sprite segments to stack (height = segments x sprite height)")]
    public int maxSegments = 5;

    [Tooltip("How many new segments appear per second (e.g., 1 = one per second)")]
    public float growSpeed = 1f;

    private SpriteRenderer parentSprite;
    private BoxCollider2D boxCollider;
    private float segmentHeight;
    //private bool isGrowing = false;

    private void Awake()
    {
        parentSprite = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        segmentHeight = parentSprite.bounds.size.y;
        //StartCoroutine(GrowRoutine());
    }

    public void StartGrowing()
    { 
        StartCoroutine(GrowRoutine()); 
    }

    private IEnumerator GrowRoutine()
    {
        float segmentGrowDuration = 1f / growSpeed;

        for (int i = 1; i < maxSegments; i++)
        {
            GameObject segment = new GameObject("VineSegment");
            segment.transform.SetParent(transform);

            SpriteRenderer sr = segment.AddComponent<SpriteRenderer>();
            sr.sprite = parentSprite.sprite;
            sr.sortingLayerID = parentSprite.sortingLayerID;
            sr.sortingOrder = parentSprite.sortingOrder;

            // start at TOP of previous segment, zero height
            float startY = i * segmentHeight - segmentHeight * 0.5f;
            segment.transform.localPosition = new Vector3(0, startY, 0);
            segment.transform.localScale = new Vector3(1, 0, 1);

            float targetY = i * segmentHeight;
            float elapsed = 0f;

            while (elapsed < segmentGrowDuration)
            {
                float t = elapsed / segmentGrowDuration;
                segment.transform.localPosition = new Vector3(0,
                    Mathf.Lerp(startY, targetY, t), 0);
                segment.transform.localScale = new Vector3(1,
                    Mathf.Lerp(0, 1, t), 1);

                float totalH = (i + t) * segmentHeight;
                boxCollider.size = new Vector2(boxCollider.size.x, totalH);
                boxCollider.offset = new Vector2(boxCollider.offset.x,
                    (i + t - 1) * segmentHeight * 0.5f);

                elapsed += Time.deltaTime;
                yield return null;
            }

            segment.transform.localPosition = new Vector3(0, targetY, 0);
            segment.transform.localScale = Vector3.one;

            int count = i + 1;
            boxCollider.size = new Vector2(boxCollider.size.x, count * segmentHeight);
            boxCollider.offset = new Vector2(boxCollider.offset.x,
                (count - 1) * segmentHeight * 0.5f);
        }
    }
}