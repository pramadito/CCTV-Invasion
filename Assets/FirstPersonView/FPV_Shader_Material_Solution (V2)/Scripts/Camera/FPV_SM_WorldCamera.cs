using UnityEngine;
using UnityEngine.Rendering;

namespace FirstPersonView.ShaderMaterialSolution
{
    /// <summary>
    /// Component for the World Camera in this FPV Shader-Material Setup
    /// </summary>
    [AddComponentMenu("FPV/Shader-Material Setup/FPV Camera World")]
    public class FPV_SM_WorldCamera : FPV_Camera
    {
        public bool castCorrectFpvShadows;

        private bool d3d = true;
        private bool usesReversedZBuffer = false;

        private CommandBuffer _commandBufferBefore;
        private CommandBuffer _commandBufferAfter;

        private CommandBuffer _commandBufferMotionVectorBefore;
        private CommandBuffer _commandBufferMotionVectorAfter;

        public override void Setup() {
            base.Setup();

            usesReversedZBuffer = SystemInfo.usesReversedZBuffer;

			d3d = SystemInfo.graphicsDeviceVersion.IndexOf("Direct3D") > -1;

            if (castCorrectFpvShadows) {
                PrepareLightingCommandBuffer();
            }

            PrepareMotionVectorCommandBuffer();
        }
        /// <summary>
        /// Manualy update the static first person view camera variable.
        /// </summary>
        public override void UpdateStaticCamera() {
            FPV.mainCamera = this;
        }

        void OnPreCull() {
            //Before rendering the scene, update the global first person projection on the shaders.
            Matrix4x4 fpvProjection = FPV.firstPersonCamera.GetProjectionMatrix();

            Matrix4x4 fpvProjectionCustom = fpvProjection;

            //DIRECTX
            if (d3d) {
                fpvProjection.m11 *= -1; // Invert Y value of the MVP vertex position
                fpvProjectionCustom.m11 *= -1; // Invert Y value of the MVP vertex position

                if (usesReversedZBuffer) { //Inverted Z-Buffer
                    fpvProjection.m22 *= 0.3f; // Shorten the Z value of the MVP vertex position
                    fpvProjection.m23 *= -1;
                    fpvProjectionCustom.m23 *= -1;
                }
                else { //Z-Buffer not inverted
                    fpvProjection.m22 *= 0.7f; // Shorten the Z value of the MVP vertex position
                }
            }
            //OPEN GL
            else {
                if (usesReversedZBuffer) { //Inverted Z-Buffer
                    fpvProjection.m22 *= 0.3f; // Shorten the Z value of the MVP vertex position
                    fpvProjection.m23 *= -1;
                    fpvProjectionCustom.m23 *= -1;
                }
                else { //Z-Buffer not inverted
                    fpvProjection.m22 = fpvProjection.m22 * 0.7f + 0.3f; // Shorten the Z value of the MVP vertex position
                }
            }
            
            //Update the global shader variable
            Shader.SetGlobalMatrix("_firstPersonProjectionMatrix", fpvProjection);
            Shader.SetGlobalMatrix("_firstPersonProjectionMatrixCustom", fpvProjection);
        }

        private void PrepareLightingCommandBuffer() {
            _commandBufferBefore = new CommandBuffer();
            _commandBufferAfter = new CommandBuffer();

            _commandBufferBefore.EnableShaderKeyword("FPV_LIGHT");
            _commandBufferAfter.DisableShaderKeyword("FPV_LIGHT");

            _camera.AddCommandBuffer(CameraEvent.BeforeLighting, _commandBufferBefore);
            _camera.AddCommandBuffer(CameraEvent.AfterLighting, _commandBufferAfter);
        }

        private void PrepareMotionVectorCommandBuffer() {
            _commandBufferMotionVectorBefore = new CommandBuffer();
            _commandBufferMotionVectorAfter = new CommandBuffer();

            _commandBufferMotionVectorBefore.EnableShaderKeyword("FPV_MOTION_VECTOR");
            _commandBufferMotionVectorAfter.DisableShaderKeyword("FPV_MOTION_VECTOR");

            _camera.AddCommandBuffer(CameraEvent.AfterSkybox, _commandBufferMotionVectorBefore);
            _camera.AddCommandBuffer(CameraEvent.BeforeForwardAlpha, _commandBufferMotionVectorAfter);
        }
    }
}