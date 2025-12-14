using UnityEngine;
using slc.NIGHTSWIM.WaterSystem;
using slc.NIGHTSWIM.Utilities;

namespace slc.NIGHTSWIM.Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class WaterFloat : MonoBehaviour
    {
        [Header("Public Properties")]
        public float airDrag = 1.0f;
        public float waterDrag = 10.0f;
        public bool affectDirection = true;
        public bool attachToSurface = false;
        public Transform[] FloatPoints;

        [Space]
        protected Rigidbody m_rigidbody;
        [SerializeField] protected WaterSurfaceController m_waterSurfaceController;

        protected float WaterLine;
        protected Vector3[] WaterLinePoints;

        protected Vector3 smoothVectorRotation;
        protected Vector3 TargetUp;
        protected Vector3 centerOffset;

        public Vector3 Center => transform.position + centerOffset;

        // Start is called before the first frame update
        void Awake()
        {
            // Get components
            if (m_waterSurfaceController == null)
                m_waterSurfaceController = FindObjectOfType<WaterSurfaceController>();

            m_rigidbody = GetComponent<Rigidbody>();
            m_rigidbody.useGravity = false;

            // Compute center
            WaterLinePoints = new Vector3[FloatPoints.Length];
            for (int i = 0; i < FloatPoints.Length; i++)
            {
                WaterLinePoints[i] = FloatPoints[i].position;
            }

            centerOffset = PhysicsHelper.GetCenter(WaterLinePoints) - transform.position;
        }

        private void FixedUpdate()
        {
            // Update water line points
            UpdateWaterLinePoints();

            // Compute gravity and drag
            UpdateGravityAndDrag();

            // Apply rotation if under water
            if (IsAnyPointUnderWater())
                ApplyRotationToSurface();
        }

        private void UpdateWaterLinePoints()
        {
            float t_newWaterLine = 0f;

            for (int i = 0; i < FloatPoints.Length; i++)
            {
                Vector3 t_floatPoint = FloatPoints[i].position;
                WaterLinePoints[i].y = m_waterSurfaceController.GetHeight(t_floatPoint);
                t_newWaterLine += WaterLinePoints[i].y / FloatPoints.Length;
            }

            WaterLine = t_newWaterLine;
        }

        private void UpdateGravityAndDrag()
        {
            m_rigidbody.linearDamping = airDrag;
            Vector3 t_gravity = Physics.gravity;

            if (WaterLine > Center.y)
            {
                m_rigidbody.linearDamping = waterDrag;
                if (attachToSurface)
                {
                    m_rigidbody.position = new Vector3(m_rigidbody.position.x, WaterLine - centerOffset.y, m_rigidbody.position.z);
                }
                else
                {
                    t_gravity = affectDirection ? TargetUp * -Physics.gravity.y : -Physics.gravity;
                    transform.Translate((WaterLine - Center.y) * 0.9f * Vector3.up);
                }
            }

            m_rigidbody.AddForce(t_gravity * Mathf.Clamp(Mathf.Abs(WaterLine - Center.y), 0, 1));
        }

        private void ApplyRotationToSurface()
        {
            TargetUp = PhysicsHelper.GetNormal(WaterLinePoints);
            TargetUp = Vector3.SmoothDamp(transform.up, TargetUp, ref smoothVectorRotation, 0.2f);
            m_rigidbody.rotation = Quaternion.FromToRotation(transform.up, TargetUp) * m_rigidbody.rotation;
        }

        private bool IsAnyPointUnderWater()
        {
            for (int i = 0; i < WaterLinePoints.Length; i++)
            {
                if (WaterLinePoints[i].y > FloatPoints[i].position.y)
                {
                    return true;
                }
            }

            return false;
        }

        private void OnDrawGizmos()
        {
            if (FloatPoints == null || m_waterSurfaceController == null || !Application.isPlaying) return;

            Gizmos.color = Color.green;
            for (int i = 0; i < FloatPoints.Length; i++)
            {
                if (FloatPoints[i] == null)
                    continue;

                // Draw WaterLine Points as cubes
                Gizmos.color = Color.red;
                Gizmos.DrawCube(WaterLinePoints[i], Vector3.one * 0.3f);

                // Draw FloatPoints as spheres
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(FloatPoints[i].position, 0.1f);
            }

            // Draw center
            Gizmos.color = Color.red;
            Gizmos.DrawCube(new Vector3(Center.x, WaterLine, Center.z), Vector3.one * 1f);
            Gizmos.DrawRay(new Vector3(Center.x, WaterLine, Center.z), TargetUp * 1f);
        }
    }
}
