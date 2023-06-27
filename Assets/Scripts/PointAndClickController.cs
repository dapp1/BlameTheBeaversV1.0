using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointAndClickController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity);
            var hitObjects = new List<ClickableObject>();
            
            foreach (var hit in hits)
            {
                if (hit.collider == null)
                    return;

                var clickable = hit.collider.GetComponent<ClickableObject>();
                if (clickable != null)
                    hitObjects.Add(clickable);
            }

            var hitObject = hitObjects
                .OrderByDescending(x => x.ClickOrderPriority)
                .FirstOrDefault();
            
            if (hitObject != null)
                hitObject.OnClick();
        }
    }
}
