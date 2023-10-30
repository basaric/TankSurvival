using Unity.Burst.CompilerServices;
using Unity.Services.Analytics.Internal;
using UnityEngine;

namespace Complete {
    public class TankMovement : MonoBehaviour {
        public float maxVelocity = 5f;
        public float inputScaledMaxMagnitude = 1f;
        public float movementStrength = 50f;
        public float aimLerpStrength = 1f;
        public float enginePitchRange = 0.2f;

        public AudioSource m_MovementAudio;
        public AudioClip m_EngineIdling;
        public AudioClip m_EngineDriving;
        public Transform turretTransform;

        private Rigidbody rigidBody;
        private float enginePitch;

        private void Awake() {
            rigidBody = GetComponent<Rigidbody>();
        }
        private void Start() {
            enginePitch = m_MovementAudio.pitch;
        }
        private void Update() {
            EngineAudio();
        }
        private void FixedUpdate() {
            rigidBody.velocity = Vector3.ClampMagnitude(rigidBody.velocity, maxVelocity);
            orientToMovement();
        }
        public void onMoveInput(Vector3 input) {
            rigidBody.AddForce(input.normalized * movementStrength);
        }
        public void onMoveInputScaled(Vector3 input) {
            rigidBody.AddForce(Vector3.ClampMagnitude(input, inputScaledMaxMagnitude) * movementStrength);
        }
        public void aimAt(Vector3 point) {
            Vector3 aimDirection = (point - transform.position).normalized;
            Quaternion newRotation = Quaternion.LookRotation(aimDirection, Vector3.up);
            newRotation.x = 0;
            newRotation.z = 0;
            newRotation = Quaternion.Lerp(turretTransform.transform.rotation, newRotation, aimLerpStrength);
            turretTransform.transform.rotation = newRotation;
        }
        public void orientToMovement() {
            Vector3 XZVelocity = rigidBody.velocity;
            XZVelocity.y = 0;
            float previousRotationY = transform.rotation.y;
            if (XZVelocity.magnitude > 0.05f) {
                Quaternion newRotation = Quaternion.LookRotation(XZVelocity.normalized, Vector3.up);
                newRotation.x = 0;
                newRotation.z = 0;
                float _lerpSpeed = 0.3f + (Vector3.Dot(transform.forward, XZVelocity.normalized) - 1f) * 0.5f * 0.2f;
                //transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, _lerpSpeed);
            }

            /*
            float rotationOffset = transform.rotation.y - previousRotationY;
            Quaternion newTurretRotation = turretTransform.rotation;
            newTurretRotation.y -= rotationOffset;
            turretTransform.rotation = newTurretRotation;
            */
        }
        private void EngineAudio() {
            if (rigidBody.velocity.magnitude < 0.1f) {
                if (m_MovementAudio.clip == m_EngineDriving) {
                    m_MovementAudio.clip = m_EngineIdling;
                    m_MovementAudio.pitch = Random.Range(enginePitch - enginePitchRange, enginePitch + enginePitchRange);
                    m_MovementAudio.Play();
                }
            } else if (m_MovementAudio.clip == m_EngineIdling) {
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(enginePitch - enginePitchRange, enginePitch + enginePitchRange);
                m_MovementAudio.Play();
            }
        }
    }
}