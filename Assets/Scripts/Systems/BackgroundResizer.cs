using UnityEngine;

public class BackgroundResizer : MonoBehaviour
{
    void Start()
    {
        // Calculated in world units to stay independent from resolution and aspect ratio
        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        // Applied to children to allow layered backgrounds under a single root
        foreach (Transform child in transform)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            if (sr == null)
                continue;

            float width = sr.sprite.bounds.size.x;
            float height = sr.sprite.bounds.size.y;

            // Scaled explicitly to fill screen without relying on camera stretch
            Vector3 newScale = new Vector3(
                worldScreenWidth / width,
                worldScreenHeight / height,
                1f
            );

            child.localScale = newScale;
        }
    }
}

