using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Minimal runtime safety: creates an EventSystem if missing and performs a conservative fix for full-screen overlay UI that blocks raycasts.
/// Put this file anywhere outside of an Editor folder (for example Assets/Scripts).
/// </summary>
public static class EnsureUIRuntime
{
    private static readonly string[] OverlayNameKeywords = new[] { "scrim", "overlay", "background", "panel", "shadow" };

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void EnsureEventSystem()
    {
        if (EventSystem.current == null)
        {
            var go = new GameObject("EventSystem_Runtime");
            go.AddComponent<EventSystem>();
            go.AddComponent<StandaloneInputModule>();
            GameObject.DontDestroyOnLoad(go);
            Debug.Log("[EnsureUIRuntime] Created EventSystem_Runtime because none existed.");
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void AfterSceneLoadFixes()
    {
        // Start a helper GameObject to run fixes after a frame so scene initialization completes.
        var helper = new GameObject("EnsureUIRuntime_Helper");
        helper.AddComponent<HelperBehaviour>();
        Object.DontDestroyOnLoad(helper);
    }

    private class HelperBehaviour : MonoBehaviour
    {
        private void Start()
        {
            FixImages();
            FixRawImages();
            FixTMPTexts();
            Destroy(gameObject);
        }

        private void FixImages()
        {
            var images = FindObjectsOfType<Image>(true);
            foreach (var img in images)
            {
                if (!img.raycastTarget) continue;
                if (!IsLikelyFullScreen(img.rectTransform)) continue;

                bool nameMatches = NameContainsKeyword(img.gameObject.name);
                bool semiTransparent = img.color.a < 0.99f;

                if (nameMatches || semiTransparent)
                {
                    img.raycastTarget = false;
                    Debug.Log($"[EnsureUIRuntime] Disabled Image.raycastTarget on '{img.gameObject.name}' to avoid blocking UI.");
                }
            }
        }

        private void FixRawImages()
        {
            var rawImages = FindObjectsOfType<RawImage>(true);
            foreach (var r in rawImages)
            {
                if (!r.raycastTarget) continue;
                if (!IsLikelyFullScreen(r.rectTransform)) continue;

                bool nameMatches = NameContainsKeyword(r.gameObject.name);
                bool semiTransparent = r.color.a < 0.99f;

                if (nameMatches || semiTransparent)
                {
                    r.raycastTarget = false;
                    Debug.Log($"[EnsureUIRuntime] Disabled RawImage.raycastTarget on '{r.gameObject.name}' to avoid blocking UI.");
                }
            }
        }

        private void FixTMPTexts()
        {
            var tmpTexts = FindObjectsOfType<TMP_Text>(true);
            foreach (var t in tmpTexts)
            {
                var prop = t.GetType().GetProperty("raycastTarget");
                if (prop == null) continue;
                bool rt = (bool)prop.GetValue(t);
                if (!rt) continue;
                if (!IsLikelyFullScreen(t.rectTransform)) continue;

                bool nameMatches = NameContainsKeyword(t.gameObject.name);
                bool semiTransparent = t.color.a < 0.99f;

                if (nameMatches || semiTransparent)
                {
                    prop.SetValue(t, false);
                    Debug.Log($"[EnsureUIRuntime] Disabled TMP_Text.raycastTarget on '{t.gameObject.name}' to avoid blocking UI.");
                }
            }
        }

        private static bool NameContainsKeyword(string name)
        {
            var lower = name.ToLowerInvariant();
            foreach (var kw in OverlayNameKeywords)
                if (lower.Contains(kw)) return true;
            return false;
        }

        private static bool IsLikelyFullScreen(RectTransform rt)
        {
            if (rt == null) return false;
            var aMin = rt.anchorMin;
            var aMax = rt.anchorMax;

            bool anchorsFull = Mathf.Approximately(aMin.x, 0f) && Mathf.Approximately(aMin.y, 0f)
                            && Mathf.Approximately(aMax.x, 1f) && Mathf.Approximately(aMax.y, 1f);

            bool sizeZero = Mathf.Approximately(rt.sizeDelta.x, 0f) && Mathf.Approximately(rt.sizeDelta.y, 0f);
            bool posZero = Mathf.Approximately(rt.anchoredPosition.x, 0f) && Mathf.Approximately(rt.anchoredPosition.y, 0f);

            return anchorsFull && (sizeZero || posZero);
        }
    }
}