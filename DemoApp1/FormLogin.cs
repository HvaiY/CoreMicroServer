using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoApp1
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            LoginReq req = new LoginReq();
            req.username = txtUserName.Text;
            req.password = txtPassword.Text;
            string jsonReq =  JsonConvert.SerializeObject(req);
            using (HttpClient http = new HttpClient())
            using (var content = new StringContent(jsonReq, Encoding.UTF8, "application/json"))
            {
                var resp = await http.PostAsync("http://127.0.0.1:5000/LoginService/Login", content);
                if(resp.IsSuccessStatusCode==false)
                {
                    MessageBox.Show("error"+resp.StatusCode);
                    return;
                }
                string jsonResp = await resp.Content.ReadAsStringAsync();
                dynamic respObj =  JsonConvert.DeserializeObject<dynamic>(jsonResp);
                string access_token = respObj.access_token;
                string token_type = respObj.token_type;
                // MessageBox.Show(token_type+" "+access_token);
                Form1.token = token_type + " " + access_token;
                this.Close();
            }
        }
    }

    class LoginReq
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
