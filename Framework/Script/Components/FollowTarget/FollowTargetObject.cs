using System;
using UnityEngine;

public class FollowTargetObject : MonoBehaviour
{
    GameObject targetObject;
    Camera camera;
    RectTransform targetRectTransform;

    public void SetTargetObject(GameObject targetObject)
    {
        this.targetObject = targetObject;
        
        if (targetObject.transform is RectTransform rct)
        {
            targetRectTransform = rct;
        }
    }

    public void SetCamera(Camera camera)
    {
        this.camera = camera;
    }

    public void LateUpdate()
    {
        if (targetObject == null)
        {
            return;
        }

        if (targetRectTransform !=null)
        {
            if (camera != null)
            {
                var worldPosition = camera.ScreenToWorldPoint(targetRectTransform.position);
                targetObject.transform.position = worldPosition;
            }
        }
        else
        {
            transform.position = targetObject.transform.position;
        }
    }
}