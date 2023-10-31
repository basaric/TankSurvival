using UnityEngine;

namespace Complete {
    public class TankWeapon : MonoBehaviour {
        [Header("Tank")]
        public Transform m_FireTransform;
        public AudioSource m_ShootingAudio;

        [Header("Weapon")]
        public GameObject m_Shell;
        public AudioClip m_FireClip;
        public float m_MaxLaunchForce = 30f;
        public float recoilStrength = 200f;

        public void Fire() {
            GameObject shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as GameObject;
            shellInstance.GetComponent<Rigidbody>().velocity = m_MaxLaunchForce * m_FireTransform.forward;
            shellInstance.GetComponent<ShellExplosion>().owner = gameObject;
            gameObject.GetComponent<Rigidbody>().AddForce(-recoilStrength * m_FireTransform.forward);

            m_ShootingAudio.clip = m_FireClip;
            m_ShootingAudio.Play();
        }
    }
}