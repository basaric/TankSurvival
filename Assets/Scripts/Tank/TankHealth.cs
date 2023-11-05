﻿using UnityEngine;
using UnityEngine.UI;
using System;

namespace Complete {
    public class TankHealth : MonoBehaviour {
        public float startHealth = 100f;
        public GameObject explosionPrefab;
        public Vector3 positionOffset = new Vector3(0, 50, 0);
        public Slider slider;
        public bool destroyOnKill = true;
        public bool showSlider = false;

        public event Action<TankHealth> onKilled; 

        private float health;
        private Camera mainCamera;
        
        private void Awake() {
            slider.gameObject.transform.parent.gameObject.SetActive(showSlider);
        }
        private void OnEnable() {
            mainCamera = Camera.main;
            health = startHealth;
            refreshGUI();
        }
        private void FixedUpdate() {
            Vector3 newPosition = mainCamera.WorldToScreenPoint(gameObject.transform.position) + positionOffset;
            slider.transform.position = newPosition;
        }
        public void TakeDamage(float amount) {
            health -= amount;
            refreshGUI();
            if (health <= 0f) {
                kill();
            }
        }
        private void refreshGUI() {
            slider.value = health / startHealth * slider.maxValue;
        }
        public void kill() {
            ParticleSystem explosion = Instantiate(explosionPrefab).GetComponent<ParticleSystem>();
            explosion.transform.position = transform.position;
            explosion.Play();
            explosion.GetComponent<AudioSource>().Play();
            Destroy(explosion,  3f);

            onKilled?.Invoke(this);

            if (destroyOnKill) {
                Destroy(gameObject);
            } else {
                gameObject.SetActive(false);
            }
        }
    }
}