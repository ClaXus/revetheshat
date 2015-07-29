using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))]

public class TargetScript : MonoBehaviour
{
    [SerializeField]
    Unit unit;
    [SerializeField]
    Camera camera;
    [SerializeField]
    LayerMask layermask;

	void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = camera.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 2000f, layermask))
                {
                    if(hit.collider.tag == "Unit")
                    {
                        unit.Target = hit.collider.GetComponent<Unit>();
                        Debug.Log(hit.collider);
                    }
                    else
                    {
                        unit.Target = null;
                    }
                }
            }
        }
    }
}
