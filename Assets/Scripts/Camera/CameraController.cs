using System.Collections.Generic;
using Pixelplacement;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] private float _minimalHorizontalSize;

    public List<Camera> Cameras;

    protected virtual void Awake()
    {
        foreach(var camera in Cameras){
            var verticalSize   = camera.orthographicSize * 2.0f;
            
            float ar = Screen.width / (float)Screen.height;
            
            var horizontalSize = verticalSize * ar;
        
            if (horizontalSize < _minimalHorizontalSize)
            {
                horizontalSize = _minimalHorizontalSize;
                verticalSize = horizontalSize / ar;
                camera.orthographicSize = verticalSize / 2;
            }
        }       
    }

    public void Slide()
    {
        Tween.Position(transform, new Vector3(0, transform.position.y, -10), 1, 0);
    }
}
