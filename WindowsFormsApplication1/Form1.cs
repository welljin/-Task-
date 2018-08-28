using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 按钮事件，异步事件，获取结果，非UI堵塞
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button1_Click(object sender, EventArgs e)
        {
            string getstr=await GetHttpPostStringAsync(this.textBox1.Text);
            this.textBox2.Text=getstr;
        }

        /// <summary>
        /// 异步请求，返回请求结果
        /// </summary>
        /// <param name="Url">请求地址</param>
        /// <returns>参数列表</returns>
        public async Task<string> GetHttpPostStringAsync(string Url)
        {
            return await Task.Run<string>(() =>
            {
                return  HttpPost(Url);
            });

            //Invoke(new Action(() => { }));
            //Action<int> act = new Action<int>((i) => {  i=i + 1; });  
            //Func<int, string> func = new Func<int, string>((i) =>
            //    {
            //        return (i + 1).ToString();
            //    });
        }
    
        /// <summary>
        /// HTTP POST请求
        /// </summary>
        /// <param name="Url">URL地址</param>
        /// <param name="postDataStr">参数字符串</param>
        /// <returns>返回结果</returns>
        public string HttpPost(string Url, string postDataStr = "")
        {
            string responseData = "";
            HttpWebResponse response;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postDataStr.Length;
            request.Timeout = 1000;

            try
            {
                byte[] bs = Encoding.ASCII.GetBytes(postDataStr);
                Stream reqStream = request.GetRequestStream();

                reqStream.Write(bs, 0, bs.Length);
                reqStream.Close();
                response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                responseData = reader.ReadToEnd().ToString();
                reader.Close();
                request.Abort();
                response.Close();
                
            }
            catch (Exception ee)
            {
                responseData = ee.ToString();
            }
            return responseData;
        }

        /// <summary>
        /// 按钮事件。请求结果。UI堵塞
        /// </summary>
        /// <param name="sender">按钮</param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = HttpPost(this.textBox1.Text);
        }

    }

}
