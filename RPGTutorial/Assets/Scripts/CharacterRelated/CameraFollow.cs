using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    private Transform target;

    private float xMax, xMin, yMax, yMin;

    [SerializeField]
    private Tilemap tileMap;

    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        player = target.GetComponent<Player>();
        
        Vector3 minTile = tileMap.CellToWorld(tileMap.cellBounds.min);
        Vector3 maxTile = tileMap.CellToWorld(tileMap.cellBounds.max);

        SetLimits(minTile, maxTile);
        player.SetLimits(minTile, maxTile);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(target.position.x, xMin - 10, xMax + 10), Mathf.Clamp(target.position.y, yMin, yMax),-10);
    }

    private void SetLimits(Vector3 minTile, Vector3 maxTile)
    {
        Camera cam = Camera.main;

        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        xMin = minTile.x + width / 2;
        xMax = maxTile.x - width / 2;

        yMin = minTile.y + height / 2;
        yMax = maxTile.y - height / 2;
    }
}
