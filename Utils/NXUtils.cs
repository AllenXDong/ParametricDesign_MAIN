
using System;
using NXOpen;
using NXOpen.UF;
using NXOpen.BlockStyler;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using MainProject.Bean;
using NXOpen.Assemblies;
using NXOpen.Drawings;
//------------------------------------------------------------------------------
//Represents Block Styler application class
//------------------------------------------------------------------------------
namespace MainProject.Utils
{
    public class NXUtils

    {
        public List<expression_struct> exprevector = null;
        public List<expression_struct> typevector = null;
        public Dictionary<string, List<string>> dic1;
        public Dictionary<string, List<string>> dic2;
        public string dllPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        public static void AssemClone(string target_folder, string template_path)
        {
            //模板路径

            //string strTemplPath = env_dir + "\\HL_TEMPLATE\\1EP\\3d\\" + templatename + ".prt";
            UFSession theUfSession = UFSession.GetUFSession();
            UI theUI = UI.GetUI();
            try
            {
                //int error;
                //初始化
                UFClone.OperationClass operation_class = UFClone.OperationClass.CloneOperation;
                //UF_CLONE_operation_class_t operation_class = UF_CLONE_clone_operation;
                theUfSession.Clone.Initialise(operation_class);

                //添加原装配到克隆装配中，有Add Assembly和Add Part两种方法，此处用Add Assembly
                UFPart.LoadStatus error_status;
                theUfSession.Clone.AddAssembly(template_path, out error_status);

                //指定默认的克隆方式
                UFClone.Action action = UFClone.Action.Clone;
                theUfSession.Clone.SetDefAction(action);

                //指定默认文件名的方法
                UFClone.NamingTechnique naming_technique = UFClone.NamingTechnique.NamingRule;
                //UF_CLONE_naming_technique_t naming_technique = UF_CLONE_naming_rule;
                theUfSession.Clone.SetDefNaming(naming_technique);

                //定义新装配的克隆命名规则
                UFClone.NameRuleDef name_rule;
                UFClone.NamingFailures naming_failures;

                name_rule.type = UFClone.NameRuleType.AppendString;
                name_rule.new_string = " ";
                name_rule.base_string = "";
                //name_rule.type = UFClone.NameRuleType.ReplaceString;
                //name_rule.new_string = replace_name;
                //name_rule.base_string = base_name;
                theUfSession.Clone.InitNamingFailures(out naming_failures);
                theUfSession.Clone.SetNameRule(ref name_rule, ref naming_failures);


                //创建或定义克隆部件的存储目录
                theUfSession.Clone.SetDefDirectory(target_folder);

                //执行克隆操作
                theUfSession.Clone.PerformClone(ref naming_failures);

            }
            catch (NXException ex)
            {
                theUI.NXMessageBox.Show("Setting", NXMessageBox.DialogType.Error, ex.Message);
            }
            finally
            {
                theUfSession.Clone.Terminate();
            }
            //string strClonePath = strCloneDir + "\\" + strClonePath;
        }

        public static int readpartdata(Part workPart,string name, String value)
        {
            int errorCode = 0;
            string extendata;
            //string extendstring;
            if (!workPart.HasUserAttribute(name, NXObject.AttributeType.Any, -1))
            {
                errorCode = 1;
                return errorCode;
            }
            extendata = workPart.GetUserAttributeAsString(name, NXObject.AttributeType.Any, -1);
            //extendstring = extendata.GetLocaleText();
            if (extendata == "") errorCode = 1;
            else if (extendata != value) errorCode = 2;


            return errorCode;
        }


        public static string select_file()
        {
            string file = "";
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件位置";
            dialog.Filter = "所有文件(*.*)|*.prt*";
            dialog.CheckFileExists = false;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                file = dialog.FileName;
            }
            return file;
        }

        public static string select_folder()
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择Txt所在文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show("文件夹路径不能为空", "提示");
                    return "";
                }else
                {
                    return dialog.SelectedPath;
                }
            }
            return "";
        }

        public static void addcomponent(string pathpath)
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Point3d basePoint1 = new Point3d(0.0, 0.0, 0.0);
            Matrix3x3 orientation1;
            orientation1.Xx = 1.0;
            orientation1.Xy = 0.0;
            orientation1.Xz = 0.0;
            orientation1.Yx = 0.0;
            orientation1.Yy = 1.0;
            orientation1.Yz = 0.0;
            orientation1.Zx = 0.0;
            orientation1.Zy = 0.0;
            orientation1.Zz = 1.0;
            PartLoadStatus partLoadStatus1;
            NXOpen.Assemblies.Component component1;
            component1 = workPart.ComponentAssembly.AddComponent(pathpath, "MODEL", "_MODEL2", basePoint1, orientation1, -1, out partLoadStatus1, true);
        }

        
        public static Dictionary<string, List<string>> OpenCSV(string filePath)
        {
            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            //Encoding encoding = Common.GetType(filePath); //Encoding.ASCII;//
            System.Data.DataTable dt = new System.Data.DataTable();
            FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.Default);
            //StreamReader sr = new StreamReader(fs, encoding);
            //string fileContent = sr.ReadToEnd();
            //encoding = sr.CurrentEncoding;
            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine = null;
            string[] tableHead = null;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;
            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                //strLine = Common.ConvertStringUTF8(strLine, encoding);
                //strLine = Common.ConvertStringUTF8(strLine);

                if (IsFirst == true)
                {
                    tableHead = strLine.Split(',');
                    IsFirst = false;
                    columnCount = tableHead.Length;
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dc = new DataColumn(tableHead[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    aryLine = strLine.Split(',');
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j];
                    }
                    dt.Rows.Add(dr);
                }
            }
            if (aryLine != null && aryLine.Length > 0)
            {
                dt.DefaultView.Sort = tableHead[0] + " " + "asc";
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                List<string> valuelist = new List<string>();
                //Range cel;
                //double valuedouble;
                string rowfirst;
                string cellvalue;
                rowfirst = dt.Rows[i][0].ToString();
                if (rowfirst == "")
                {
                    break;
                }
                for (int j = 1; j < dt.Columns.Count; j++)
                {
                    cellvalue = dt.Rows[i][j].ToString();
                    if (cellvalue == "")
                    {
                        break;
                    }
                    //valuedouble = cel.Value;
                    valuelist.Add(cellvalue);
                }
                dic.Add(rowfirst, valuelist);
            }
            sr.Close();
            fs.Close();
            return dic;
        }

        public static int getexpvalue(string expname, out string expvalue)
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            int errorCode = 0;
            expvalue = "0";
            Expression expression1;
            try
            {
                expression1 = (Expression)workPart.Expressions.FindObject(expname);
                if (expression1 != null)
                {
                    expvalue = expression1.RightHandSide;
                }
            }
            catch (NXException ex)
            {
                if (ex.ErrorCode == 3520016)
                {
                    errorCode = 1;
                    //BlockStyler17110_2A::theUI->NXMessageBox()->Show("Block Styler", NXOpen::NXMessageBox::DialogTypeError, "找不到表达式：" + expname);
                }

            }
            return errorCode;
        }
        public static int replaceexpression(List<expression_struct> expvector)
        {
            int errorCode = 0;
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            UI theUI = UI.GetUI();
            Expression expression1;
            for (int n = 0; n < expvector.Count; n++)
            {
                try
                {
                    expression1 = (Expression)(workPart.Expressions.FindObject(expvector[n].name));
                    if (expression1 != null && expvector[n].blocks != null)
                    {
                        if (!expression1.RightHandSide.Equals(expvector[n].value))
                        {
                            workPart.Expressions.Edit(expression1, expvector[n].value);

                        }
                    }
                }
                catch (NXException ex)
                {
                    errorCode = 1;
                    if (ex.ErrorCode == 3520016)
                    {
                        //theUI.NXMessageBox.Show("Block Styler",NXMessageBox.DialogType.Error, "找不到表达式：" + expvector[n].name);
                        //return errorCode;
                        //continue;
                    }
                    else
                    {
                        theUI.NXMessageBox.Show("Block Styler", NXOpen.NXMessageBox.DialogType.Error, ex.Message);
                    }
                }
            }
            return errorCode;
        }

        public static int updateExpValue(ParameterBean parameterBean, string value)
        {
            int errorCode = 0;
            Session theSession = Session.GetSession();
            Part workPart = Project.theSession.Parts.Work;
            UI theUI = UI.GetUI();
            Expression expression1;
            try
            {
                expression1 = (Expression)(workPart.Expressions.FindObject(parameterBean.ExpressionName));
                if (expression1 != null && !"".Equals(value))
                {
                    if (!expression1.RightHandSide.Equals(value))
                    {
                        workPart.Expressions.Edit(expression1, value);

                    }
                }
            }
            catch (NXException ex)
            {
                errorCode = 1;
                if (ex.ErrorCode == 3520016)
                {
                    theUI.NXMessageBox.Show("Block Styler", NXOpen.NXMessageBox.DialogType.Error, parameterBean.DisplayName + "对应的表达式:" + parameterBean.ExpressionName + "不存在，请检查！");
                }
                else
                {
                    theUI.NXMessageBox.Show("Block Styler", NXOpen.NXMessageBox.DialogType.Error, parameterBean.DisplayName+"对应的表达式:"+parameterBean.ExpressionName+",对应的值为:"+ value +","+ ex.Message);
                }
            }
            return errorCode;
        }

        /*
         * * 根据名称激活草图
         */
        public static void activesketch(string sketchname)
        {
             NXOpen.Session theSession = Project.theSession;//.GetSession();
             NXOpen.Part workPart = theSession.Parts.Work;
             NXOpen.Part displayPart = theSession.Parts.Display;
             NXOpen.Session.UndoMarkId markId1;
             markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "编辑草图");

             theSession.BeginTaskEnvironment();

             NXOpen.Session.UndoMarkId markId2;
             markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Enter Sketch");

            try
            { 
             NXOpen.Sketch sketch1 = (Sketch)workPart.Sketches.FindObject(sketchname);
             NXOpen.Features.SketchFeature sketchFeature1 = (NXOpen.Features.SketchFeature)sketch1.Feature;
             sketch1.Activate(NXOpen.Sketch.ViewReorient.True);
            }
            catch(NXException ex)
            {
                if (ex.ErrorCode == 3520016)
                    UI.GetUI().NXMessageBox.Show("Error", NXMessageBox.DialogType.Error, "未找到该草图");
            }


            theSession.DeleteUndoMarksUpToMark(markId2, null, true);

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Open Sketch");
            // ---------------------------------------------
            
        }

        public static void reAttachReference(string sketchname)
        {
            NXOpen.Session theSession = Project.theSession;//.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "编辑草图");

            theSession.BeginTaskEnvironment();

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Enter Sketch");

            try
            {
                NXOpen.Sketch sketch1 = (Sketch)workPart.Sketches.FindObject(sketchname);
                NXOpen.Features.SketchFeature sketchFeature1 = (NXOpen.Features.SketchFeature)sketch1.Feature;
                sketch1.Activate(NXOpen.Sketch.ViewReorient.True);
            }
            catch (NXException ex)
            {
                if (ex.ErrorCode == 3520016)
                    UI.GetUI().NXMessageBox.Show("Error", NXMessageBox.DialogType.Error, "未找到该草图");
            }


            theSession.DeleteUndoMarksUpToMark(markId2, null, true);

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Open Sketch");
            // ---------------------------------------------

        }

        public static void editPointFeature(int featureIndex, TaggedObject taggedObject)
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;

            NXOpen.Features.WavePoint wavePoint1 = ((NXOpen.Features.WavePoint)workPart.Features.ToArray()[featureIndex]); //("LINKED_POINT(2)")) ; ;// ;
            //NXOpen.Features.EditWithRollbackManager editWithRollbackManager1;
            //editWithRollbackManager1 = 
            workPart.Features.SetEditWithRollbackFeature(wavePoint1);


            NXOpen.Features.WavePointBuilder wavePointBuilder1;
            wavePointBuilder1 = workPart.Features.CreateWavePointBuilder(wavePoint1);

            wavePointBuilder1.ParentPart = NXOpen.Features.WavePointBuilder.ParentPartType.OtherPart;


            NXOpen.Point point1 = ((NXOpen.Point)taggedObject);
            bool added1;
            added1 = wavePointBuilder1.Points.Add(point1);

            wavePointBuilder1.Associative = true;

            wavePointBuilder1.Points.Clear();

            bool added2;
            added2 = wavePointBuilder1.Points.Add(point1);

            NXOpen.Features.Feature nullNXOpen_Features_Feature = null;
            wavePointBuilder1.FrecAtTimeStamp = nullNXOpen_Features_Feature;

            NXOpen.TaggedObject nullNXOpen_TaggedObject = null;
            wavePointBuilder1.SourcePartOccurrence = nullNXOpen_TaggedObject;

            NXOpen.Assemblies.ProductInterface.InterfaceObject[] selectedobjects1 = new NXOpen.Assemblies.ProductInterface.InterfaceObject[0];
            wavePointBuilder1.SetProductInterfaceObjects(selectedobjects1);

            NXOpen.NXObject nXObject1;
            nXObject1 = wavePointBuilder1.Commit();

            wavePointBuilder1.Destroy();


        }

        public static void editEdgeFeatureNew(int featureIndex, TaggedObject taggedObject)
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;

            NXOpen.Features.CompositeCurve compositeCurve1 = ((NXOpen.Features.CompositeCurve)workPart.Features.ToArray()[featureIndex]);
            workPart.Features.SetEditWithRollbackFeature(compositeCurve1);


            NXOpen.Features.CompositeCurveBuilder compositeCurveBuilder1;
            compositeCurveBuilder1 = workPart.Features.CreateCompositeCurveBuilder(compositeCurve1);


            compositeCurveBuilder1.Associative = true;
            NXOpen.NXObject nullNXOpen_NXObject = null;
            if (taggedObject is NXOpen.Edge)
            {
                NXOpen.SelectionIntentRuleOptions selectionIntentRuleOptions1;
                selectionIntentRuleOptions1 = workPart.ScRuleFactory.CreateRuleOptions();

                selectionIntentRuleOptions1.SetSelectedFromInactive(false);

                NXOpen.Edge[] edges1 = new NXOpen.Edge[1];
                NXOpen.Edge edge1 = (Edge)taggedObject;
                edges1[0] = edge1;
                NXOpen.EdgeDumbRule edgeDumbRule1;
                edgeDumbRule1 = workPart.ScRuleFactory.CreateRuleEdgeDumb(edges1, selectionIntentRuleOptions1);

                selectionIntentRuleOptions1.Dispose();

                //NXOpen.Edge[] edges1 = new NXOpen.Edge[1];
                //edges1[0] = (Edge)taggedObject;
                //NXOpen.EdgeDumbRule edgeDumbRule1;
                //edgeDumbRule1 = workPart.ScRuleFactory.CreateRuleEdgeDumb(edges1);
                NXOpen.SelectionIntentRule[] rules1 = new NXOpen.SelectionIntentRule[1];
                rules1[0] = edgeDumbRule1;
                Point3d helpPoint1 = new Point3d(0, 0, 0);
                compositeCurveBuilder1.Section.AddToSection(rules1, (Edge)taggedObject, nullNXOpen_NXObject, nullNXOpen_NXObject, helpPoint1, NXOpen.Section.Mode.Create, false);
            }
            else if (taggedObject is NXOpen.Curve)
            {
                NXOpen.Curve[] curve = new NXOpen.Curve[1];
                curve[0] = (Curve)taggedObject;
                NXOpen.CurveDumbRule edgeDumbRule1;
                edgeDumbRule1 = workPart.ScRuleFactory.CreateRuleCurveDumb(curve);
                NXOpen.SelectionIntentRule[] rules1 = new NXOpen.SelectionIntentRule[1];
                rules1[0] = edgeDumbRule1;
                Point3d helpPoint1 = new Point3d(0, 0, 0);
                compositeCurveBuilder1.Section.AddToSection(rules1, (Curve)taggedObject, nullNXOpen_NXObject, nullNXOpen_NXObject, helpPoint1, NXOpen.Section.Mode.Create, false);

            }
            compositeCurveBuilder1.Commit();

            compositeCurveBuilder1.Destroy();



        }



        public static TaggedObject select_anything()
        {
            TaggedObject selobj;
            Point3d cursor;
            Selection.SelectionType[] typeArray = { Selection.SelectionType.Curves };

            Selection.Response resp = Project.theUI.SelectionManager.SelectTaggedObject("Select anything", "Select anything",
                       Selection.SelectionScope.WorkPart, false, typeArray, out selobj, out cursor);

            if (resp == Selection.Response.ObjectSelected ||
                    resp == Selection.Response.ObjectSelectedByName)
            {
                return selobj;
            }
            else
                return null;
        }

        public static TaggedObject select_anything_with_mask(string type)
        {
            TaggedObject selobj;
            Point3d cursor;
            Selection.MaskTriple[] mask = new Selection.MaskTriple[1];
            mask[0].Type = UFConstants.UF_solid_type;
            mask[0].Subtype = 0;
            switch(type)
            {
                case "6": mask[0].Type = UFConstants.UF_point_type; break;
                case "7": mask[0].SolidBodySubtype = UFConstants.UF_UI_SEL_FEATURE_ANY_FACE;break;
                case "8": mask[0].SolidBodySubtype = UFConstants.UF_UI_SEL_FEATURE_ANY_EDGE; break;
                case "9": mask[0].SolidBodySubtype = UFConstants.UF_UI_SEL_FEATURE_BODY;break;
                case "10": mask[0].Type = UFConstants.UF_sketch_type;break;
                case "11": mask[0].Type = UFConstants.UF_coordinate_system_type;break;
                case "12": mask[0].Type = UFConstants.UF_datum_plane_type;break;
            }

            Selection.Response resp = Project.theUI.SelectionManager.SelectTaggedObject("Select", "Select",
                Selection.SelectionScope.AnyInAssembly, Selection.SelectionAction.ClearAndEnableSpecific, false, false, mask, out selobj, out cursor);
            /*
                    Selection.Response resp = theUI.SelectionManager.SelectObject("Select anything by mask", "Select anything by mask",
                               Selection.SelectionScope.WorkPart, false, typeArray, out selobj, out cursor);
            */
            if (resp == Selection.Response.ObjectSelected ||
                    resp == Selection.Response.ObjectSelectedByName)
            {
                return selobj;
            }
            else
                return null;
        }

        public static void editPointFeature(ReferenceButtonBean bean)
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Redefine Feature");

            NXOpen.Features.WavePoint wavePoint1 = (NXOpen.Features.WavePoint)workPart.Features.ToArray()[int.Parse(bean.FeatureIndex)] ;
            NXOpen.Features.EditWithRollbackManager editWithRollbackManager1;
            editWithRollbackManager1 = workPart.Features.StartEditWithRollbackManager(wavePoint1, markId1);
            TaggedObject taggedObject = select_anything_with_mask(bean.ReferenceType);
            while (taggedObject == wavePoint1)  //如果选择自己会出错
            {
                Project.theUI.NXMessageBox.Show("Info", NXMessageBox.DialogType.Information, "请选择其他的点！");
                taggedObject = select_anything_with_mask(bean.ReferenceType);
            }
            if (taggedObject != null)
            {
                NXOpen.Features.WavePointBuilder wavePointBuilder1;
                wavePointBuilder1 = workPart.Features.CreateWavePointBuilder(wavePoint1);

                wavePointBuilder1.ParentPart = NXOpen.Features.WavePointBuilder.ParentPartType.OtherPart;

                NXOpen.Point point1 = ((NXOpen.Point)taggedObject);
                bool added1;
                added1 = wavePointBuilder1.Points.Add(point1);

                wavePointBuilder1.Associative = true;

                NXOpen.Features.Feature nullNXOpen_Features_Feature = null;
                wavePointBuilder1.FrecAtTimeStamp = nullNXOpen_Features_Feature;

                NXOpen.TaggedObject nullNXOpen_TaggedObject = null;
                wavePointBuilder1.SourcePartOccurrence = nullNXOpen_TaggedObject;

                NXOpen.Assemblies.ProductInterface.InterfaceObject[] selectedobjects1 = new NXOpen.Assemblies.ProductInterface.InterfaceObject[0];
                wavePointBuilder1.SetProductInterfaceObjects(selectedobjects1);

                NXOpen.NXObject nXObject1;
                nXObject1 = wavePointBuilder1.Commit();

                wavePointBuilder1.Destroy();
            }

            editWithRollbackManager1.UpdateFeature(false);

            editWithRollbackManager1.Stop();

            theSession.Preferences.Modeling.UpdatePending = false;

            editWithRollbackManager1.Destroy();

        }
        public static void editEdgeFeature(ReferenceButtonBean bean)
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Redefine Feature");

            NXOpen.Features.CompositeCurve compositeCurve1 = (NXOpen.Features.CompositeCurve)workPart.Features.ToArray()[int.Parse(bean.FeatureIndex)] ;
            NXOpen.Features.EditWithRollbackManager editWithRollbackManager1;
            editWithRollbackManager1 = workPart.Features.StartEditWithRollbackManager(compositeCurve1, markId1);

            TaggedObject taggedObject = select_anything_with_mask(bean.ReferenceType);

            if(taggedObject!=null)
            { 
                NXOpen.Features.CompositeCurveBuilder compositeCurveBuilder1;
                compositeCurveBuilder1 = workPart.Features.CreateCompositeCurveBuilder(compositeCurve1);


                NXOpen.IBaseCurve[] edges1 = new NXOpen.IBaseCurve[1];
                edges1[0] = (IBaseCurve)taggedObject;
                NXOpen.CurveDumbRule edgeDumbRule1;
                edgeDumbRule1 = workPart.ScRuleFactory.CreateRuleBaseCurveDumb(edges1);

                compositeCurveBuilder1.Section.AllowSelfIntersection(false);

                compositeCurveBuilder1.Section.AllowDegenerateCurves(false);

                NXOpen.SelectionIntentRule[] rules1 = new NXOpen.SelectionIntentRule[1];
                rules1[0] = edgeDumbRule1;
                NXOpen.NXObject nullNXOpen_NXObject = null;
                NXOpen.Point3d helpPoint1 = new NXOpen.Point3d(0.0, 0.0, 0.0);
                compositeCurveBuilder1.Section.AddToSection(rules1, (NXObject)taggedObject, nullNXOpen_NXObject, nullNXOpen_NXObject, helpPoint1, NXOpen.Section.Mode.Create, false);

                compositeCurveBuilder1.Associative = true;

                NXOpen.TaggedObject nullNXOpen_TaggedObject = null;
                compositeCurveBuilder1.SourcePartOccurrence = nullNXOpen_TaggedObject;

                NXOpen.Assemblies.ProductInterface.InterfaceObject[] selectedobjects1 = new NXOpen.Assemblies.ProductInterface.InterfaceObject[0];
                compositeCurveBuilder1.SetProductInterfaceObjects(selectedobjects1);

                NXOpen.NXObject nXObject1;
                nXObject1 = compositeCurveBuilder1.Commit();

                compositeCurveBuilder1.Destroy();
            }
            editWithRollbackManager1.UpdateFeature(false);

            editWithRollbackManager1.Stop();

            theSession.Preferences.Modeling.UpdatePending = false;

            editWithRollbackManager1.Destroy();

        }

        public static void editDatumPlaneFeature(ReferenceButtonBean bean)
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Redefine Feature");

            NXOpen.Features.WaveDatum waveDatum1 = (NXOpen.Features.WaveDatum)workPart.Features.ToArray()[int.Parse(bean.FeatureIndex)];
            NXOpen.Features.EditWithRollbackManager editWithRollbackManager1;
            editWithRollbackManager1 = workPart.Features.StartEditWithRollbackManager(waveDatum1, markId1);

            NXOpen.Features.WaveDatumBuilder waveDatumBuilder1;
            waveDatumBuilder1 = workPart.Features.CreateWaveDatumBuilder(waveDatum1);

            waveDatumBuilder1.ParentPart = NXOpen.Features.WaveDatumBuilder.ParentPartType.OtherPart;
            TaggedObject taggedObject = select_anything_with_mask(bean.ReferenceType);

            if (taggedObject != null)
            {
                waveDatumBuilder1.Datums.Add(taggedObject);

                waveDatumBuilder1.Associative = true;

                waveDatumBuilder1.ReverseDirection = false;

                NXOpen.TaggedObject nullNXOpen_TaggedObject = null;
                waveDatumBuilder1.SourcePartOccurrence = nullNXOpen_TaggedObject;

                NXOpen.Assemblies.ProductInterface.InterfaceObject[] selectedobjects1 = new NXOpen.Assemblies.ProductInterface.InterfaceObject[0];
                waveDatumBuilder1.SetProductInterfaceObjects(selectedobjects1);

                NXOpen.NXObject nXObject1;
                nXObject1 = waveDatumBuilder1.Commit();

                waveDatumBuilder1.Destroy();

            }
            editWithRollbackManager1.UpdateFeature(false);

            editWithRollbackManager1.Stop();

            theSession.Preferences.Modeling.UpdatePending = false;

            editWithRollbackManager1.Destroy();

        }

        public static void editSketchFeature(ReferenceButtonBean bean)
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Edit Feature Parameters");

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "起点");

            NXOpen.Features.SketchFeature sketchFeature1 = (NXOpen.Features.SketchFeature)workPart.Features.ToArray()[int.Parse(bean.FeatureIndex)];
            NXOpen.Features.WaveSketchBuilder waveSketchBuilder1;
            waveSketchBuilder1 = workPart.Features.CreateWaveSketchBuilder(sketchFeature1);

            waveSketchBuilder1.ParentPart = NXOpen.Features.WaveSketchBuilder.ParentPartType.OtherPart;
            TaggedObject taggedObject = select_anything_with_mask(bean.ReferenceType);

            if (taggedObject != null)
            {
                //NXOpen.Features.SketchFeature sketchFeature2 = ((NXOpen.Features.SketchFeature)displayPart.Features.FindObject("SKETCH(4)"));
                //NXOpen.Sketch sketch1 = ((NXOpen.Sketch)sketchFeature2.FindObject("SKETCH_001"));
                //bool added1;
                waveSketchBuilder1.Sketches.Add(taggedObject);

                waveSketchBuilder1.Associative = true;

                NXOpen.TaggedObject nullNXOpen_TaggedObject = null;
                waveSketchBuilder1.SourcePartOccurrence = nullNXOpen_TaggedObject;

                NXOpen.Assemblies.ProductInterface.InterfaceObject[] selectedobjects1 = new NXOpen.Assemblies.ProductInterface.InterfaceObject[0];
                waveSketchBuilder1.SetProductInterfaceObjects(selectedobjects1);

                NXOpen.NXObject nXObject1;
                nXObject1 = waveSketchBuilder1.Commit();

                waveSketchBuilder1.Destroy();
            }
            int nErrs1;
            nErrs1 = theSession.UpdateManager.DoUpdate(markId1);


        }

        public static void editFaceFeature(ReferenceButtonBean bean)
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Redefine Feature");

            NXOpen.Features.ExtractFace extractFace1 = (NXOpen.Features.ExtractFace)workPart.Features.ToArray()[int.Parse(bean.FeatureIndex)];
            NXOpen.Features.EditWithRollbackManager editWithRollbackManager1;
            editWithRollbackManager1 = workPart.Features.StartEditWithRollbackManager(extractFace1, markId1);
            TaggedObject taggedObject = select_anything_with_mask(bean.ReferenceType);

            if (taggedObject != null)
            {
                NXOpen.Features.ExtractFaceBuilder extractFaceBuilder1;
                extractFaceBuilder1 = workPart.Features.CreateExtractFaceBuilder(extractFace1);

                NXOpen.TaggedObject nullNXOpen_TaggedObject = null;
                extractFaceBuilder1.SetSelectedBody(nullNXOpen_TaggedObject);

                NXOpen.DisplayableObject[] replacementobjects3 = new NXOpen.DisplayableObject[1];
                NXOpen.Face face1 = ((NXOpen.Face)taggedObject);
                replacementobjects3[0] = face1;
                extractFaceBuilder1.ReplacementAssistant.SetNewParents(replacementobjects3);

                NXOpen.SelectionIntentRuleOptions selectionIntentRuleOptions1;
                selectionIntentRuleOptions1 = workPart.ScRuleFactory.CreateRuleOptions();

                selectionIntentRuleOptions1.SetSelectedFromInactive(false);

                NXOpen.Face[] boundaryFaces1 = new NXOpen.Face[0];
                NXOpen.FaceTangentRule faceTangentRule1;
                faceTangentRule1 = workPart.ScRuleFactory.CreateRuleFaceTangent(face1, boundaryFaces1, 0.050000000000000003, selectionIntentRuleOptions1);

                selectionIntentRuleOptions1.Dispose();
                NXOpen.SelectionIntentRule[] rules2 = new NXOpen.SelectionIntentRule[1];
                rules2[0] = faceTangentRule1;
                extractFaceBuilder1.FaceChain.ReplaceRules(rules2, false);


                extractFaceBuilder1.SetSelectedBody(face1);

                extractFaceBuilder1.Associative = true;


                extractFaceBuilder1.SourcePartOccurrence = nullNXOpen_TaggedObject;

                NXOpen.Assemblies.ProductInterface.InterfaceObject[] selectedobjects1 = new NXOpen.Assemblies.ProductInterface.InterfaceObject[0];
                extractFaceBuilder1.SetProductInterfaceObjects(selectedobjects1);

                NXOpen.NXObject nXObject1;
                nXObject1 = extractFaceBuilder1.Commit();

                extractFaceBuilder1.Destroy();
            }

            editWithRollbackManager1.UpdateFeature(false);

            editWithRollbackManager1.Stop();

            theSession.Preferences.Modeling.UpdatePending = false;

            editWithRollbackManager1.Destroy();

        }

        public static void editBodyFeature(ReferenceButtonBean bean)
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Redefine Feature");

            NXOpen.Features.ExtractFace extractFace1 = (NXOpen.Features.ExtractFace)workPart.Features.ToArray()[int.Parse(bean.FeatureIndex)];
            NXOpen.Features.EditWithRollbackManager editWithRollbackManager1;
            editWithRollbackManager1 = workPart.Features.StartEditWithRollbackManager(extractFace1, markId1);
            TaggedObject taggedObject = select_anything_with_mask(bean.ReferenceType);

            if (taggedObject != null)
            {
                NXOpen.Features.ExtractFaceBuilder extractFaceBuilder1;
                extractFaceBuilder1 = workPart.Features.CreateExtractFaceBuilder(extractFace1);


                NXOpen.TaggedObject nullNXOpen_TaggedObject = null;
                extractFaceBuilder1.SetSelectedBody(nullNXOpen_TaggedObject);

                NXOpen.SelectionIntentRuleOptions selectionIntentRuleOptions1;
                selectionIntentRuleOptions1 = workPart.ScRuleFactory.CreateRuleOptions();

                selectionIntentRuleOptions1.SetSelectedFromInactive(false);

                NXOpen.Body[] bodies1 = new NXOpen.Body[1];
                NXOpen.Body body1 = ((NXOpen.Body)taggedObject);
                bodies1[0] = body1;
                NXOpen.BodyDumbRule bodyDumbRule1;
                bodyDumbRule1 = workPart.ScRuleFactory.CreateRuleBodyDumb(bodies1, true, selectionIntentRuleOptions1);

                selectionIntentRuleOptions1.Dispose();
                NXOpen.SelectionIntentRule[] rules1 = new NXOpen.SelectionIntentRule[1];
                rules1[0] = bodyDumbRule1;
                extractFaceBuilder1.ExtractBodyCollector.ReplaceRules(rules1, false);

                NXOpen.DisplayableObject[] replacementobjects3 = new NXOpen.DisplayableObject[1];
                replacementobjects3[0] = body1;
                extractFaceBuilder1.ReplacementAssistant.SetNewParents(replacementobjects3);

                extractFaceBuilder1.SetSelectedBody(body1);

                //NXOpen.Features.Block block1 = ((NXOpen.Features.Block)displayPart.Features.FindObject("BLOCK(1)"));
                //extractFaceBuilder1.FrecAtTimeStamp = block1;

                extractFaceBuilder1.Associative = true;

                extractFaceBuilder1.SourcePartOccurrence = nullNXOpen_TaggedObject;

                NXOpen.Assemblies.ProductInterface.InterfaceObject[] selectedobjects1 = new NXOpen.Assemblies.ProductInterface.InterfaceObject[0];
                extractFaceBuilder1.SetProductInterfaceObjects(selectedobjects1);

                NXOpen.NXObject nXObject1;
                nXObject1 = extractFaceBuilder1.Commit();

                extractFaceBuilder1.Destroy();
            }

            editWithRollbackManager1.UpdateFeature(false);

            editWithRollbackManager1.Stop();

            theSession.Preferences.Modeling.UpdatePending = false;

            editWithRollbackManager1.Destroy();

            // ----------------------------------------------
            //   菜单：工具(T)->操作记录(J)->停止录制(S)
            // ----------------------------------------------

        }

        public static void editCsysFeature(ReferenceButtonBean bean)
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Redefine Feature");

            NXOpen.Features.DatumCsys datumCsys1 = (NXOpen.Features.DatumCsys)workPart.Features.ToArray()[int.Parse(bean.FeatureIndex)];
            NXOpen.Features.EditWithRollbackManager editWithRollbackManager1;
            editWithRollbackManager1 = workPart.Features.StartEditWithRollbackManager(datumCsys1, markId1);
            TaggedObject taggedObject = select_anything_with_mask(bean.ReferenceType);

            if (taggedObject != null)
            {
                NXOpen.Features.WaveDatumBuilder waveDatumBuilder1;
                waveDatumBuilder1 = workPart.Features.CreateWaveDatumBuilder(datumCsys1);

                waveDatumBuilder1.ParentPart = NXOpen.Features.WaveDatumBuilder.ParentPartType.OtherPart;
                
                waveDatumBuilder1.Datums.Add(taggedObject);

                waveDatumBuilder1.Associative = true;


                NXOpen.TaggedObject nullNXOpen_TaggedObject = null;
                waveDatumBuilder1.SourcePartOccurrence = nullNXOpen_TaggedObject;

                NXOpen.Assemblies.ProductInterface.InterfaceObject[] selectedobjects1 = new NXOpen.Assemblies.ProductInterface.InterfaceObject[0];
                waveDatumBuilder1.SetProductInterfaceObjects(selectedobjects1);

                NXOpen.NXObject nXObject1;
                nXObject1 = waveDatumBuilder1.Commit();
                waveDatumBuilder1.Destroy();
            }
            editWithRollbackManager1.UpdateFeature(false);

            editWithRollbackManager1.Stop();

            theSession.Preferences.Modeling.UpdatePending = false;

            editWithRollbackManager1.Destroy();

            // ----------------------------------------------
            //   菜单：工具(T)->操作记录(J)->停止录制(S)
            // ----------------------------------------------

        }

        public static void batchChangeComponentName(Component root, string attributeName, ref List<string> deleteTheseFiles)
        {
            Component[] children = root.GetChildren();
            //Component parent;
            foreach (Component child in children)
            {
                //Console.WriteLine(child.Name);

                Part theProtoPart = (Part)child.Prototype;
                //Part theParentProtoPart;
                if (!theProtoPart.HasUserAttribute(attributeName, NXObject.AttributeType.Any, -1)) continue;
                string attributeValue = theProtoPart.GetUserAttributeAsString(attributeName, NXObject.AttributeType.Any, -1);
                if (!"".Equals(attributeValue))
                {
                    //Console.WriteLine("");
                    //Console.WriteLine("Found : " + child.Name);
                    //Console.WriteLine("   full path: " + theProtoPart.FullPath);
                    
                    //parent = child.Parent;
                    //theParentProtoPart = (Part)parent.Prototype; 
                    //Console.WriteLine("   parent assy path: " + theParentProtoPart.FullPath);
                    //Console.WriteLine("");

                    // set the found component the current workpart.
                    NXOpen.PartLoadStatus partLoadStatus1;
                    Project.theSession.Parts.SetWorkComponent(child, NXOpen.PartCollection.RefsetOption.Entire, NXOpen.PartCollection.WorkComponentOption.Visible, out partLoadStatus1);
                    partLoadStatus1.Dispose();

                    // build the new path containing the new name.
                    string thePath = theProtoPart.FullPath;
                    thePath = thePath.Substring(0, thePath.LastIndexOf("\\"));
                    thePath = thePath + "\\" + attributeValue + ".prt";
                    //thePath = thePath + "\\" + (IsSheet(theProtoPart)?attributeValue+"_2D": attributeValue) + ".prt";
                    //Console.WriteLine("the new path: " + thePath);

                    // save the component as the new name.
                    Part workPart = Project.theSession.Parts.Work;
                    if (!File.Exists(thePath))
                    {
                        deleteTheseFiles.Add(theProtoPart.FullPath);

                        NXOpen.PartSaveStatus partSaveStatus1;
                        partSaveStatus1 = workPart.SaveAs(thePath);
                        partSaveStatus1.Dispose();
                    }else
                    {
                        Echo(child.DisplayName + "文件已经存在，跳过重命名！");
                    }
                }
                batchChangeComponentName(child, attributeName, ref deleteTheseFiles);
            }
        }

        public static void Echo(string output)
        {
            if (!Project.lw.IsOpen) Project.lw.Open();
            Project.lw.WriteLine(output);
        }

        public static bool IsSheet(Part dp)
        {
            bool isSheet = false;
            DrawingSheet[] drawingSheets = dp.DrawingSheets.ToArray();

            if (drawingSheets.GetLength(0) > 0)
            {
                isSheet = true;
            }
            return isSheet;
        }

        
}
}