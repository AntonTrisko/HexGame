using DG.Tweening;
using UnityEngine;

public class BuildingWindow : BasicWindow
{
    public override void OpenWindow()
    {
        transform.localScale = Vector3.zero;
        base.OpenWindow();
        transform.DOScale(Vector3.one, 0.5f);
    }
}
