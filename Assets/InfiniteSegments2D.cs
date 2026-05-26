using UnityEngine;
using System.Collections.Generic;

public class InfiniteSegments2D : MonoBehaviour
{
    [Header("Follow target (player_0)")]
    public Transform followTarget;

    [Header("Segments (A, B, C, D...)")]
    public Transform[] segments;

    [Header("Background root name inside each segment")]
    public string backgroundRootName = "Background";

    [Header("Recycle settings (WORLD units)")]
    public float recycleDistance = 3f;
    public float gap = 0.01f;

    [Header("Debug")]
    public bool logBoundsOnStart = true;

    void Start()
    {
        if (followTarget == null)
        {
            Debug.LogError("InfiniteSegments2D: followTarget is NULL. Assign player_0.");
            enabled = false;
            return;
        }

        if (segments == null || segments.Length < 2)
        {
            Debug.LogError("InfiniteSegments2D: Need at least 2 segments.");
            enabled = false;
            return;
        }

        // ✅ Перевірка на дублікати
        var set = new HashSet<Transform>();
        for (int i = 0; i < segments.Length; i++)
        {
            if (segments[i] == null)
            {
                Debug.LogError($"InfiniteSegments2D: segments[{i}] is NULL");
                enabled = false;
                return;
            }

            if (!set.Add(segments[i]))
            {
                Debug.LogError($"InfiniteSegments2D: DUPLICATE segment in array: {segments[i].name}. " +
                               "Ти підключив один і той самий Segment кілька разів.");
                enabled = false;
                return;
            }
        }

        // ✅ Перевірка що у кожного сегмента є bounds (колайдер/спрайт під Background)
        for (int i = 0; i < segments.Length; i++)
        {
            if (!TryGetSegmentBounds(segments[i], out var b))
            {
                Debug.LogError($"InfiniteSegments2D: Can't read bounds for {segments[i].name}. " +
                               $"Перевір що є '{backgroundRootName}' і під ним є BoxCollider2D або SpriteRenderer.");
                enabled = false;
                return;
            }
        }

        // ✅ Вишикуємо сегменти без дірок
        for (int i = 1; i < segments.Length; i++)
        {
            TryGetSegmentBounds(segments[i - 1], out var prevB);
            TryGetSegmentBounds(segments[i], out var curB);

            float targetMinX = prevB.max.x + gap;
            MoveSegmentToMinX(segments[i], targetMinX);
        }

        if (logBoundsOnStart)
        {
            Debug.Log("=== Segments bounds after arrange ===");
            for (int i = 0; i < segments.Length; i++)
            {
                TryGetSegmentBounds(segments[i], out var b);
                Debug.Log($"{i}: {segments[i].name}  minX={b.min.x:F2}  maxX={b.max.x:F2}");
            }
        }
    }

void Update()
{
    if (segments == null || segments.Length < 2) return;

    // leftmost & rightmost by bounds
    int leftIndex = 0;
    int rightIndex = 0;

    TryGetSegmentBounds(segments[0], out var leftB);
    var rightB = leftB;

    for (int i = 1; i < segments.Length; i++)
    {
        TryGetSegmentBounds(segments[i], out var b);

        if (b.min.x < leftB.min.x)
        {
            leftB = b;
            leftIndex = i;
        }

        if (b.max.x > rightB.max.x)
        {
            rightB = b;
            rightIndex = i;
        }
    }

    // recycle when player passed left segment
    if (followTarget.position.x > leftB.max.x + recycleDistance)
    {
        float targetMinX = rightB.max.x + gap;

        // ✅ ДОДАЙ ЦЕЙ LOG
        Debug.Log(
            $"RECYCLE: moving {segments[leftIndex].name} → after {segments[rightIndex].name} | " +
            $"playerX={followTarget.position.x:F2} | leftMax={leftB.max.x:F2} | rightMax={rightB.max.x:F2}"
        );

        MoveSegmentToMinX(segments[leftIndex], targetMinX);
    }
}


    bool TryGetSegmentBounds(Transform segment, out Bounds bounds)
    {
        bounds = new Bounds(segment.position, Vector3.one);

        Transform bgRoot = segment.Find(backgroundRootName);
        if (bgRoot == null)
        {
            Debug.LogError($"InfiniteSegments2D: '{backgroundRootName}' not found in {segment.name}");
            return false;
        }

        // Prefer collider bounds (у тебе колізія вже готова)
        BoxCollider2D bc = bgRoot.GetComponentInChildren<BoxCollider2D>();
        if (bc != null && bc.enabled)
        {
            bounds = bc.bounds;
            return true;
        }

        // Fallback to sprite bounds
        SpriteRenderer sr = bgRoot.GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            bounds = sr.bounds;
            return true;
        }

        return false;
    }

    void MoveSegmentToMinX(Transform segment, float targetMinX)
    {
        TryGetSegmentBounds(segment, out var b);
        float dx = targetMinX - b.min.x;
        segment.position += new Vector3(dx, 0f, 0f);
    }
}
