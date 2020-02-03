using UnityEngine;

namespace FirstPersonView.ShaderMaterialSolution
{
    [AddComponentMenu("FPV/Shader-Material Setup/FPV Renderer")]
    public class FPV_SM_Renderer : FPV_Renderer
    {
        public FPV_SM_Material fpvMaterial;

        /// <summary>
        /// Setup the renderer
        /// </summary>
        /// <param name="render"></param>
        public override void Setup(IFPV_Object parent, Renderer render)
        {
            base.Setup(parent, render);

            InitialteFPVMaterials();
        }

        /// <summary>
        /// Initiate the Material of this renderer
        /// </summary>
        private void InitialteFPVMaterials()
        {
            if (fpvMaterial == null) fpvMaterial = new FPV_SM_Material();

            fpvMaterial.OriginalMaterial = _render.sharedMaterial; //Store the original material

            if(fpvMaterial.FirstPersonViewMaterial == null) //Instance a new material of the original used if none is assigned to the firstpersonmaterial
            {
                fpvMaterial.FirstPersonViewMaterial = _render.material; //Create instance.

                //Reset the original material
                _render.sharedMaterial = fpvMaterial.OriginalMaterial;
            }

            //Enable the Keyword FIRSTPERSONVIEW for this material
            fpvMaterial.FirstPersonViewMaterial.EnableKeyword("FIRSTPERSONVIEW");
        }

        /// <summary>
        /// Set this renderer as First Person Object
        /// </summary>
        public override void SetAsFirstPersonObject()
        {
            _render.sharedMaterial = fpvMaterial.FirstPersonViewMaterial;
        }

        /// <summary>
        /// Set this renderer as First Person Object
        /// </summary>
        public override void RemoveAsFirstPersonObject()
        {
            _render.sharedMaterial = fpvMaterial.OriginalMaterial;
        }
    }
}
