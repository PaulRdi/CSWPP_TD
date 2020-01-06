using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuildingController : MonoBehaviour
{
    List<TowerController> builtTowers;
    Camera camera;
    [SerializeField] LayerMask buildableLayer;

    private void Awake()
    {
        builtTowers = new List<TowerController>();
        camera = FindObjectOfType<Camera>();
    }
    public TowerController Build(TowerController tower, Vector3 point)
    {
        TowerController inst = Instantiate(tower);
        inst.transform.position = point;
        builtTowers.Add(tower);
        return inst;
    }
    
    public bool CanBuild(TowerController tower, out Vector3 point)
    {
        point = Vector3.zero;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);
        if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, buildableLayer))
        {
            point = hit.point;
            Bounds boundsToTest = tower.boundingBox.bounds;
            //Bounds sollen auf der ebene stehen! Deswegen die addition!
            boundsToTest.center = hit.point + new Vector3(0f, boundsToTest.extents.y / 2.0f, 0f);
            if (builtTowers.Any(tc => tc.boundingBox.bounds.Intersects(boundsToTest)))
            {
                return false;
            }

            return true;
        }

        return false;
        
    }
}
