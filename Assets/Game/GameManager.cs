using UnityEngine;
using System.Collections;

namespace Game
{
    public class GameManager : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            float count = Mathf.Floor(Mathf.Sqrt(Camera.allCamerasCount));
            Camera[] cameras = Camera.allCameras;
            int index = 0;
            for (int i = 0; i < count; i++)
            {
                float y = i / count;
                for (int j = 0; j < count; j++)
                {
                    float x = j / count;
                    Camera camera = cameras[index];
                    index++;
                    camera.rect = new Rect(x, y, 1 / count, 1 / count);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}