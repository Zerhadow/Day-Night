
using UnityEngine;

public class TextureScaler : MonoBehaviour
{
    [SerializeField] Vector2 tilingFactor = new Vector2(1, 1);
    [SerializeField] float scaleAmount = 1;
    [SerializeField] float tilingOffset = 0;
    [SerializeField] float textureRotation = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        Renderer objectRenderer = GetComponent<Renderer>();
        Material objectMaterial = objectRenderer.material;

        Bounds objectBounds = objectRenderer.bounds;
        Vector3 objectSize = objectBounds.size;

        Vector2 textureTiling = new Vector2(objectSize.x * tilingFactor.x, objectSize.z * tilingFactor.y);

        objectMaterial.SetTextureScale("_MainTex", textureTiling);
    }
}
