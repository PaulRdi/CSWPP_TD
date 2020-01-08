using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ConstructionController : MonoBehaviour
{
    Camera camera;
    [SerializeField] LayerMask buildableLayer;
    [SerializeField] LayerMask boundingBoxLayer;

    Bounds currBounds;
    private void Awake()
    {
        camera = FindObjectOfType<Camera>();
    }
    public TowerController Build(TowerController tower, Vector3 point)
    {
        TowerController inst = Instantiate(tower.gameObject).GetComponent<TowerController>();
        inst.transform.position = point;
        return inst;
    }
    
    public bool CanBuild(TowerController tower, out Vector3 point)
    {
        point = Vector3.zero;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);
        if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, buildableLayer))
        {
            point = hit.point;
            //Bounds sollen auf der Ebene stehen! Deswegen die addition!
            Vector3 center = hit.point + tower.boundingBox.center;
            Collider[] res = new Collider[1];
            if (Physics.OverlapBoxNonAlloc(center, tower.boundingBox.size, res, Quaternion.identity, boundingBoxLayer) > 0)
            {
                return false;
            }
            return true;
        }
        return false;
        
    }
}
