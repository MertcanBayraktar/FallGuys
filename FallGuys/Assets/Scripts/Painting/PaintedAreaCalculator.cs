using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintedAreaCalculator : MonoBehaviour
{
	public ComputeShader compute_shader;
	public RenderTexture render_texture;
	public Material material;
	private GUIStyle gui_style = new GUIStyle();
	[HideInInspector] public uint[] data;
	protected ComputeBuffer compute_buffer;
	public GameObject plane;
	int handle_init;
	int handle_main;
	public float percent;

	void Start()
	{
		material = plane.GetComponent<Renderer>().material;
		handle_init = compute_shader.FindKernel("CSInit");
		handle_main = compute_shader.FindKernel("CSMain");
		compute_buffer = new ComputeBuffer(1, sizeof(uint));
		data = new uint[1];
		compute_shader.SetTexture(handle_main, "image", render_texture);
		compute_shader.SetBuffer(handle_main, "compute_buffer", compute_buffer);
		compute_shader.SetBuffer(handle_init, "compute_buffer", compute_buffer);
	}

	void OnDestroy()
	{
		if (null != compute_buffer)
		{
			compute_buffer.Release();
			compute_buffer = null;
		}
	}

	void Update()
	{
		compute_shader.Dispatch(handle_init, 64, 1, 1);
		compute_shader.Dispatch(handle_main, render_texture.width / 8, render_texture.height / 8, 1);
		compute_buffer.GetData(data);
		uint result = data[0];

		percent = ((float)result / ((float)render_texture.width * (float)render_texture.height)) * 100.0f;
		Graphics.Blit(material.mainTexture, render_texture, material);
	}

	void OnGUI()
	{
		if (GUIManager.instance != null)
		{
			if (TexturePainter.instance.playerHasStartedPainting)
			{
				gui_style.fontSize = 40;
				GUI.Label(new Rect(10, 5, 300, 20), "Percents " + percent.ToString(), gui_style);
			}
		}
	}

}

