//----------------------------------------------------------------------------
//                  Copyright (c) 2007 Siemens
//                      All rights reserved
//----------------------------------------------------------------------------
// 
//
// MenuBarCSharpApp.cs
//
// Description:
//   This is a an example of adding a C Sharp application to NX.
//
//   Note: You must include a reference to Microsoft.VisualBasic to 
//   compile this program.
//
//   In addition to using this source file there are two menu files 
//   (MenuBarDotNetAppButton.men and MenuBarCSharpApp.men) which are also required.
//   All of the files for this sample reside in the ugopen kit under:
//   SampleNXOpenApplications/.NET/MenuBarDotNetApp.
//
//   Please see the "Adding Custom Applications to NX" chapter of the
//   NX Open Programmer's Guide or search the documentation for
//   "MenuBarDotNetApp" for a detailed description of this example.
//
//   These files are intended to provide a template that can be modified
//   by renaming the files, and renaming the variables and routines
//   to allow you create your own application.
//
//   Additional information on MenuScript can be found in:
//     - the MenuScript User's Guide
//     - the NXOpen Programmer's Guide
//     - the NX Open for .NET Reference Guide (see MenuBar)
//     - any other place in the NX documentation that comes up when
//       you search for "MenuScript" or "MenuBar"
//
//   *** PLEASE NOTE ***
//   The shared library created from this program must be placed in the
//   "application" directory under a directory listed in the file pointed to by the
//   environment variable in UGII_CUSTOM_DIRECTORY_FILE (i.e. the shared library must
//   not be in the "startup" directory).
//
//----------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using MainProject.Bean;
using MainProject.Forms;
using MainProject.Utils;
using Microsoft.VisualBasic;
using NXOpen;
using NXOpen.Assemblies;
using NXOpen.UF;

public class Project
{
    // class members
    public static Session theSession = null;
    public static UFSession theUFSession = null;
    public static UI theUI = null;
    public static ListingWindow lw = null;
    public static bool isDisposeCalled;
    public static List<ButtonBean> bts = null;

   
    public static NXOpen.MenuBar.MenuBarManager.CallbackStatus EnterClass(NXOpen.MenuBar.MenuButtonEvent buttonEvent)
    {
        //foreach (ButtonBean buttonBea111n in bts)


        NXOpen.MenuBar.MenuButton button = buttonEvent.ActiveButton;
        string button_name = buttonEvent.ActiveButton.ButtonName;
        theSession.LogFile.WriteLine("curent name = " + button_name);
        ButtonBean buttonBean = bts.Find(x => button_name.Equals(x.ID_VALUE));
        if (buttonBean != null)
        {
            theSession.LogFile.WriteLine("Current Button is " + buttonBean.ToString());

            string templateID = buttonBean.ID_NAME;
            string templateValue = buttonBean.ID_VALUE;
            string template_path = buttonBean.templateName;
            if(!File.Exists(template_path))
            {
                UI.GetUI().NXMessageBox.Show("Error", NXMessageBox.DialogType.Error, "模板文件不存在，请检查！");
                return NXOpen.MenuBar.MenuBarManager.CallbackStatus.Error;
            }


            string template_name = Path.GetFileNameWithoutExtension(template_path);
            string template_folder = Path.GetDirectoryName(template_path);
        
            
            Part workPart = theSession.Parts.Work;
            if(workPart==null)
            {
                string target_folder = NXUtils.select_folder();
                if (target_folder != "")
                {
                    //string template_folder = Constain.VENDORDIR + "\\template\\";
                    //string target_folder = Path.GetDirectoryName(file_path);
                    //string replace_name = Path.GetFileNameWithoutExtension(file_path);
                    //file_path
                    NXUtils.AssemClone(target_folder, template_path);
                    //FileUtil.CopyDirectory(template_folder, target_folder);
                    PartLoadStatus loadStatus = null;
                    theSession.Parts.OpenDisplay(target_folder + "\\" + template_name + ".prt", out loadStatus);
                    //NXUtils.addcomponent(target_folder + "\\" + replace_name + ".prt");
                }
            }
            else
            { 
                int return_code = NXUtils.readpartdata(theSession.Parts.Work, templateID , templateValue);
                // The following method shows the dialog immediately
                if (return_code == 0)
                {
                    if (!File.Exists(buttonBean.excelPath))
                    {
                        UI.GetUI().NXMessageBox.Show("Error",NXMessageBox.DialogType.Error,"数据文件不存在，请检查！");
                        return NXOpen.MenuBar.MenuBarManager.CallbackStatus.Error;
                    }
                    try
                    {
                        NPOIUtils.loadExcelDataNew(buttonBean);
                    }
                    catch (Exception ex)
                    {
                        UI.GetUI().NXMessageBox.Show("Error", NXMessageBox.DialogType.Error, ex.Message);
                        return NXOpen.MenuBar.MenuBarManager.CallbackStatus.Error;
                    }
                    Form1 form1 = Form1.InstanceObject(buttonBean);
                    form1.Focus();
                    form1.Show();
                }
                else if (return_code == 2) theUI.NXMessageBox.Show("Error", NXMessageBox.DialogType.Error, "修改类型选择错误！");
                else
                {
                    string target_folder = NXUtils.select_folder();
                    if (target_folder != "")
                    {
                        //string template_folder = Constain.VENDORDIR + "\\template\\";
                        //string target_folder = Path.GetDirectoryName(file_path);
                        //string replace_name = Path.GetFileNameWithoutExtension(file_path);
                        FileUtil.CopyDirectory(template_folder, target_folder);
                        //file_path
                        //NXUtils.AssemClone(target_folder, template_path, template_name, replace_name);
                        NXUtils.addcomponent(target_folder + "\\" + template_name + ".prt");
                    }
                }
            }

        }
        return NXOpen.MenuBar.MenuBarManager.CallbackStatus.Continue;
    }

    public static NXOpen.MenuBar.MenuBarManager.CallbackStatus LoginCallBack(NXOpen.MenuBar.MenuButtonEvent buttonEvent)
    {
        List<ProgramSeq> licenseQuary = LiteDBUtils.queryAll();
        if(licenseQuary.Count != 0)
        {
            ProgramSeq current = licenseQuary[0];
            if (current.index>150)
            {
                //UI.GetUI().NXMessageBox.Show("Error", NXMessageBox.DialogType.Error, "");
                return NXOpen.MenuBar.MenuBarManager.CallbackStatus.Continue;
            }
            else
            {
                current.index = current.index + 1;
                LiteDBUtils.updateProgramSeq(current);
            }
        }
        else
        {
            ProgramSeq customer = new ProgramSeq
            {
                index = 1
            };

            LiteDBUtils.insertData(customer);
        }

        DateTime now = DateTime.Now;
        DateTime dateTime = new DateTime(2023, 12, 30);
        if (DateTime.Compare(now, dateTime) > 0)
        {
            return NXOpen.MenuBar.MenuBarManager.CallbackStatus.Continue;
        }
        LoginForm form1 = new LoginForm(bts);
        form1.ShowDialog(); //


        return NXOpen.MenuBar.MenuBarManager.CallbackStatus.Continue;
    }

    public static NXOpen.MenuBar.MenuBarManager.CallbackStatus ImportTemplateCallBack(NXOpen.MenuBar.MenuButtonEvent buttonEvent)
    {
        ImportTempalteForm form1 = new ImportTempalteForm();
        form1.Show();


        return NXOpen.MenuBar.MenuBarManager.CallbackStatus.Continue;

    }

    public static NXOpen.MenuBar.MenuBarManager.CallbackStatus ModifyComponentNameCallBack(NXOpen.MenuBar.MenuButtonEvent buttonEvent)
    {
        Part dp = theSession.Parts.Display;
        string thePath = dp.FullPath;
        string folderPath = thePath.Substring(0, thePath.LastIndexOf("\\")); ;
        string backup = folderPath + "\\backup";
        Component c = dp.ComponentAssembly.RootComponent;
        List<string> deleteFiles = new List<string>();
        if (c != null) NXUtils.batchChangeComponentName(c, "EXTEND10", ref deleteFiles);
        
        if(dp.HasUserAttribute("EXTEND10", NXObject.AttributeType.Any, -1))
        {
            string attributeValue = dp.GetUserAttributeAsString("EXTEND10", NXObject.AttributeType.Any, -1);
            if(!"".Equals(attributeValue))
            { 
                string newFilePath = folderPath + "\\" +  attributeValue + ".prt";
                //string newFilePath = folderPath + "\\" + (NXUtils.IsSheet(dp) ? attributeValue + "_2D" : attributeValue) + ".prt";
                if (!File.Exists(newFilePath))
                {
                    deleteFiles.Add(thePath);
                    NXOpen.PartSaveStatus partSaveStatus1;
                    partSaveStatus1 = dp.SaveAs(newFilePath);
                    partSaveStatus1.Dispose();
                }
                else
                {
                    NXUtils.Echo(c.DisplayName + "文件已经存在，跳过重命名！");
                }
            }
        }
        Directory.CreateDirectory(backup);
        foreach (string item in deleteFiles)
        {
            File.Move(item, backup + "\\" + Path.GetFileName(item));
        }
        return NXOpen.MenuBar.MenuBarManager.CallbackStatus.Continue;
    }

    //------------------------------------------------------------------------------
    //  NX Startup
    //      This entry point registers the application at NX startup
    //------------------------------------------------------------------------------
    public static int Startup()
    {
        int retValue = 0;
        try
        {
            if (theSession == null)
            {
                theSession = Session.GetSession();
                theUFSession = UFSession.GetUFSession();
                theUI = UI.GetUI();
            }
            if (lw == null)
            {
                lw = theSession.ListingWindow;
            }
            { 

            bts = XmlDocumentUtil.LoadXML(Constain.VENDORDIR + "\\config\\ParametricDesignConfig.xml");
            //theUI.MenuBarManager.RegisterApplication("SAMPLE_CSHARP_APP",
            //new NXOpen.MenuBar.MenuBarManager.InitializeMenuApplication(Program.ApplicationInit),
            //new NXOpen.MenuBar.MenuBarManager.EnterMenuApplication(Program.ApplicationEnter),
            //new NXOpen.MenuBar.MenuBarManager.ExitMenuApplication(Project.ApplicationExit), true, true, true);
            theUI.MenuBarManager.AddMenuAction("PARAMETRIC_DESIGN", new NXOpen.MenuBar.MenuBarManager.ActionCallback(Project.EnterClass));
            theUI.MenuBarManager.AddMenuAction("LOGIN_ACTION", new NXOpen.MenuBar.MenuBarManager.ActionCallback(Project.LoginCallBack));
            theUI.MenuBarManager.AddMenuAction("IMPORT_TEMPLATE", new NXOpen.MenuBar.MenuBarManager.ActionCallback(Project.ImportTemplateCallBack));
            theUI.MenuBarManager.AddMenuAction("BATCH_MODIFY_NAME", new NXOpen.MenuBar.MenuBarManager.ActionCallback(Project.ModifyComponentNameCallBack));
            }

            //theUI.MenuBarManager.AddMenuAction("MOVABLEJOINT", new NXOpen.MenuBar.MenuBarManager.ActionCallback(Program.NewCoatNut));
            //theUI.MenuBarManager.AddMenuAction("SAMPLE_CSHARP_APP__action3", new NXOpen.MenuBar.MenuBarManager.ActionCallback(Program.PrintApplicationIdCB));
            //theUI.MenuBarManager.AddMenuAction("SAMPLE_CSHARP_APP__action4", new NXOpen.MenuBar.MenuBarManager.ActionCallback(Program.PrintButtonDataCB));
            //theUI.MenuBarManager.AddMenuAction("SAMPLE_CSHARP_APP__action5", new NXOpen.MenuBar.MenuBarManager.ActionCallback(Program.PrintToggleStatusCB));
        }
        catch (NXOpen.NXException ex)
        {
            throw ex;
            // ---- Enter your exception handling code here -----
        }
        return retValue;
    }

    public static int Main()
    {
        int retValue = 0;
        try
        {
            //Form1 form1 = new Form1();
            //form1.ShowDialog();
            //List<ButtonBean> bts =XmlDocumentUtil.LoadXML("C:\\Program Files (x86)\\Siemens\\ParametricDesignPluginSystem\\config\\ParametricDesignConfig.xml");
            //new Form2()).ShowDialog();
            //theprocessInfo = new processInfo();
            //XmlDocumentUtil.ParseXML();
            // The following method shows the dialog immediately
            //theprocessInfo.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
           
            //---- Enter your exception handling code here -----
            //theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
        }
        finally
        {
        }
        return retValue;
    }

    //------------------------------------------------------------------------------
    // Following method disposes all the class members
    //------------------------------------------------------------------------------
    public void Dispose()
    {
        try
        {
            if (isDisposeCalled == false)
            {
                //TODO: Add your application code here 
            }
            isDisposeCalled = true;
        }
        catch (NXOpen.NXException ex)
        {
            throw ex;
            // ---- Enter your exception handling code here -----

        }
    }

    //------------------------------------------------------------
    //
    //  GetUnloadOption()
    //
    //     Used to tell NX when to unload this library
    //
    //     Available options include:
    //       Session.LibraryUnloadOption.Immediately
    //       Session.LibraryUnloadOption.Explicitly
    //       Session.LibraryUnloadOption.AtTermination
    //
    //     Any programs that register callbacks must use 
    //     AtTermination as the unload option.
    //------------------------------------------------------------
    public static int GetUnloadOption(string arg)
    {
        //Unloads the image explicitly, via an unload dialog
        //return System.Convert.ToInt32(Session.LibraryUnloadOption.Explicitly);

        //Unloads the image immediately after execution within NX
        // return System.Convert.ToInt32(Session.LibraryUnloadOption.Immediately);

        //Unloads the image when the NX session terminates
        return System.Convert.ToInt32(Session.LibraryUnloadOption.AtTermination);
    }

}

