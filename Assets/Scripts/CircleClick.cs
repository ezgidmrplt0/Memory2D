using UnityEngine;

public class CircleClick : MonoBehaviour
{
    private CircleManager manager;
    private SpriteRenderer sr;
    private Color originalColor;
    private int myIndex;

    void Start()
    {
        manager = FindObjectOfType<CircleManager>();
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;

        myIndex = System.Array.IndexOf(manager.circles, gameObject);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
                OnClicked();
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
                OnClicked();
        }
    }

    void OnClicked()
    {
        if (!manager.canClick) return; // 🔒 sırada tıklama engeli

        // CircleManager’a index gönder
        manager.CheckPlayerChoice(myIndex);

        // Tıklandıysa bir daha tıklanamasın 0.2 sn
        manager.canClick = false;

        // Kısa animasyon
        sr.color = new Color(
            originalColor.r * 0.8f,
            originalColor.g * 0.8f,
            originalColor.b * 0.8f,
            1f
        );

        // 0.2 saniye sonra rengi resetle ve tıklamayı aç
        Invoke(nameof(ResetClick), 0.2f);
    }

    void ResetClick()
    {
        sr.color = originalColor;
        manager.canClick = true;
    }


    public void ResetColor()
    {
        sr.color = originalColor;
    }
}
