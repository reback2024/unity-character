using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Initial : MonoBehaviour
{
    private Canvas canvas;
    private Font uiFont;
    private float buttonYOffset = 50; // ��ť��ʼƫ�ƣ���ϱ��⣩
    private const float ButtonSpacing = 200; // ��ť���
    private const float ButtonWidth = 550;   // ��ť���
    private const float ButtonHeight = 140;  // ��ť�߶�

    // ����Բ�ǵľŹ���ͼƬ����Ҫ��Resources�ļ�����׼����
    private Sprite roundedCornerSprite;

    // ===================== ����ɫ������Ī��������ɫϵ =====================
    private readonly Color BgColor = new Color(0.12f, 0.15f, 0.16f); // ǳ����ұ�������ѹ�֣�
    private readonly Color TitleColor = new Color(0.92f, 0.98f, 0.96f); // ����ǳ���ɫ����Ŀ�����ۣ�
    private readonly Color ButtonNormalColor = new Color(0.32f, 0.78f, 0.72f); // ��ťĬ��ɫ��ǳ����
    private readonly Color ButtonGradientHighlight = new Color(1, 1, 1, 0.25f); // ��ť����߹⣨����������
    private readonly Color ButtonBorderColor = new Color(0.18f, 0.58f, 0.52f); // ��ť�߿���һ������̣�����Ϸ��
    private readonly Color ButtonHighlightColor = new Color(0.42f, 0.85f, 0.78f); // ��ť��������������
    private readonly Color ButtonPressColor = new Color(0.22f, 0.68f, 0.62f); // ��ť���£���������
    private readonly Color TextColor = Color.white; // �ı���ɫ���Ա�������
    private readonly Color ShadowColor = new Color(0, 0, 0, 0.3f); // ��Ӱ��͸���ڣ���Ͳ���Ӳ��
    // =====================================================================

    void Start()
    {
        // ����������ã������±���ɫ��
        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Camera.main.backgroundColor = BgColor;

        // �����¼�ϵͳ
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        // ��ʼ����Դ�������Բ��ͼƬ��
        InitializeResources();

        // ��������
        CreateCanvas();

        // ��ӱ��⣨���±���ɫ��
        CreateMenuTitle();

        // ������ť����һ����ť����Ϊ��������Ϸ��
        CreateStyledButton("������Ϸ", () =>
        {
            DestroyUI();
            SceneManager.LoadScene("House"); // ���������䣬����ť��ʾ�ı��޸�
        });

        CreateStyledButton("�˳���Ϸ", () =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });

        Canvas.ForceUpdateCanvases();
    }

    // ��ʼ�������Բ��ͼƬ�����ݾɰ汾��
    private void InitializeResources()
    {
        // ��ʼ������
        uiFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (uiFont == null)
        {
            uiFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
            if (uiFont == null)
            {
                Debug.LogError("�޷��������壬����Unity��Դ");
            }
        }

        // ���Լ���Բ��ͼƬ�����û�����ô�ɫ�����
        try
        {
            roundedCornerSprite = Resources.Load<Sprite>("rounded_corner");
        }
        catch
        {
            Debug.LogWarning("δ�ҵ�Բ��ͼƬ����ʹ�ô�ɫ��ť");
        }
    }

    // ��������������ɰ汾��
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

    // �����˵����⣨������ɫ��
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
        titleText.text = "��Ϸ���˵�";
        titleText.font = uiFont;
        titleText.fontSize = 72;
        titleText.color = TitleColor; // �����±���ɫ
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.fontStyle = FontStyle.Bold;

        // ��Ӱ��������Ӱɫ
        Shadow titleShadow = titleObj.AddComponent<Shadow>();
        titleShadow.effectColor = ShadowColor;
        titleShadow.effectDistance = new Vector2(3, -3);
    }

    // �������ݾɰ汾����ʽ��ť��������ɫ��
    private void CreateStyledButton(string text, System.Action onClick)
    {
        // ��ť����
        GameObject buttonObj = new GameObject($"{text}Button");
        buttonObj.transform.SetParent(canvas.transform);
        buttonObj.SetActive(true);

        // ��ťλ�úʹ�С
        RectTransform btnRect = buttonObj.AddComponent<RectTransform>();
        btnRect.anchorMin = new Vector2(0.5f, 0.5f);
        btnRect.anchorMax = new Vector2(0.5f, 0.5f);
        btnRect.pivot = new Vector2(0.5f, 0.5f);
        btnRect.sizeDelta = new Vector2(ButtonWidth, ButtonHeight);
        btnRect.anchoredPosition = new Vector2(0, buttonYOffset);

        // ������һ����ťλ��
        buttonYOffset -= ButtonSpacing;

        // ��ť�����������°�ťɫ��
        Image buttonImage = buttonObj.AddComponent<Image>();

        // Բ�Ǵ���ʹ��ͼƬ��ɫ��
        if (roundedCornerSprite != null)
        {
            buttonImage.sprite = roundedCornerSprite;
            buttonImage.type = Image.Type.Sliced; // �Ź�������
            buttonImage.color = ButtonNormalColor; // Բ��ͼƬʱ�����°�ťɫȾɫ
        }
        else
        {
            // û��Բ��ͼƬʱ��ֱ�����°�ťɫ
            buttonImage.color = ButtonNormalColor;
        }

        // ����Ч�����������ɫ���������������ʸУ�
        CreateGradientOverlay(buttonObj);

        // ��ť��Ӱ����������Ӱɫ��
        Shadow btnShadow = buttonObj.AddComponent<Shadow>();
        btnShadow.effectColor = ShadowColor;
        btnShadow.effectDistance = new Vector2(5, -5);

        // ��ť�߿򣨸����±߿�ɫ��
        GameObject borderObj = new GameObject("ButtonBorder");
        borderObj.transform.SetParent(buttonObj.transform);
        Image borderImage = borderObj.AddComponent<Image>();
        borderImage.color = ButtonBorderColor;

        // �߿�ʹ����ͬ��Բ��ͼƬ
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

        // ��ť�ı�����ɫ���䣬���ְ�ɫ������
        GameObject textObj = new GameObject("ButtonText");
        textObj.transform.SetParent(buttonObj.transform);
        Text buttonText = textObj.AddComponent<Text>();
        buttonText.text = text;
        buttonText.font = uiFont;
        buttonText.fontSize = 48;
        buttonText.color = TextColor;
        buttonText.alignment = TextAnchor.MiddleCenter;
        buttonText.fontStyle = FontStyle.Bold;

        // �ı���Ӱ����������Ӱɫ��
        Shadow textShadow = textObj.AddComponent<Shadow>();
        textShadow.effectColor = ShadowColor;
        textShadow.effectDistance = new Vector2(2, -2);

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        // ��ť������������״̬ɫ�����ɸ���Ȼ��
        Button button = buttonObj.AddComponent<Button>();
        button.onClick.AddListener(() => onClick.Invoke());

        // ��ť״̬��ɫ������ɫ�ĸ���/����Ч����
        ColorBlock colors = button.colors;
        colors.normalColor = Color.white; // ����ɫ����Ӱ��ͼƬ/��ɫȾɫ
        colors.highlightedColor = ButtonHighlightColor; // ��������������
        colors.pressedColor = ButtonPressColor; // ���£���������
        colors.disabledColor = new Color(0.5f, 0.6f, 0.6f); // ���ã�ǳ������
        colors.fadeDuration = 0.1f; // ��ɫ�����ٶ�
        button.colors = colors;

        // ������ŷ���
        ButtonScaleFeedback feedback = buttonObj.AddComponent<ButtonScaleFeedback>();
        feedback.normalScale = Vector3.one;
        feedback.pressedScale = new Vector3(0.95f, 0.95f, 1f);
        feedback.duration = 0.1f;
    }

    // ����Ч�����������ɫ���������������ʸУ�
    private void CreateGradientOverlay(GameObject parent)
    {
        GameObject gradientObj = new GameObject("GradientOverlay");
        gradientObj.transform.SetParent(parent.transform);

        Image gradient = gradientObj.AddComponent<Image>();
        gradient.color = ButtonGradientHighlight; // �����½���߹�ɫ������������
        gradient.raycastTarget = false;

        RectTransform gradRect = gradientObj.GetComponent<RectTransform>();
        gradRect.anchorMin = Vector2.zero;
        gradRect.anchorMax = Vector2.one;
        gradRect.offsetMin = Vector2.zero;
        gradRect.offsetMax = Vector2.zero;

        // ������ʵ�ֶ������в��Ľ��䣨������������ǿ����У�
        GameObject maskObj = new GameObject("GradientMask");
        maskObj.transform.SetParent(gradientObj.transform);
        Mask mask = maskObj.AddComponent<Mask>();
        mask.showMaskGraphic = false;

        RectTransform maskRect = maskObj.GetComponent<RectTransform>();
        maskRect.anchorMin = Vector2.zero;
        maskRect.anchorMax = Vector2.one;
        maskRect.offsetMin = Vector2.zero;
        maskRect.offsetMax = Vector2.zero;

        // ��������ͼ�Σ�ֻ��ʾ��������ʵ�֡����������ײ���������Ȼ���ɣ�
        GameObject maskImageObj = new GameObject("MaskImage");
        maskImageObj.transform.SetParent(maskObj.transform);
        Image maskImage = maskImageObj.AddComponent<Image>();
        maskImage.color = Color.white;

        RectTransform maskImageRect = maskImageObj.GetComponent<RectTransform>();
        maskImageRect.anchorMin = new Vector2(0, 0.5f); // �Ӵ�ֱ�м俪ʼ
        maskImageRect.anchorMax = Vector2.one; // ����������
        maskImageRect.offsetMin = Vector2.zero;
        maskImageRect.offsetMax = Vector2.zero;
    }

    private void DestroyUI()
    {
        if (canvas != null)
            Destroy(canvas.gameObject);
    }
}

// ��ť���ŷ���������������а汾��
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