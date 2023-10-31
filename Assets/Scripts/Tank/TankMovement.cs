﻿using Unity.Burst.CompilerServices;
using Unity.Services.Analytics.Internal;
using UnityEngine;

namespace Complete {
    public class TankMovement : MonoBehaviour {
        [Header("Movement")]
        public float maxVelocity = 5f;
        public float torqueStrength = 5f;
        public float inputMaxMagnitude = 1f;
        public float movementStrength = 50f;
        public AudioClip m_EngineIdling;
        public AudioClip m_EngineDriving;

        [Header("References")]
        public AudioSource m_MovementAudio;
        public Transform turretTransform;

        private Rigidbody rigidBody;
        private float enginePitch;
        private float enginePitchRange = 0.2f;

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
            rigidBody.AddForce(Vector3.ClampMagnitude(input, inputMaxMagnitude) * movementStrength);
        }
        public void aimAt(Vector3 point) {
            Vector3 aimDirection = (point - transform.position).normalized;
            Quaternion newRotation = Quaternion.LookRotation(aimDirection, Vector3.up);
            newRotation.x = 0;
            newRotation.z = 0;
            turretTransform.transform.rotation = newRotation;
        }
        public void orientToMovement() {
            Vector3 XZVelocity = rigidBody.velocity;
            XZVelocity.y = 0;
            if (XZVelocity.magnitude > 0.05f) {
                Quaternion newRotation = Quaternion.LookRotation(XZVelocity.normalized, Vector3.up);
                newRotation.x = 0;
                newRotation.z = 0;
                rigidBody.AddTorque(Vector3.Cross(transform.forward, XZVelocity.normalized) * torqueStrength);
            }
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