using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class ReplaceTMPFont : MonoBehaviour
{
    public TMP_FontAsset oldFont;
    public TMP_FontAsset newFont;

    [Button]
    private void ReplaceFonts()
    {
        TMP_Text[] texts = FindObjectsOfType<TMP_Text>(true);

        int count = 0;

        foreach (var text in texts)
        {
            if (text.font == oldFont)
            {
                text.font = newFont;
                count++;
     }
        }

        Debug.Log($"Replaced {count} texts");
    }
}