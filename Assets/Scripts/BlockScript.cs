using UnityEngine;
using UnityEngine.UI;

public class BlockScript : MonoBehaviour
{
    [SerializeField] private Image blockImage;

    void Awake()
    {
        originalColor = blockImage.color;
    }

    private Color originalColor;
    public void EnabledBlock()
    {
        blockImage.color = Color.green;
    }
    public void DisableBlock()
    {
        blockImage.color = originalColor;
    }
}
