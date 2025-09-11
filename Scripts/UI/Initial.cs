using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Initial : MonoBehaviour
{
    private Canvas canvas;
    private Font uiFont;
    private float buttonYOffset = 50; // 按钮起始偏移（配合标题）
    private const float ButtonSpacing = 200; // 按钮间距
    private const float ButtonWidth = 550;   // 按钮宽度
    private const float ButtonHeight = 140;  // 按钮高度

    // 用于圆角的九宫格图片（需要在Resources文件夹中准备）
    private Sprite roundedCornerSprite;

    // ===================== 新配色方案：莫兰迪青绿色系 =====================
    private readonly Color BgColor = new Color(0.12f, 0.15f, 0.16f); // 浅暗青灰背景（不压抑）
    private readonly Color TitleColor = new Color(0.92f, 0.98f, 0.96f); // 标题浅青白色（醒目不刺眼）
    private readonly Color ButtonNormalColor = new Color(0.32f, 0.78f, 0.72f); // 按钮默认色：浅青绿
    private readonly Color ButtonGradientHighlight = new Color(1, 1, 1, 0.25f); // 按钮渐变高光（顶部提亮）
    private readonly Color ButtonBorderColor = new Color(0.18f, 0.58f, 0.52f); // 按钮边框：深一点的青绿（不抢戏）
    private readonly Color ButtonHighlightColor = new Color(0.42f, 0.85f, 0.78f); // 按钮高亮：稍亮青绿
    private readonly Color ButtonPressColor = new Color(0.22f, 0.68f, 0.62f); // 按钮按下：稍深青绿
    private readonly Color TextColor = Color.white; // 文本白色（对比清晰）
    private readonly Color ShadowColor = new Color(0, 0, 0, 0.3f); // 阴影半透明黑（柔和不生硬）
    // =====================================================================

    void Start()
    {
        // 基础相机设置（改用新背景色）
        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Camera.main.backgroundColor = BgColor;

        // 创建事件系统
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        // 初始化资源（字体和圆角图片）
        InitializeResources();

        // 创建画布
        CreateCanvas();

        // 添加标题（用新标题色）
        CreateMenuTitle();

        // 创建按钮：第一个按钮名改为“进入游戏”
        CreateStyledButton("进入游戏", () =>
        {
            DestroyUI();
            SceneManager.LoadScene("House"); // 场景名不变，仅按钮显示文本修改
        });

        CreateStyledButton("退出游戏", () =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });

        Canvas.ForceUpdateCanvases();
    }

    // 初始化字体和圆角图片（兼容旧版本）
    private void InitializeResources()
    {
        // 初始化字体
        uiFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (uiFont == null)
        {
            uiFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
            if (uiFont == null)
            {
                Debug.LogError("无法加载字体，请检查Unity资源");
            }
        }

        // 尝试加载圆角图片（如果没有则用纯色替代）
        try
        {
            roundedCornerSprite = Resources.Load<Sprite>("rounded_corner");
        }
        catch
        {
            Debug.LogWarning("未找到圆角图片，将使用纯色按钮");
        }
    }

    // 创建画布（适配旧版本）
    private void CreateCanvas()
    {
        GameObject canvasObj = new GameObject("UICanvas");
        canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000;

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;
        scaler.referencePixelsPerUnit = 100;

        canvasObj.AddComponent<GraphicRaycaster>();
    }

    // 创建菜单标题（用新配色）
    private void CreateMenuTitle()
    {
        GameObject titleObj = new GameObject("MenuTitle");
        titleObj.transform.SetParent(canvas.transform);

        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 0.5f);
        titleRect.anchorMax = new Vector2(0.5f, 0.5f);
        titleRect.pivot = new Vector2(0.5f, 0.5f);
        titleRect.anchoredPosition = new Vector2(0, buttonYOffset + ButtonHeight + 80);
        titleRect.sizeDelta = new Vector2(800, 100);

        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = "游戏主菜单";
        titleText.font = uiFont;
        titleText.fontSize = 72;
        titleText.color = TitleColor; // 改用新标题色
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.fontStyle = FontStyle.Bold;

        // 阴影改用新阴影色
        Shadow titleShadow = titleObj.AddComponent<Shadow>();
        titleShadow.effectColor = ShadowColor;
        titleShadow.effectDistance = new Vector2(3, -3);
    }

    // 创建兼容旧版本的样式按钮（用新配色）
    private void CreateStyledButton(string text, System.Action onClick)
    {
        // 按钮主体
        GameObject buttonObj = new GameObject($"{text}Button");
        buttonObj.transform.SetParent(canvas.transform);
        buttonObj.SetActive(true);

        // 按钮位置和大小
        RectTransform btnRect = buttonObj.AddComponent<RectTransform>();
        btnRect.anchorMin = new Vector2(0.5f, 0.5f);
        btnRect.anchorMax = new Vector2(0.5f, 0.5f);
        btnRect.pivot = new Vector2(0.5f, 0.5f);
        btnRect.sizeDelta = new Vector2(ButtonWidth, ButtonHeight);
        btnRect.anchoredPosition = new Vector2(0, buttonYOffset);

        // 调整下一个按钮位置
        buttonYOffset -= ButtonSpacing;

        // 按钮背景（改用新按钮色）
        Image buttonImage = buttonObj.AddComponent<Image>();

        // 圆角处理（使用图片或纯色）
        if (roundedCornerSprite != null)
        {
            buttonImage.sprite = roundedCornerSprite;
            buttonImage.type = Image.Type.Sliced; // 九宫格拉伸
            buttonImage.color = ButtonNormalColor; // 圆角图片时，用新按钮色染色
        }
        else
        {
            // 没有圆角图片时，直接用新按钮色
            buttonImage.color = ButtonNormalColor;
        }

        // 渐变效果（配合新配色，顶部提亮更显质感）
        CreateGradientOverlay(buttonObj);

        // 按钮阴影（改用新阴影色）
        Shadow btnShadow = buttonObj.AddComponent<Shadow>();
        btnShadow.effectColor = ShadowColor;
        btnShadow.effectDistance = new Vector2(5, -5);

        // 按钮边框（改用新边框色）
        GameObject borderObj = new GameObject("ButtonBorder");
        borderObj.transform.SetParent(buttonObj.transform);
        Image borderImage = borderObj.AddComponent<Image>();
        borderImage.color = ButtonBorderColor;

        // 边框使用相同的圆角图片
        if (roundedCornerSprite != null)
        {
            borderImage.sprite = roundedCornerSprite;
            borderImage.type = Image.Type.Sliced;
        }

        RectTransform borderRect = borderObj.GetComponent<RectTransform>();
        borderRect.anchorMin = Vector2.zero;
        borderRect.anchorMax = Vector2.one;
        borderRect.offsetMin = new Vector2(2, 2);
        borderRect.offsetMax = new Vector2(-2, -2);
        borderImage.raycastTarget = false;

        // 按钮文本（颜色不变，保持白色清晰）
        GameObject textObj = new GameObject("ButtonText");
        textObj.transform.SetParent(buttonObj.transform);
        Text buttonText = textObj.AddComponent<Text>();
        buttonText.text = text;
        buttonText.font = uiFont;
        buttonText.fontSize = 48;
        buttonText.color = TextColor;
        buttonText.alignment = TextAnchor.MiddleCenter;
        buttonText.fontStyle = FontStyle.Bold;

        // 文本阴影（改用新阴影色）
        Shadow textShadow = textObj.AddComponent<Shadow>();
        textShadow.effectColor = ShadowColor;
        textShadow.effectDistance = new Vector2(2, -2);

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        // 按钮交互（改用新状态色，过渡更自然）
        Button button = buttonObj.AddComponent<Button>();
        button.onClick.AddListener(() => onClick.Invoke());

        // 按钮状态颜色（新配色的高亮/按下效果）
        ColorBlock colors = button.colors;
        colors.normalColor = Color.white; // 基础色，不影响图片/纯色染色
        colors.highlightedColor = ButtonHighlightColor; // 高亮：稍亮青绿
        colors.pressedColor = ButtonPressColor; // 按下：稍深青绿
        colors.disabledColor = new Color(0.5f, 0.6f, 0.6f); // 禁用：浅灰青绿
        colors.fadeDuration = 0.1f; // 颜色过渡速度
        button.colors = colors;

        // 点击缩放反馈
        ButtonScaleFeedback feedback = buttonObj.AddComponent<ButtonScaleFeedback>();
        feedback.normalScale = Vector3.one;
        feedback.pressedScale = new Vector3(0.95f, 0.95f, 1f);
        feedback.duration = 0.1f;
    }

    // 渐变效果（配合新配色，顶部提亮更显质感）
    private void CreateGradientOverlay(GameObject parent)
    {
        GameObject gradientObj = new GameObject("GradientOverlay");
        gradientObj.transform.SetParent(parent.transform);

        Image gradient = gradientObj.AddComponent<Image>();
        gradient.color = ButtonGradientHighlight; // 改用新渐变高光色（顶部提亮）
        gradient.raycastTarget = false;

        RectTransform gradRect = gradientObj.GetComponent<RectTransform>();
        gradRect.anchorMin = Vector2.zero;
        gradRect.anchorMax = Vector2.one;
        gradRect.offsetMin = Vector2.zero;
        gradRect.offsetMax = Vector2.zero;

        // 用遮罩实现顶部到中部的渐变（提亮顶部，增强立体感）
        GameObject maskObj = new GameObject("GradientMask");
        maskObj.transform.SetParent(gradientObj.transform);
        Mask mask = maskObj.AddComponent<Mask>();
        mask.showMaskGraphic = false;

        RectTransform maskRect = maskObj.GetComponent<RectTransform>();
        maskRect.anchorMin = Vector2.zero;
        maskRect.anchorMax = Vector2.one;
        maskRect.offsetMin = Vector2.zero;
        maskRect.offsetMax = Vector2.zero;

        // 渐变遮罩图形（只显示顶部区域，实现“顶部亮、底部暗”的自然过渡）
        GameObject maskImageObj = new GameObject("MaskImage");
        maskImageObj.transform.SetParent(maskObj.transform);
        Image maskImage = maskImageObj.AddComponent<Image>();
        maskImage.color = Color.white;

        RectTransform maskImageRect = maskImageObj.GetComponent<RectTransform>();
        maskImageRect.anchorMin = new Vector2(0, 0.5f); // 从垂直中间开始
        maskImageRect.anchorMax = Vector2.one; // 到顶部结束
        maskImageRect.offsetMin = Vector2.zero;
        maskImageRect.offsetMax = Vector2.zero;
    }

    private void DestroyUI()
    {
        if (canvas != null)
            Destroy(canvas.gameObject);
    }
}

// 按钮缩放反馈组件（兼容所有版本）
public class ButtonScaleFeedback : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public Vector3 normalScale;
    public Vector3 pressedScale;
    public float duration = 0.1f;

    private bool isPressed = false;
    private float currentTime = 0;

    void Update()
    {
        if (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / duration;
            t = Mathf.SmoothStep(0, 1, t);
            transform.localScale = Vector3.Lerp(
                isPressed ? normalScale : pressedScale,
                isPressed ? pressedScale : normalScale,
                t
            );
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        currentTime = 0;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        currentTime = 0;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPressed = false;
        currentTime = 0;
    }
}