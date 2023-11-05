using System.Collections;
using UnityEditor.XR.LegacyInputHelpers;
using UnityEngine;

namespace Complete {
    public class CameraControl : MonoBehaviour {
        public float m_DampTime = 0.2f;                 
        public float m_ScreenEdgeBuffer = 4f;           
        public float m_MinSize = 6.5f;

        public float horizontalSensitivity = 0.1f;
        public float verticalSensitivity = -0.1f;
        public float minX = 35f;
        public float maxX = 85f;

        public float distanceThreshold = 25f;
        public float distanceMultiplier = 0.5f;

        private Camera m_Camera;                        
        private GameObject player;
        private Vector3 m_MoveVelocity;
        private Vector3 offset = Vector3.zero;

        private void Awake() {
            m_Camera = GetComponentInChildren<Camera>();
        }
        void Start() {
            player = GameObject.FindWithTag("Player");
            transform.position = player.transform.position;
        }
        private void Update() {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            if (Input.GetMouseButton(1)) {
                Vector3 _rotation = m_Camera.transform.parent.localEulerAngles;
                _rotation.x += mouseY * verticalSensitivity * Time.deltaTime;
                _rotation.x = Mathf.Clamp(_rotation.x, minX, maxX);
                m_Camera.transform.parent.localEulerAngles = _rotation;
                transform.Rotate(0f, mouseX * horizontalSensitivity * Time.deltaTime, 0f);
            }

            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, float.MaxValue, LayerMask.GetMask("Ground"))) {
                offset = player.transform.position - hit.point;
                offset.y = 0;
                offset = Vector3.ClampMagnitude(offset, distanceThreshold);
                offset = offset * distanceMultiplier;

            }
        }
        private void FixedUpdate() {
            if (player != null) {
                transform.position = Vector3.SmoothDamp(transform.position, player.transform.position - offset, ref m_MoveVelocity, m_DampTime);
            }
        }
        private Vector3 FindAveragePosition(Transform[] m_Targets) {
            Vector3 averagePos = new Vector3();
            int numTargets = 0;
            for (int i = 0; i < m_Targets.Length; i++) {
                if (!m_Targets[i].gameObject.activeSelf)
                    continue;
                averagePos += m_Targets[i].position;
                numTargets++;
            }
            if (numTargets > 0)
                averagePos /= numTargets;
            averagePos.y = transform.position.y;
            return averagePos;
        }
    }
}