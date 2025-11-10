using UnityEngine;
using UnityEngine.UI;

public class FanShapeLayout : LayoutGroup
{
    [SerializeField]
    private float _horizBase = 950f; // 기본 posX
    [SerializeField]
    private float _verBase = 80f; // 기본 posY
    [SerializeField]
    private float _horizGrowth = 100f; // posX 계수
    [SerializeField]
    private float _verGrowth = 20f; // posY 계수
    [SerializeField]
    private float _angleGrowth = 10f; // 각도 증가 계수
    [SerializeField]
    private float _curveExponent = 2f; // 곡선 강조 계수 (1 = 직선, >1 = 가장자리 강조)

    public override void CalculateLayoutInputHorizontal() { }
    public override void CalculateLayoutInputVertical() { }

    public override void SetLayoutHorizontal() => Arrange();
    public override void SetLayoutVertical() => Arrange();

    private void Arrange()
    {
        int count = transform.childCount;
        if (count == 0) return;

        float totalAngle = (count == 1) ? 0f : _angleGrowth * Mathf.Log(count, 2f); // log로 완만하게 증가
        float startAngle = -totalAngle / 2f;

        float horizScale = _horizBase + Mathf.Pow(count / 3f, 2f) * _horizGrowth;
        float verScale = Mathf.Pow(count / 5f, 2f) * _verGrowth;

        for (int i = 0; i < count; i++)
        {
            RectTransform child = transform.GetChild(i).GetComponent<RectTransform>();
            float t = (count == 1) ? 0.5f : (float)i / (count - 1); // 0~1 보간
            float centerT = Mathf.Abs(t - 0.5f) * 2f; // 0 = 중앙, 1 = 가장자리

            // 카드 위치 각도 계산
            float angle = startAngle + totalAngle * t;
            float rad = angle * Mathf.Deg2Rad;
            // 부채꼴 형태로 배치
            float x = Mathf.Sin(rad) * horizScale;
            float y = -Mathf.Pow(centerT, _curveExponent) * verScale;

            child.localPosition = new Vector3(x, y - _verBase, 0f);
            child.localRotation = Quaternion.Euler(0, 0, -angle);
        }
    }
}