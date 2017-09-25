using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Model
{
    public GameObject prefab;
    public Vector3 rotation;
}

public class ModelSpawner : MonoBehaviour
{
    public static ModelSpawner instance;

    public float xDist = 10, yDist = 10;
    public Material DefaultMat;
    public Model[] Models = new Model[0];
    private ModelCamera[] Cameras;

    // Use this for initialization
    void Awake()
    {
        instance = this;
        Cameras = new ModelCamera[Models.Length];

        int width = Mathf.RoundToInt(Mathf.Sqrt(Models.Length));
        int x = 0, y = 0;

        for (int i = 0; i < Models.Length; ++i)
        {
            //Spawn a camera and a model
            GameObject camera = new GameObject("ModelViewer" + i.ToString(), new System.Type[] { typeof(Camera), typeof(ModelCamera) });
            GameObject model = Instantiate(Models[i].prefab);
            model.GetComponent<MeshRenderer>().sharedMaterial = DefaultMat;

            //Parent the camera and model to this, to make it neater
            camera.transform.SetParent(transform, true);
            model.transform.SetParent(transform, true);

            //Set this index to be the newly spawned camera
            Cameras[i] = camera.GetComponent<ModelCamera>();

            //Position the objects
            model.transform.position = transform.position + new Vector3(x * xDist, y * yDist, 0);
            camera.transform.position = model.transform.position - new Vector3(0, 0, 10);

            //Rotate the model
            model.transform.rotation = Quaternion.Euler(Models[i].rotation);

            ++x;
            if (x >= width)
            {
                ++y;
                x = 0;
            }
        }
    }

    public RenderTexture UseRenderTexture(int a_index)
    {
        //Enable the camera if need be
        if (Cameras[a_index].RenderCount <= 0)
        {
            Cameras[a_index].camera.enabled = true;
        }
        ++Cameras[a_index].RenderCount;

        return Cameras[a_index].m_Texture;
    }

    public void UnuseRenderTexture(int a_index)
    {
        --Cameras[a_index].RenderCount;

        //Disable the camera if need be
        if (Cameras[a_index].RenderCount <= 0)
        {
            if (Cameras[a_index].camera != null)
                Cameras[a_index].camera.enabled = false;
        }
    }
}
