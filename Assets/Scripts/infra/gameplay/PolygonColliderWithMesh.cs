using UnityEngine;

namespace Infra.Gameplay {
	/// <summary>
	/// Updates its mesh according to its collider.
	/// </summary>
	[RequireComponent(typeof(PolygonCollider2D), typeof(MeshFilter))]
	[ExecuteInEditMode]
	public class PolygonColliderWithMesh : MonoBehaviour {
		[SerializeField] bool autoUpdateToMatchPolygon;

		private PolygonCollider2D polygonCollider;
		private MeshFilter meshFilter;

		public Vector2[] hardcoded_uvs;

		protected void Awake() {
			polygonCollider = GetComponent<PolygonCollider2D>();
			meshFilter = GetComponent<MeshFilter>();
			if (Application.isPlaying) {
				// In edit mode, this will cause leaks.
				InstantiateNewMesh();
			}
		}

		[ContextMenu("Instantiate New Mesh")]
		private void InstantiateNewMesh() {
			meshFilter.sharedMesh = meshFilter.mesh;
		}

		protected void Update() {
			if (autoUpdateToMatchPolygon) {
				UpdateMesh();
			}
		}

		/// <summary>
		/// Sets the bounding points of the mesh and the collider.
		/// </summary>
		public void SetPoints(Vector2[] points) {
			polygonCollider.points = points;
			UpdateMesh();
		}

		/// <summary>
		/// Sets the bounding points of the mesh to the collider.
		/// </summary>
		public void UpdateMesh() {
			var points = polygonCollider.points;

			// Triangulate points for mesh.
			int[] indices = Triangulator.Triangulate(points);

			// Create 3D mesh vertices.
			var vertices = new Vector3[points.Length];
			for (int i = 0; i < points.Length; i++) {
				var point = points[i];
				vertices[i] = point;
			}
				
			// Reset mesh.
			meshFilter.sharedMesh.Clear();
			meshFilter.sharedMesh.vertices = vertices;
			meshFilter.sharedMesh.triangles = indices;
			meshFilter.sharedMesh.uv = hardcoded_uvs;
			meshFilter.sharedMesh.RecalculateBounds();
		}
	}
}
