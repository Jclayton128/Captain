using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] ProjectileBrain _projectilePrefabs = null;

    List<ProjectileBrain> _activeProjectiles = new List<ProjectileBrain>();
    Queue<ProjectileBrain> _pooledProjectiles = new Queue<ProjectileBrain>();

    
    public ProjectileBrain RequestProjectile()
    {
        ProjectileBrain pb;
        if (_pooledProjectiles.Count == 0)
        {
            pb = Instantiate(_projectilePrefabs);
            pb.Initialize(this);
        }
        else
        {
            pb = _pooledProjectiles.Dequeue();
            pb.transform.parent = null;
            
        }
        return pb;
    }


    public void ReturnProjectile(ProjectileBrain expiredProjectile)
    {
        expiredProjectile.enabled = false;
        _activeProjectiles.Remove(expiredProjectile);
        _pooledProjectiles.Enqueue(expiredProjectile);
    }
}
