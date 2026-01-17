using System;
using UnityEngine;

// NOTE : UI대상으로 한 Follow기능은 고려되어있지 않음
public class UIFollowTargetObject : MonoBehaviour
{
    [SerializeField] Canvas parentCanvas;

    RectTransform parentRectTransform;
    RectTransform rectTransform;
    GameObject targetObject;
    Camera camera;
    
    void Awake()
    {
        rectTransform = transform as RectTransform;
        parentRectTransform = parentCanvas.transform as RectTransform;
    }

    public void SetTargetObject(GameObject targetObject)
    {
        this.targetObject = targetObject;
    }

    public void SetCamera(Camera camera)
    {
        this.camera = camera;
    }

    private void LateUpdate()
    {
        if (targetObject == null || camera == null)
        {
            return;
        }
        
        var screenPoint = RectTransformUtility.WorldToScreenPoint(camera, targetObject.transform.position);
        
        if (parentCanvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle
            (
                parentRectTransform, screenPoint, camera, out var localPoint
            );
            
            rectTransform.localPosition = localPoint;
        }
        else if (parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            transform.position = screenPoint;
        }
    }
}