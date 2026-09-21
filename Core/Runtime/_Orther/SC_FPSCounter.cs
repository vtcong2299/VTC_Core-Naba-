using UnityEngine;

public class SC_FPSCounter : MonoBehaviour
{
    public enum FPSPosition
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    [Header("Display")]
    public FPSPosition displayPosition = FPSPosition.BottomLeft;
    public Vector2 offset = new Vector2(20, 20);

    public float updateInterval = 0.5f;

    private float accum = 0.0f;
    private int frames = 0;
    private float timeleft;
    private float fps;

    private GUIStyle textStyle = new GUIStyle();

    private void Start()
    {
        timeleft = updateInterval;

        textStyle.fontStyle = FontStyle.Bold;
        textStyle.normal.textColor = Color.green;
        textStyle.fontSize = 25;

#if RELEASE
        Destroy(gameObject);
#endif
    }

    private void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        if (timeleft <= 0.0f)
        {
            fps = accum / frames;

            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }

    private void OnGUI()
    {
        Rect rect = GetRect();

        GUI.Label(rect, $"{fps:F2} FPS", textStyle);
    }

    private Rect GetRect()
    {
        const float width = 150;
        const float height = 40;

        switch (displayPosition)
        {
            case FPSPosition.TopLeft:
                return new Rect(offset.x, offset.y, width, height);

            case FPSPosition.TopRight:
                return new Rect(Screen.width - width - offset.x, offset.y, width, height);

            case FPSPosition.BottomLeft:
                return new Rect(offset.x, Screen.height - height - offset.y, width, height);

            case FPSPosition.BottomRight:
                return new Rect(Screen.width - width - offset.x,
                    Screen.height - height - offset.y,
                    width, height);

            default:
                return new Rect(offset.x, offset.y, width, height);
        }
    }
}