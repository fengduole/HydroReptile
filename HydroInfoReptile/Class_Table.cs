using System;
using System.Text;
using System.IO;

namespace HydroInfoReptile
{
    public class Class_Table
    {
        public static string _dataDir = "";

        const int _max_item = 10001;
        const int _max_col = 11;

        public bool _loadStatus;

        string[] _colName = new string[_max_col];
        Class_TableItem[] _item = new Class_TableItem[_max_item];

        string _tableName;
        string _tableTime;

        int _n_col;
        int _n_item;


        //构造函数
        public Class_Table(string name)
        {
            _n_col = 0;
            _n_item = 0;

            _loadStatus = true;
            _tableName = name;
            _tableTime = "";

            for (int i = 0; i < _max_item; i++)
            {
                _item[i] = new Class_TableItem(i);
            }
        }


        //解析数据
        public void ExtractFromHtml(string html)
        {
            if (_tableName == "全国大型水库实时水情")
            {
                try
                {
                    ExtractSK(html);
                    WriteLog("Interpret - SK", "Successful");
                }
                catch
                {
                    _loadStatus = false;
                    WriteLog("Interpret - SK", "Failed");
                    WriteString(html);
                }

                try
                {
                    SaveCSV(_tableTime + " - 大型水库");
                    WriteLog("SaveCSV - SK", "Successful");
                }
                catch
                {
                    _loadStatus = false;
                    WriteLog("SaveCSV - SK", "Failed");
                }
            }
            else if (_tableName == "全国大江大河实时水情")
            {
                try
                {
                    ExtractJH(html);
                    WriteLog("Interpret - JH", "Successful");
                }
                catch
                {
                    _loadStatus = false;
                    WriteLog("Interpret - JH", "Failed");
                    WriteString(html);
                }

                try
                {
                    SaveCSV(_tableTime + " - 大江大河");
                    WriteLog("SaveCSV - JH", "Successful");
                }
                catch
                {
                    _loadStatus = false;
                    WriteLog("SaveCSV - JH", "Failed");
                }
            }
            else if (_tableName == "全国重点站实时雨情")
            {
                try
                {
                    ExtractYS(html);
                    WriteLog("Interpret - YS", "Successful");
                }
                catch
                {
                    _loadStatus = false;
                    WriteLog("Interpret - YS", "Failed");
                    WriteString(html);
                }

                try
                {
                    SaveCSV(_tableTime + " - 重点雨情");
                    WriteLog("SaveCSV - YS", "Successful");
                }
                catch
                {
                    _loadStatus = false;
                    WriteLog("SaveCSV - YS", "Failed");
                }
            }
        }


        /// <summary>
        /// 保存为.csv文件
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveCSV(string fileName)
        {
            StreamWriter sw = new StreamWriter(_dataDir + fileName + ".csv", false, Encoding.UTF8);

            for (int i = 1; i < _n_col; i++)
            {
                sw.Write("{0},", _colName[i]);
            }
            sw.Write("{0}", _colName[_n_col]);
            sw.WriteLine();

            for (int i = 1; i <= _n_item; i++)
            {
                for (int j = 1; j < _n_col; j++)
                {
                    sw.Write("{0},", _item[i].subitem[j]);
                }
                sw.Write("{0}", _item[i].subitem[_n_col]);
                sw.WriteLine();
            }

            sw.Close();
        }



        //大型水库
        private void ExtractSK(string html)
        {
            string s = Extract(ref html, "全国大型水库实时水情", "全国大江大河实时水情");

            _tableTime = Extract(ref s, "报表日期:<SPAN id=skdate>", "</SPAN>");

            _n_item = 0;


            //设定表头
            _n_col = 9;
            _colName[1] = "流域";
            _colName[2] = "行政区";
            _colName[3] = "河名";
            _colName[4] = "库名";
            _colName[5] = "库水位(米)";
            _colName[6] = "库水位涨跌";
            _colName[7] = "储水量(亿立方米)";
            _colName[8] = "入库(立方米/秒)";
            _colName[9] = "堤顶高程(米)";


            //解析数据
            while (s.IndexOf("<TR class=td-show") >= 0)
            {
                string temp = Extract(ref s, "<TR class=td-show-", "</TR>");
                _n_item++;

                _item[_n_item].colorState = int.Parse(temp[0].ToString());

                _item[_n_item].subitem[1] = Extract(ref temp, "<TD", ">", "</TD>");
                _item[_n_item].subitem[2] = Extract(ref temp, "<TD", ">", "</TD>");
                _item[_n_item].subitem[3] = Extract(ref temp, "<TD", ">", "</TD>");
                _item[_n_item].subitem[4] = Extract(ref temp, "<FONT", ">", "</FONT>");
                _item[_n_item].subitem[5] = Extract(ref temp, "<FONT", ">", "&nbsp;");
                _item[_n_item].subitem[6] = (_item[_n_item].subitem[5] != "--") ? Extract(ref temp, "<FONT", ">", "</FONT>") : "";
                _item[_n_item].subitem[7] = Extract(ref temp, "<TD", ">", "</TD>");
                _item[_n_item].subitem[8] = Extract(ref temp, "<TD", ">", "</TD>");
                _item[_n_item].subitem[9] = Extract(ref temp, "<TD", ">", "&nbsp;");

                if (_n_item == 156)
                {

                }

            }
        }


        //大江大河
        private void ExtractJH(string html)
        {
            string s = Extract(ref html, "全国大江大河实时水情", "全国重点站实时雨情");

            _tableTime = Extract(ref s, "date>", "</SPAN>");

            _n_item = 0;

            //设定表头
            _n_col = 9;
            _colName[1] = "流域";
            _colName[2] = "行政区";
            _colName[3] = "河名";
            _colName[4] = "站名";
            _colName[5] = "时间";
            _colName[6] = "水位(米)";
            _colName[7] = "水位涨跌";
            _colName[8] = "流量(立方米/秒)";
            _colName[9] = "警戒水位(米)";


            //解析数据
            while (s.IndexOf("<TR class=td-show") >= 0)
            {
                string temp = Extract(ref s, "<TR class=td-show-", "</TR>");
                _n_item++;

                _item[_n_item].colorState = int.Parse(temp[0].ToString());

                _item[_n_item].subitem[1] = Extract(ref temp, "<TD", ">", "</TD>");
                _item[_n_item].subitem[2] = Extract(ref temp, "<TD", ">", "</TD>");
                _item[_n_item].subitem[3] = Extract(ref temp, "<TD", ">", "</TD>");
                _item[_n_item].subitem[4] = Extract(ref temp, "<FONT", ">", "</FONT>");
                _item[_n_item].subitem[5] = Extract(ref temp, "<TD", ">", "</TD>");
                _item[_n_item].subitem[6] = Extract(ref temp, "<FONT", ">", "</FONT>");
                _item[_n_item].subitem[7] = (_item[_n_item].subitem[6] != "--") ? Extract(ref temp, "<FONT", ">", "</FONT>") : "";
                _item[_n_item].subitem[8] = Extract(ref temp, "<TD", ">", "</TD>");
                _item[_n_item].subitem[9] = Extract(ref temp, "<TD", ">", "&nbsp;");
            }
        }


        //重点雨水情
        private void ExtractYS(string html)
        {
            string s = Extract(ref html, "全国重点站实时雨情", "搜索</STRONG>");

            _tableTime = Extract(ref s, "date>", "</SPAN>");

            _n_item = 0;


            //设定表头
            _n_col = 7;
            _colName[1] = "流域";
            _colName[2] = "行政区";
            _colName[3] = "河名";
            _colName[4] = "站名";
            _colName[5] = "日期";
            _colName[6] = "日雨量(毫米)";
            _colName[7] = "天气";


            //解析数据
            while (s.IndexOf("<TR class=td-show") >= 0)
            {
                string temp = Extract(ref s, "<TR class=td-show-", "</TR>");
                _n_item++;

                _item[_n_item].colorState = int.Parse(temp[0].ToString());

                _item[_n_item].subitem[1] = Extract(ref temp, "<TD", ">", "</TD>");
                _item[_n_item].subitem[2] = Extract(ref temp, "<TD", ">", "</TD>");
                _item[_n_item].subitem[3] = Extract(ref temp, "<TD", ">", "</TD>");
                _item[_n_item].subitem[4] = Extract(ref temp, "<TD", ">", "</TD>");
                _item[_n_item].subitem[5] = Extract(ref temp, "<TD", ">", "</TD>");
                _item[_n_item].subitem[6] = Extract(ref temp, "<TD", ">", "</TD>");
                _item[_n_item].subitem[7] = Extract(ref temp, "<TD", ">", "</TD>");
            }
        }


        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="content"></param>
        private void WriteLog(string content, string status)
        {
            StreamWriter sw = new StreamWriter("history.log", true, Encoding.Unicode);

            sw.WriteLine("{0}\t{1}\t{2}", DateTime.Now.ToString(), content, status);

            sw.Close();
        }


        /// <summary>
        /// 记录失败记录
        /// </summary>
        /// <param name="str"></param>
        private void WriteString(string str)
        {
            StreamWriter sw = new StreamWriter("source.html", false, Encoding.Unicode);

            sw.Write(str);

            sw.Close();
        }


        private string Extract(ref string str, string start, string end)
        {
            string ans;

            str = str.Substring(str.IndexOf(start) + start.Length);
            ans = str.Substring(0, str.IndexOf(end)).Trim();
            str = str.Substring(str.IndexOf(end) + end.Length);

            return ans;
        }


        private string Extract(ref string str, string startBegin, string startEnd, string end)
        {
            string ans;

            str = str.Substring(str.IndexOf(startBegin) + startBegin.Length);
            str = str.Substring(str.IndexOf(startEnd) + startEnd.Length);
            ans = str.Substring(0, str.IndexOf(end)).Trim();
            str = str.Substring(str.IndexOf(end) + end.Length);

            return ans;
        }
    }
}