using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PerlinNoiseGen : MonoBehaviour
{
    [System.Serializable]
    public struct PrefabVariant
    {
        public float Weight;
        public GameObject[] Prefabs;

        public bool IsMet(float value)
        {
            return value <= Weight;
        }
        
        public GameObject Get()
        {
            return Prefabs[Random.Range(0, Prefabs.Length)];
        }
    }
    
    public PrefabVariant[] m_prefabs;

    public float m_width = 10;
    public float m_depth = 10;
    public float m_scale = 1f;
    public float m_heightScale = 5f;
    public float m_spacing = 2f;

    public Vector3 m_offset;

    public int m_threshold;
    
    public bool m_execute;
    public Task m_task;

    public bool m_flatten = false;

    void Awake()
    {
    }
    
    void Update()
    {
        if (m_task == null &&
            m_execute)
        {
            Debug.Log("Executing..");
            m_task = Execute();
            m_execute = false;
        }
    }

    private Task Execute()
    {
        if (transform.childCount > 0)
        {
            GameObject.DestroyImmediate(transform.GetChild(0).gameObject);
        }

        var offset = m_offset + (new Vector3(-m_width / 2f, 0, -m_depth / 2f) * m_spacing);
        var width = m_width / m_scale;
        var depth = m_depth / m_scale;
        
        var container = new GameObject();
        container.transform.parent = transform;
        
        for (var y = 0; y < m_depth; ++y)
        {
            for (var x = 0; x < m_width; ++x)
            {
                var value = Mathf.PerlinNoise(x / width, y / depth);
                value = (int)(value * m_heightScale);
                if (value < m_threshold)
                    continue;

                if(m_flatten)
                    value = 0f;

                var weight = Random.Range(0, 100);
                GameObject prefab = null;
                foreach (var entry in m_prefabs)
                {
                    if (entry.IsMet(weight))
                    {
                        prefab = entry.Get();
                        break;
                    }
                }
                
                var position = new Vector3(x * m_spacing, value, y * m_spacing) + offset;

                var crate = GameObject.Instantiate(prefab, position, Quaternion.identity, container.transform);
                crate.name = crate.name.Replace("(Clone)", $"[{x}, {y}]");
            }
        }
        
        Debug.Log("Finished!");
        
        return null;
    }
}
