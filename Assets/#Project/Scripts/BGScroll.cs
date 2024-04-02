using UnityEngine;
using UnityEngine.UI;

public class BGScroll : MonoBehaviour
{
    public float scrollSpeed;
    public ScrollRect _scroll;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        _scroll.onValueChanged.AddListener(OnScrollChange);
    }

    private void OnScrollChange(Vector2 vector)
    {
        bool isScrollable = (_scroll.horizontalNormalizedPosition != 1f && _scroll.horizontalNormalizedPosition != 0f);
        if (isScrollable)
        {
            Vector2 offset = new Vector2(vector.x * scrollSpeed, 0);
            meshRenderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
        }
    }
}
