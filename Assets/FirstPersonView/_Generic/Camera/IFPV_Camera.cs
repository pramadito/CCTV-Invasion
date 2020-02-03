using UnityEngine;

namespace FirstPersonView
{
    /// <summary>
    /// Interface for all Camera components of FirstPersonView
    /// </summary>
    public interface IFPV_Camera
    {
        /// <summary>
        /// Setup every needed variable on this component
        /// </summary>
        void Setup();

        /// <summary>
        /// Manualy update the static camera variable of this component.
        /// </summary>
        void UpdateStaticCamera();

        /// <summary>
        /// Get the camera component of this camera.
        /// </summary>
        /// <returns></returns>
        Camera GetCamera();

        /// <summary>
        /// Get the projection matrix of this camera
        /// </summary>
        /// <returns></returns>
        Matrix4x4 GetProjectionMatrix();
    }
}
