using System;
using System.Windows.Forms;
using NXOpen;
using NXOpenUI;
using NXOpen.Utilities;
using NXOpen.UF;

namespace MainProject.Utils
{
    class SelectUtils
    {

        static class select_components_with_filter_callback
        {

            static Session s = Session.GetSession();
            static UFSession ufs = UFSession.GetUFSession();
            static Tag otherThan = Tag.Null;

            //  These need to be global variables - see PR 6592293
            static UFUi.SelInitFnT ip = init_proc;
            static UFUi.SelFilterFnT fp = filter_proc;

            public static void Main()
            {
                NXOpen.Assemblies.Component selComp = null;

                do
                {
                    selComp = select_a_component("Select component");
                    if (selComp != null) otherThan = selComp.Tag;
                } while (selComp != null);
            }

            public static NXOpen.Assemblies.Component select_a_component(String Prompt)
            {
                int response = 0;
                NXOpen.Tag obj = default(NXOpen.Tag);
                NXOpen.Tag view = default(NXOpen.Tag);
                double[] cursor = new double[3];

                ufs.Ui.LockUgAccess(UFConstants.UF_UI_FROM_CUSTOM);

                try
                {
                    ufs.Ui.SelectWithSingleDialog(Prompt, Prompt,
                        UFConstants.UF_UI_SEL_SCOPE_ANY_IN_ASSEMBLY, ip,
                        IntPtr.Zero, out response, out obj, cursor, out view);
                }
                finally
                {
                    ufs.Ui.UnlockUgAccess(UFConstants.UF_UI_FROM_CUSTOM);
                }

                if ((response != UFConstants.UF_UI_OBJECT_SELECTED) &
                    (response != UFConstants.UF_UI_OBJECT_SELECTED_BY_NAME))
                {
                    return null;
                }
                else
                {
                    ufs.Disp.SetHighlight(obj, 0);
                    return (NXOpen.Assemblies.Component)
                        NXOpen.Utilities.NXObjectManager.Get(obj);
                }
            }

            public static int filter_proc(Tag obj, int[] type, IntPtr user_data,
                IntPtr select_)
            {
                if (obj != otherThan)
                    return UFConstants.UF_UI_SEL_ACCEPT;
                else
                    return UFConstants.UF_UI_SEL_REJECT;
            }

            public static int init_proc(IntPtr select_, IntPtr userdata)
            {
                UFUi.Mask[] mask_triples = new UFUi.Mask[1];
                mask_triples[0].object_type = UFConstants.UF_component_type;
                mask_triples[0].object_subtype = 0;
                mask_triples[0].solid_type = 0;

                ufs.Ui.SetSelMask(select_,
                    UFUi.SelMaskAction.SelMaskClearAndEnableSpecific, 1, mask_triples);
                ufs.Ui.SetSelProcs(select_, fp, null, userdata);

                return UFConstants.UF_UI_SEL_SUCCESS;
            }

            public static int GetUnloadOption(string dummy)
            {
                return UFConstants.UF_UNLOAD_IMMEDIATELY;
            }
        }
    }
}
