using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoApp1
{
    public partial class Form1 : Form
    {
        public static string token;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FormLogin login = new FormLogin();
            login.ShowDialog();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            SendSMSReq req = new SendSMSReq();
            req.phoneNum = "189189189";
            req.msg = "hello world!";
            string jsonReq = JsonConvert.SerializeObject(req);

            using (var http = new HttpClient())
            using (var content = new StringContent(jsonReq,Encoding.UTF8,"application/json"))
            {
                http.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(token);
                var resp = await http.PostAsync("http://127.0.0.1:5000/MsgService/SMS/Send_MI", content);
                if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show("发送错误，状态码" + resp.StatusCode);
                }
                else
                {
                    MessageBox.Show("发送成功");
                }
            }
        }
    }
}
