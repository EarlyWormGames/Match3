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
    private ModelCamera ModelCam;
    private Camera ActualCam;

    private MeshRenderer[] meshes = new MeshRenderer[0];

    // Use this for initialization
    void Awake()
    {
        instance = this;

        meshes = new MeshRenderer[Models.Length];

        int width = Mathf.RoundToInt(Mathf.Sqrt(Models.Length));
        int x = 0, y = 0;

        GameObject camera = new GameObject("ModelViewer", new System.Type[] { typeof(Camera), typeof(ModelCamera) });
        camera.transform.SetParent(transform, true);
        ModelCam = camera.GetComponent<ModelCamera>();
        ActualCam = camera.GetComponent<Camera>();

        for (int i = 0; i < Models.Length; ++i)
        {
            //Spawn a model
            GameObject model = Instantiate(Models[i].prefab);
            model.GetComponent<MeshRenderer>().sharedMaterial = DefaultMat;

            //Parent the model to this, to make it neater
            model.transform.SetParent(transform, true);
            meshes[i] = model.GetComponent<MeshRenderer>();

            //Position the model
            model.transform.position = transform.position + new Vector3(x * xDist, y * yDist, 0);
            //Rotate the model
            model.transform.rotation = Quaternion.Euler(Models[i].rotation);

            ++x;
            if (x >= width)
            {
                ++y;
                x = 0;
            }
        }

        Bounds boundingBox = new Bounds();
        for (int i = 0; i < meshes.Length; ++i)
        {
            if (i == 0)
            {
                boundingBox = meshes[i].bounds;
            }
            else
            {
                boundingBox.Encapsulate(meshes[i].bounds);
            }
        }

        ActualCam.orthographicSize = GetRequiredOrthographicSize(boundingBox, ModelCam.m_Texture.width, ModelCam.m_Texture.height);
        ActualCam.transform.position = new Vector3(boundingBox.center.x, boundingBox.center.y, -10f);
    }

    public RenderTexture GetTexture()
    {
        ////Enable the camera if need be
        //if (Cameras[a_index].RenderCount <= 0)
        //{
        //    Cameras[a_index].camera.enabled = true;
        //}
        //++Cameras[a_index].RenderCount;
        return ModelCam.m_Texture;
    }

    public Rect UsePortion(int a_index)
    {
        Rect r = new Rect();
        float screenSize = ActualCam.orthographicSize * 2;

        float left = ActualCam.transform.position.x - ActualCam.orthographicSize;
        float bottom = ActualCam.transform.position.y - ActualCam.orthographicSize;

        r.x = (meshes[a_index].bounds.min.x - left) / screenSize;
        r.y = (meshes[a_index].bounds.min.y - bottom) / screenSize;

        r.width = meshes[a_index].bounds.size.x / screenSize;
        r.height = meshes[a_index].bounds.size.y / screenSize;

        return r;
    }

    public void UnuseRenderTexture(int a_index)
    {
        //--Cameras[a_index].RenderCount;
        //
        ////Disable the camera if need be
        //if (Cameras[a_index].RenderCount <= 0)
        //{
        //    if (Cameras[a_index].camera != null)
        //        Cameras[a_index].camera.enabled = false;
        //}
    }

    // ------------------------------------------------------
    /// <summary>
    /// Returns an orthographic size which is big enough so all of the points are within    
    /// the viewspace when centered -> Currently only takes X into consideration.
    /// Needs updating so it will take Y bounds into consideration as well
    /// </summary>
    /// <param name="orthographicCamera"> the camera to retrieve the size for </param>
    /// <param name="positionBounds"> the points which are used to define the edges with.
    ///  Can be any number of points, bounds will be extracted </param>
    /// <returns></returns>
    public float GetRequiredOrthographicSize(Bounds a_targetBounds, int a_width, int a_height)
    {
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = a_targetBounds.size.x / a_targetBounds.size.y;

        return a_targetBounds.size.y / 2;
    }
}
