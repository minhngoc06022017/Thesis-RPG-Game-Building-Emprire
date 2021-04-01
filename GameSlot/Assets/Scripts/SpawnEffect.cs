using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    [SerializeField] Transform _initialTransform;
    [SerializeField] GameObject _lineX, _upLine, _downLine, _scatter;
    public int index, length, type;
    public int[,] _position=new int[5,2];
    //type 0:scatter , 1: lineX , 2:downLine , 3: upLine
    //length : number of object
    //index:line

    public void _spawnObject()
    {
        for(int i = 0; i < length; i++)
        {
            Debug.Log("yes");
            GameObject scatter=Instantiate(_scatter, new Vector2((_initialTransform.position.x + _position[i,0]*3.75f), (_initialTransform.position.y - (_position[i, 1] *2.5f))), Quaternion.identity);
            Destroy(scatter,1);
        }
        if (type != 0)
        {
            switch (type)
            {
                case 1:
                    GameObject lineX=Instantiate(_lineX, new Vector2(_initialTransform.position.x - 0.5f, _initialTransform.position.y - index*2.5f), Quaternion.identity);
                    Destroy(lineX,1);
                    break;
                case 2:
                    GameObject downLine=Instantiate(_downLine, new Vector2(_initialTransform.position.x - 0.5f, _initialTransform.position.y), Quaternion.identity);
                    Destroy(downLine,1);
                    break;
                case 3:
                    GameObject upLine=Instantiate(_upLine, new Vector2(_initialTransform.position.x - 0.5f, _initialTransform.position.y - 2.5f*2), Quaternion.identity);
                    Destroy(upLine,1);
                    break;
            }
        }
    }
}
