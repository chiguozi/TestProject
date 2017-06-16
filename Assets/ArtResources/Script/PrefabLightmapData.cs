using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PrefabLightmapData : MonoBehaviour
{
	[System.Serializable]
	struct RendererInfo
	{
		public Renderer 	renderer;
		public int 			lightmapIndex;
		public Vector4 		lightmapOffsetScale;
	}

	[SerializeField]
	RendererInfo[]	m_RendererInfo;

    [SerializeField] 
    GameObject[] m_Combined;

	[SerializeField]
	Texture2D[] 	m_Lightmaps;

    [SerializeField]
    bool _isFog;
    [SerializeField]
    float _fFogStartDistance;
    [SerializeField]
    float _fFogEndDistance;
    [SerializeField]
    FogMode _eFogMode;
    [SerializeField]
    Color _kFogColor;

    [SerializeField]
    Color _kCameraBackgroundColor;

	void Awake ()
	{
		if (m_RendererInfo == null || m_RendererInfo.Length == 0)
			return;

		var lightmaps = LightmapSettings.lightmaps;
		var combinedLightmaps = new LightmapData[lightmaps.Length + m_Lightmaps.Length];

		lightmaps.CopyTo(combinedLightmaps, 0);
		for (int i = 0; i < m_Lightmaps.Length;i++)
		{
			combinedLightmaps[i+lightmaps.Length] = new LightmapData();
			combinedLightmaps[i+lightmaps.Length].lightmapColor = m_Lightmaps[i];
		}

		ApplyRendererInfo(m_RendererInfo, lightmaps.Length);
		LightmapSettings.lightmaps = combinedLightmaps;

        ApplyMainCameraInfo();
        ApplyFogSettingInfo();

        //动态加载完之后需要调用一下StaticBatchingUtility.Combine(parent)来将整个场景静态化，使得batching能正常使用。
        StaticBatchingUtility.Combine(m_Combined, gameObject);
	}

//    static GameObject[] ToCombineGameObject(RendererInfo[] infos)
//    {
//        int length = infos.Length;
//        List<GameObject> gos = new List<GameObject>(length);
//        for (int i = 0; i < length; i++)
//        {
//            var info = infos[i];
//            if (info.renderer.GetComponent<Animator>() == null)
//            {
//                gos.Add(info.renderer.gameObject);
//            }
//        }
//        return gos.ToArray();
//    }
	
	static void ApplyRendererInfo (RendererInfo[] infos, int lightmapOffsetIndex)
	{
		for (int i=0;i<infos.Length;i++)
		{
			var info = infos[i];
			/*
			#if UNITY_EDITOR
			if (UnityEditor.GameObjectUtility.AreStaticEditorFlagsSet(info.renderer.gameObject, UnityEditor.StaticEditorFlags.BatchingStatic))
			{
				Debug.LogWarning("The renderer " + info.renderer.name + " is marked Batching Static. The static batch is created when building the player. " +
				                 "Setting the lightmap scale and offset will not affect lightmapping UVs as they have the scale and offset already burnt in.", info.renderer);
			}
			#endif
			*/
			info.renderer.lightmapIndex = info.lightmapIndex + lightmapOffsetIndex;
			info.renderer.lightmapScaleOffset = info.lightmapOffsetScale;
		}
	}

#if UNITY_EDITOR
	[UnityEditor.MenuItem("ArtTool/更新场景预设烘焙光照信息")]
	static void GenerateLightmapInfo ()
	{
		if (UnityEditor.Lightmapping.giWorkflowMode != UnityEditor.Lightmapping.GIWorkflowMode.OnDemand)
		{
			Debug.LogError("ExtractLightmapData requires that you have baked you lightmaps and Auto mode is disabled.");
			return;
		}
		// UnityEditor.Lightmapping.Bake();

		PrefabLightmapData[] prefabs = FindObjectsOfType<PrefabLightmapData>();

		foreach (var instance in prefabs)
		{
			var gameObject = instance.gameObject;
			var rendererInfos = new List<RendererInfo>();
			var lightmaps = new List<Texture2D>();
		    var combineds = new List<GameObject>();
			
			GenerateLightmapInfo(gameObject, rendererInfos, lightmaps);

		    GenerateCombinedInfo(gameObject, combineds);
			
			instance.m_RendererInfo = rendererInfos.ToArray();
		    instance.m_Combined = combineds.ToArray();
			instance.m_Lightmaps = lightmaps.ToArray();

            SaveMainCameraInfo(instance);
            SaveFogSettingInfo(instance);

			var targetPrefab = UnityEditor.PrefabUtility.GetPrefabParent(gameObject) as GameObject;
			if (targetPrefab != null)
			{
				//UnityEditor.Prefab
				UnityEditor.PrefabUtility.ReplacePrefab(gameObject, targetPrefab);
			}
		}
	}

	static void GenerateLightmapInfo (GameObject root, List<RendererInfo> rendererInfos, List<Texture2D> lightmaps)
	{
		var renderers = root.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer renderer in renderers)
		{
			if (renderer.lightmapIndex != -1)
			{
				RendererInfo info = new RendererInfo();
				info.renderer = renderer;
				info.lightmapOffsetScale = renderer.lightmapScaleOffset;

				Texture2D lightmap = LightmapSettings.lightmaps[renderer.lightmapIndex].lightmapColor;

				info.lightmapIndex = lightmaps.IndexOf(lightmap);
				if (info.lightmapIndex == -1)
				{
					info.lightmapIndex = lightmaps.Count;
					lightmaps.Add(lightmap);
				}

				rendererInfos.Add(info);
			}
		}
	}

    static void GenerateCombinedInfo(GameObject root, List<GameObject> combines)
    {
        List<Transform> childs = new List<Transform>();
        childs.Add(root.transform);
        while (childs.Count > 0)
        {
            Transform node = childs[0];
            childs.RemoveAt(0);
            if (node.GetComponent<Animator>() != null)
                continue;

            combines.Add(node.gameObject);
            int childCount = node.childCount;
            for (int i = 0; i < childCount; i++)
            {
                childs.Add(node.GetChild(i));
            }
        }
        combines.RemoveAt(0);
    }

#endif

    static void SaveMainCameraInfo(PrefabLightmapData kInstance)
    {
        if (Camera.main != null)
            kInstance._kCameraBackgroundColor = Camera.main.backgroundColor;
        else
        {
            Debug.LogError("Camera.main == null!!!!");
        }
    }

    void ApplyMainCameraInfo()
    {
        if (Camera.main != null)
            Camera.main.backgroundColor = _kCameraBackgroundColor;
        else
        {
            Debug.LogError("Camera.main == null!!!!");
        }
    }

    static void SaveFogSettingInfo(PrefabLightmapData kInstance)
    {
        kInstance._isFog = RenderSettings.fog;
        kInstance._eFogMode = RenderSettings.fogMode;
        kInstance._kFogColor = RenderSettings.fogColor;
        kInstance._fFogStartDistance = RenderSettings.fogStartDistance;
        kInstance._fFogEndDistance = RenderSettings.fogEndDistance;
    }

    void ApplyFogSettingInfo()
    {
        RenderSettings.fog = _isFog;
        RenderSettings.fogMode = _eFogMode;
        RenderSettings.fogColor = _kFogColor;
        RenderSettings.fogStartDistance = _fFogStartDistance;
        RenderSettings.fogEndDistance = _fFogEndDistance;
    }
}