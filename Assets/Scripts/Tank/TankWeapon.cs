using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Complete {
    public class TankWeapon : MonoBehaviour {
        [Header("Tank")]
        public Transform m_FireTransform;

        [Header("Weapon")]
        public GameObject m_Shell;
        public float launchVelocity = 30f;
        public float recoilStrength = 0;
        public bool autoFire = true;
        public float fireRate = 0.3f;
        public ParticleSystem muzzleFX;

        [Header("Audio")]
        public AudioSource m_ShootingAudio;
        public AudioClip m_FireClip;
        public bool isLooping = false;

        private bool isTriggered = false;
        private Coroutine fireCoroutine;

        private void Start() {
            m_ShootingAudio.loop = isLooping;
            m_ShootingAudio.clip = m_FireClip;
        }

        public void fire() {
            GameObject shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as GameObject;
            shellInstance.GetComponent<Rigidbody>().velocity = launchVelocity * m_FireTransform.forward;
            shellInstance.GetComponent<ShellExplosion>().owner = gameObject;

            if (gameObject.GetComponent<Rigidbody>()) {
                gameObject.GetComponent<Rigidbody>().AddForce(-recoilStrength * m_FireTransform.forward);
            }

            muzzleFX.Play();

            if (!isLooping) {
                m_ShootingAudio.Play();
            }
        }
        public void triggerOn() {
            if (autoFire) {
                isTriggered = true;
                fireCoroutine = StartCoroutine(fireLoop());
            } else {
                fire();
            }
            if (isLooping) {
                m_ShootingAudio.Play();
            }
        }
        public void triggerOff() {
            isTriggered = false;
            if (fireCoroutine != null) {
                StopCoroutine(fireCoroutine);
            }
            if (isLooping) {
                m_ShootingAudio.Stop();
            }
        }
        public IEnumerator fireLoop() {
            while (isTriggered) {
                fire();
                yield return new WaitForSeconds(fireRate);
            }
        }
    }
}