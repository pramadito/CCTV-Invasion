using UnityEngine;

namespace FirstPersonView
{
    public class FPV_Shader_Example_Player : MonoBehaviour
    {
        public Transform weaponPlacement;

        public GameObject weaponPrefab;

        private FPV_Shader_Example_Weapon weapon;

        [Header("Cameras")]
        public Camera worldCamera;
        public Camera fpvCamera;

        void Start()
        {
            CreateWeapon();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                SwitchWeapon();
            }

            Aim();

            ChangeFOV();

            Fire();
        }

        private void Aim()
        {
            if (weapon == null) return;

            bool aiming = Input.GetMouseButton(1);

            weapon.Aim(aiming, fpvCamera.transform);

            float futureFoV = aiming ? 20 : 70;

            fpvCamera.fieldOfView = Mathf.Lerp(fpvCamera.fieldOfView, futureFoV, Time.deltaTime * 10);
        }

        private void ChangeFOV()
        {
            //World Camera FOV
            float fOVChange = 0;
            if (Input.GetKey(KeyCode.Comma))
            {
                fOVChange = -Time.deltaTime * 10;
            }
            else if (Input.GetKey(KeyCode.Period))
            {
                fOVChange = Time.deltaTime * 10;
            }
            worldCamera.fieldOfView = Mathf.Clamp(worldCamera.fieldOfView + fOVChange, 50, 120);

            //First Person Camera FOV
            fOVChange = 0;
            if (Input.GetKey(KeyCode.N))
            {
                fOVChange = -Time.deltaTime * 10;
            }
            else if (Input.GetKey(KeyCode.M))
            {
                fOVChange = Time.deltaTime * 10;
            }
            fpvCamera.fieldOfView = Mathf.Clamp(fpvCamera.fieldOfView + fOVChange, 4, 70);
        }

        private void CreateWeapon()
        {
            if (weapon != null) return;

            GameObject newWeapon = GameObject.Instantiate(weaponPrefab);

            weapon = newWeapon.GetComponent<FPV_Shader_Example_Weapon>();
            weapon.Setup();
            SetWeaponOnPlayer();
        }

        private void Fire()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (weapon != null)
                {
                    weapon.Fire();
                }
            }
        }

        private void SwitchWeapon()
        {
            if (weapon.IsOnPlayer())
            {
                SetWeaponOnWorld();
            }
            else
            {
                SetWeaponOnPlayer();
            }
        }

        private void SetWeaponOnPlayer()
        {
            weapon.SetWeaponOnPlayer(weaponPlacement);
        }

        private void SetWeaponOnWorld()
        {
            weapon.SetWeaponOnWorld();
        }
    }
}