using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

namespace LUI
{
    public class FootprintMinigame : UI
    {
        private Controls _controls;

        public GameObject controlledImage;
        public GameObject refrenceObject;

        public TextMeshProUGUI similarityText;

        [SerializeField] private float scrollRateMultiplier;

        private Animator _animator;

        private float scrollForce;
        public float smoothRate = 10f;

        void Start() => Init();

        public void Init()
        {
            _animator = GetComponent<Animator>();

            _controls = new Controls();
            _controls.UI.Enable();

            _controls.UI.ScrollWheel.performed += scrolled;
            _controls.UI.Click.performed += clicked;

            controlledImage.transform.rotation = Quaternion.Euler(0, 0, Random.Range(20, 340));
            refrenceObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(20, 340));
        }

        private void clicked(InputAction.CallbackContext obj)
        {
            _controls.UI.ScrollWheel.performed -= scrolled;
            similarityText.gameObject.SetActive(true);
            _animator.Play("UI_footprint_leave");
        }

        private void Update()
        {
            scrollForce = Mathf.Lerp(scrollForce, 0, Time.deltaTime * smoothRate);
            controlledImage.transform.rotation *= Quaternion.Euler(0, 0, scrollForce);

            float similarity = controlledImage.transform.rotation.eulerAngles.z / refrenceObject.transform.rotation.eulerAngles.z;

            similarityText.text = (similarity * 100).ToString("0") + "%";
        }

        private void scrolled(InputAction.CallbackContext context)
        {
            Vector2 value = context.ReadValue<Vector2>();

            if (value.y == 0) return;

            scrollForce += (-value.y * scrollRateMultiplier) / 100;
        }
    }
}