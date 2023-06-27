using System.Collections.Generic;
using System.Linq;
using Pixelplacement;
using UnityEngine;

public class BeaverSpawnController : Singleton<BeaverSpawnController>
{

    [SerializeField] private BeaverController _beaverPrefab;
    [SerializeField] List<BeaverController> _beaversPool;

    private void Awake()
    {
        for(int i = 0; i < GlobalSettings.Instance.MaxBeaverCount; i++)
        {
            var beaverInstance = Instantiate(_beaverPrefab);
            _beaversPool.Add(beaverInstance);
            beaverInstance.gameObject.SetActive(false);
        }
    }

    public void TrySpawnBeaverFromPool(Vector2 position)
    {
        var inactiveBeaver = _beaversPool.FirstOrDefault(x => !x.gameObject.activeSelf);
        if (inactiveBeaver == null)
            return;

        inactiveBeaver.transform.position = position;
        inactiveBeaver.gameObject.SetActive(true);
    }
}
