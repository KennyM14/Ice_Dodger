using UnityEngine;
using DG.Tweening; 

public class TittleAnimation : MonoBehaviour
{
    public RectTransform titleImage;
    void Start()
    {
        StartEffect(); 
    }

    private void StartEffect()
    {
        titleImage.DOScale(new Vector3(1.05f, 1.05f, 1f), 0.4f) // Escala (X,Y,Z, tiempo); 
                  .SetEase(Ease.InOutSine)
                  .SetLoops(-1, LoopType.Yoyo);
    }
}
