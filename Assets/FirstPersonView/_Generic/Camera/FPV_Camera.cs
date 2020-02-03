using UnityEngine;

namespace FirstPersonView
{
    /// <summary>
    /// Abstract class to the Camera component of FPV.
    /// </summary>
    public abstract class FPV_Camera : MonoBehaviour, IFPV_Camera
    {
        /// <summary>
        /// Reference to the camera component of this gameobject
        /// </summary>
        protected Camera _camera;

        void Awake()
        {
            Setup();
        }

        /// <summary>
        /// Setup every needed variable on this component
        /// </summary>
        public virtual void Setup()
        {
            _camera = GetComponent<Camera>();
            UpdateStaticCamera();
        }

        /// <summary>
        /// Manualy update the static camera variable of this component.
        /// </summary>
        public abstract void UpdateStaticCamera();

        /// <summary>
        /// Get the camera component of this camera.
        /// </summary>
        /// <returns></returns>
        public Camera GetCamera()
        {
            return _camera;
        }

        /// <summary>
        /// Get the current projection matrix of this camera.
        /// </summary>
        /// <returns></returns>
        public Matrix4x4 GetProjectionMatrix()
        {
            return _camera.projectionMatrix;
        }
    }
}
