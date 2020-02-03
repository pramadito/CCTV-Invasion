using UnityEngine;

namespace FirstPersonView.ShaderMaterialSolution
{
    /// <summary>
    /// Component for the First Person Camera in this FPV Shader-Material Setup
    /// </summary>
    [AddComponentMenu("FPV/Shader-Material Setup/FPV Camera First Person")]
    public class FPV_SM_FirstPersonCamera : FPV_Camera
    {
        /// <summary>
        /// Manualy update the static first person view camera variable.
        /// </summary>
        public override void UpdateStaticCamera()
        {
            FPV.firstPersonCamera = this;
        }
    }
}