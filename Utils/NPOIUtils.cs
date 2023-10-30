using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aspose.Cells;
using MainProject.Bean;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace MainProject.Utils
{
    public class NPOIUtils
    {
        public XSSFWorkbook hssfworkbook;

        public void InitializeWorkbook(string path)
        {
            //read the template via FileStream, it is suggested to use FileAccess.Read to prevent file lock.
            //book1.xls is an Excel-2007-generated file, so some new unknown BIFF records are added. 
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new XSSFWorkbook(file);
            }
        }

        public void loadExcelData()
        {
            ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            rows.MoveNext();
            IRow headRow = null;
            while (rows.MoveNext())
            {
                IRow row = (XSSFRow)rows.Current;
                if (row.GetCell(0) == null) continue;
                if("显示名称".Equals(row.GetCell(0)))
                {
                    headRow = row;
                    break;
                }
                
            }
            ParameterBean parameterBean = new ParameterBean();
            parameterBean.DisplayName = headRow.GetCell(1).StringCellValue;
            //<string, Dictionary<string, List<SelectNumBean>>> test = keyValuePairs.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);
            //return keyValuePairs.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);
        }
        public static void loadExcelDataNew(ButtonBean buttonBean)
        {
            // 加載 Excel 文件

            Workbook wb = new Workbook(buttonBean.excelPath);

            // 獲取所有工作表
            WorksheetCollection collection = wb.Worksheets;

            List<ParameterBean> parameterBeans = new List<ParameterBean>();
            List<ClassBean> classBeans = new List<ClassBean>();
            List<TabBean> tabBeans = new List<TabBean>();
            // 遍歷所有工作表
            for (int worksheetIndex = 0; worksheetIndex < collection.Count; worksheetIndex++)
            {

                // 使用其索引獲取工作表
                Worksheet worksheet = collection[worksheetIndex];

                // 打印工作表名稱
                Console.WriteLine("Worksheet: " + worksheet.Name);
                if(!worksheet.Name.Equals(buttonBean.excelSheet))
                {
                    continue;
                }
                // 獲取行數和列數
                int rows = worksheet.Cells.MaxDataRow;
                int cols = worksheet.Cells.MaxDataColumn;
                int firstRow = 0;
                // 遍歷行
                for (int i = 0; i < rows; i++)
                {
                    if ("菜单".Equals(worksheet.Cells[i, 0].Value))
                    {
                        firstRow = i;   //获取第一个数据行索引
                        break;
                    }
                }
                int expDisNameRow = firstRow + 2; //参数行开始位置

                //先获取所有的tab页
                for (int i = 1; i < cols; i++)
                {
                    string tabName = worksheet.Cells[firstRow, i].StringValue;
                    if(!"".Equals(tabName))
                    {
                        TabBean tabBean = new TabBean();
                        tabBean.TabName = tabName;
                        tabBean.Bitmap = worksheet.Cells[firstRow+1, i].StringValue;
                        tabBean.index = i;   //tab页对应的列号

                        /*
                        List<SkechButtonBean> sketchButtonBeans = new List<SkechButtonBean>();
                        string skechs = worksheet.Cells[firstRow + 2, i].StringValue;
                        if(!"".Equals(skechs))
                        { 
                            foreach (string buttonNameSketch in worksheet.Cells[firstRow + 2, i].StringValue.Split(','))
                            {
                                string[] buttonSketch = buttonNameSketch.Split('=');
                                SkechButtonBean skechButtonBean = new SkechButtonBean();
                                skechButtonBean.ButtonName = buttonSketch[0];
                                skechButtonBean.SketchName = buttonSketch[1];
                                sketchButtonBeans.Add(skechButtonBean);
                            }
                            tabBean.SkecthButtonBeans = sketchButtonBeans;
                        }
                        */
                        tabBeans.Add(tabBean);
                    }
                }

                //获取tab页中的参数
                for (int i = 0; i < tabBeans.Count; i++)
                {
                    TabBean lastTabBean = tabBeans[i];
                    List<ParameterBean> tempParameterBean = new List<ParameterBean>();
                    List<ReferenceButtonBean> referenceButtonBeans = new List<ReferenceButtonBean>();
                    List<SkechButtonBean> sketchButtonBeans = new List<SkechButtonBean>();
                    if (i < tabBeans.Count - 1)  //没到最后一个的时候，获取的是上一个索引到下一个索引
                    {
                        TabBean nextTabBean = tabBeans[i+1];
                        for (int j = lastTabBean.index; j < nextTabBean.index; j++)
                        {
                            //string tabName = worksheet.Cells[firstRow, i].StringValue;
                            ParameterBean parameterBean = new ParameterBean();
                            parameterBean.DisplayName = worksheet.Cells[expDisNameRow, j].StringValue;
                            parameterBean.ExpressionName = worksheet.Cells[expDisNameRow + 1, j].StringValue;
                            parameterBean.isDisplay = "是".Equals(worksheet.Cells[expDisNameRow + 2, j].StringValue);
                            parameterBean.ValueType = worksheet.Cells[expDisNameRow + 3, j].StringValue;
                            parameterBean.ValueScope = worksheet.Cells[expDisNameRow + 4, j].StringValue;
                            
                            if (int.Parse(parameterBean.ValueType) == 5)
                            {
                                SkechButtonBean skechButtonBean = new SkechButtonBean();
                                skechButtonBean.ButtonName = parameterBean.DisplayName;
                                skechButtonBean.SketchName = parameterBean.ExpressionName;
                                sketchButtonBeans.Add(skechButtonBean);
                            }
                            else if (int.Parse(parameterBean.ValueType) > 5)
                            {
                                ReferenceButtonBean referenceButtonBean = new ReferenceButtonBean();
                                referenceButtonBean.ReferenceType = parameterBean.ValueType;
                                referenceButtonBean.ButtonName = parameterBean.DisplayName;
                                referenceButtonBean.FeatureIndex = parameterBean.ExpressionName;
                                referenceButtonBeans.Add(referenceButtonBean);
                            }else
                            {
                                if ("2".Equals(parameterBean.ValueType) || "3".Equals(parameterBean.ValueType))
                                {
                                    Dictionary<string, string> values = new Dictionary<string, string>();
                                    string[] items = parameterBean.ValueScope.Split(',');
                                    foreach (string item in items)
                                    {
                                        if (parameterBean.ValueScope.Contains("="))
                                        {
                                            string[] itemvalue = item.Split('=');
                                            values.Add(itemvalue[0], itemvalue[1]);
                                        }
                                        else
                                        {
                                            values.Add(item, item);
                                        }
                                    }
                                    parameterBean.DisRealValueDic = values;
                                }
                                tempParameterBean.Add(parameterBean);
                            }
                        }
                    }
                    else //到最后一个的时候，获取的是上一个索引到末尾列
                    {
                        for (int j = lastTabBean.index; j <= cols; j++)
                        {
                           //string tabName = worksheet.Cells[firstRow, i].StringValue;
                            ParameterBean parameterBean = new ParameterBean();
                            parameterBean.DisplayName = worksheet.Cells[expDisNameRow, j].StringValue;
                            parameterBean.ExpressionName = worksheet.Cells[expDisNameRow + 1, j].StringValue;
                            parameterBean.isDisplay = "是".Equals(worksheet.Cells[expDisNameRow + 2, j].StringValue);
                            parameterBean.ValueType = worksheet.Cells[expDisNameRow + 3, j].StringValue;
                            parameterBean.ValueScope = worksheet.Cells[expDisNameRow + 4, j].StringValue;
                            if (int.Parse(parameterBean.ValueType) == 5)
                            {
                                SkechButtonBean skechButtonBean = new SkechButtonBean();
                                skechButtonBean.ButtonName = parameterBean.DisplayName;
                                skechButtonBean.SketchName = parameterBean.ExpressionName;
                                sketchButtonBeans.Add(skechButtonBean);
                            }
                            else if (int.Parse(parameterBean.ValueType) > 5)
                            {
                                ReferenceButtonBean referenceButtonBean = new ReferenceButtonBean();
                                referenceButtonBean.ReferenceType = parameterBean.ValueType;
                                referenceButtonBean.ButtonName = parameterBean.DisplayName;
                                referenceButtonBean.FeatureIndex = parameterBean.ExpressionName;
                                referenceButtonBeans.Add(referenceButtonBean);
                            }
                            else
                            {
                                if ("2".Equals(parameterBean.ValueType) || "3".Equals(parameterBean.ValueType))
                                {
                                    Dictionary<string, string> values = new Dictionary<string, string>();
                                    string[] items = parameterBean.ValueScope.Split(',');
                                    foreach (string item in items)
                                    {
                                        if (parameterBean.ValueScope.Contains("="))
                                        {
                                            string[] itemvalue = item.Split('=');
                                            values.Add(itemvalue[0], itemvalue[1]);
                                        }
                                        else
                                        {
                                            values.Add(item, item);
                                        }
                                    }
                                    parameterBean.DisRealValueDic = values;
                                }
                                tempParameterBean.Add(parameterBean);
                            }
                        }
                    }
                    parameterBeans.AddRange(tempParameterBean);
                    lastTabBean.ParameterBeans = tempParameterBean;
                    lastTabBean.referenceButtonBeans = referenceButtonBeans;
                    lastTabBean.SkecthButtonBeans = sketchButtonBeans;

                }
                /*
                int index = i;
                foreach (TabBean tabBean in tabBeans)
                {
                    for (int i = index; i < tabBean.index; i++)
                    {
                    }
                }
                for (int i = 1; i < cols; i++)
                {
                    string tabName = worksheet.Cells[firstRow, i].StringValue;
                    ParameterBean parameterBean = new ParameterBean();
                    parameterBean.DisplayName = worksheet.Cells[firstRow, i].StringValue;
                    parameterBean.ExpressionName = worksheet.Cells[firstRow+1, i].StringValue;
                    parameterBean.isDisplay = "是".Equals(worksheet.Cells[firstRow+2, i].StringValue);
                    parameterBean.ValueType = worksheet.Cells[firstRow+3, i].StringValue;
                    parameterBean.ValueScope = worksheet.Cells[firstRow+4, i].StringValue;
                    if(parameterBean.ValueScope.Contains("="))
                    {
                        Dictionary<string, string> values = new Dictionary<string, string>();
                        string[] items = parameterBean.ValueScope.Split(',');
                        foreach (string item in items)
                        {
                            string[] itemvalue = item.Split('=');
                            values.Add(itemvalue[0], itemvalue[1]);
                        }
                        parameterBean.DisRealValueDic = values;
                    }
                    parameterBeans.Add(parameterBean);
                }
                */

                for (int i = expDisNameRow + 5; i <= rows; i++)
                {
                    ClassBean classBean = new ClassBean();
                    classBean.Name = worksheet.Cells[i, 0].StringValue;
                    Dictionary<string, string> expValuePairs = new Dictionary<string, string>();
                    for (int j = 1; j <= cols; j++)
                    {
                        string typeIndex = worksheet.Cells[expDisNameRow + 3, j].StringValue;
                        if (int.Parse(typeIndex) > 4)
                            continue;
                        string exp = worksheet.Cells[expDisNameRow + 1, j].StringValue;
                        string value = worksheet.Cells[i, j].StringValue;
                        expValuePairs.Add(exp, value);
                    }
                    classBean.expDictionary = expValuePairs;
                    classBeans.Add(classBean);
                }
            }
            buttonBean.parameterBeans = parameterBeans;
            buttonBean.classBeans = classBeans;
            buttonBean.TabBeans = tabBeans;

        }

        public static Dictionary<string, string> loadExcelLoginData(string loginExcelPath)
        {
            // 加載 Excel 文件
           Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
           Workbook wb = new Workbook(loginExcelPath);

            // 獲取所有工作表
            WorksheetCollection collection = wb.Worksheets;

            List<ParameterBean> parameterBeans = new List<ParameterBean>();
            List<ClassBean> classBeans = new List<ClassBean>();
            List<TabBean> tabBeans = new List<TabBean>();
            // 遍歷所有工作表
            for (int worksheetIndex = 0; worksheetIndex < collection.Count; worksheetIndex++)
            {

                // 使用其索引獲取工作表
                Worksheet worksheet = collection[worksheetIndex];

                // 打印工作表名稱
                Console.WriteLine("Worksheet: " + worksheet.Name);

                // 獲取行數和列數
                int rows = worksheet.Cells.MaxDataRow;
                int cols = worksheet.Cells.MaxDataColumn;
                for (int i = 1; i <= rows; i++)
                {
                    string userID = worksheet.Cells[i, 0].StringValue;
                    string password = worksheet.Cells[i, 1].StringValue;
                    if(!"".Equals(userID) && !keyValuePairs.ContainsKey(userID))
                    {
                        keyValuePairs.Add(userID, password);
                    }
                }
            }
            return keyValuePairs;
        }


    }
}
