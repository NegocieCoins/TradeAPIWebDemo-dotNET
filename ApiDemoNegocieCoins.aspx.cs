using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Net;
using System.Text;
using System.IO;
using RestSharp;
using Newtonsoft.Json;
using RestSharp.Deserializers;

namespace TradeApi_WebDemo
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        CustomDelegatingHandler customDelegatingHandler = new CustomDelegatingHandler();
        HttpClient client = new HttpClient();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TextBoxURL.Text = ConfigurationManager.AppSettings["WebApiUrl"].ToString();

                TextBoxAppId.Text = "";
                TextBoxAppKey.Text = "";

                TextBoxPage.Text = "1";
                TextBoxPageSize.Text = "2";
                RadioButtonStatusPendente.Checked = true;
                RadioButtonTipoComprar.Checked = true;
                RadioButtonMoedaBTC.Checked = true;

                RadioButtonMoedaBTCSend.Checked = true;
                RadioButtonTipoComprarSend.Checked = true;

            }

            customDelegatingHandler = new CustomDelegatingHandler(TextBoxAppId.Text, TextBoxAppKey.Text);
            client = HttpClientFactory.Create(customDelegatingHandler);
        }

        protected async void ButtonSaldo_Click(object sender, EventArgs e)
        {
            string requestUri = TextBoxURL.Text + "user/balance";

            HttpResponseMessage response = await client.GetAsync(requestUri);

            TextBoxRequest.Text = "GET:" + requestUri;

            await Task.Run(() => DisplayResponse(response));
        }

        protected async void ButtonOrderList_Click(object sender, EventArgs e)
        {
            int page = int.Parse(TextBoxPage.Text);
            int pageSize = int.Parse(TextBoxPageSize.Text);
            string pair = RadioButtonMoedaBTC.Checked ? "BRLBTC" : "BRLLTC";
            string type = RadioButtonTipoComprar.Checked ? "buy" : "sell";

            string status = "pending";
            if (RadioButtonStatusCancelado.Checked)
                status = "cancelled";
            else if (RadioButtonStatusExecutado.Checked)
                status = "filled";
            else if (RadioButtonStatusPendente.Checked)
                status = "pending";
            else if (RadioButtonStatusParcialmente.Checked)
                status = "partially filled";
            
            //ORDER LIST
            var orderListRequest = new
            {
                page = page,
                pageSize = pageSize,
                pair = pair,  //btc_brl or ltc_brl.
                type = type,      // buy or sell
                status = status
            };

            string requestUri = TextBoxURL.Text + "user/orders";
            HttpResponseMessage response = await client.PostAsJsonAsync(requestUri, orderListRequest);

            TextBoxRequest.Text = "POST:" + requestUri + "\n\r";
            TextBoxRequest.Text += orderListRequest.ToString(); 
            
            TextBoxResponse.Text = JsonConvert.SerializeObject(orderListRequest).ToString();
            
            await Task.Run(() => DisplayResponse(response));
        }

        protected async void ButtonOrderDetails_Click(object sender, EventArgs e)
        {
            var id = TextBoxOrderID.Text == "" ? 0 : int.Parse(TextBoxOrderID.Text);

            string requestUri = TextBoxURL.Text + string.Format("user/order/{0}", id);

            HttpResponseMessage response = await client.GetAsync(requestUri);

            TextBoxRequest.Text = "GET:" + requestUri;

            await Task.Run(() => DisplayResponse(response));
        }

        protected async void ButtonSendOrder_Click(object sender, EventArgs e)
        {
            var pair = RadioButtonMoedaBTCSend.Checked ? "BRLBTC" : "BRLLTC";
            var type = RadioButtonTipoComprar.Checked ? "buy" : "sell";
            var price = decimal.Parse(TextBoxValor.Text);
            var volume = decimal.Parse(TextBoxVolume.Text);

            var sendOrderRequest = new
            {
                pair = pair, //btc_brl or ltc_brl.
                type = type, // buy or sell
                price = price,
                volume = volume
            };

            string requestUri = TextBoxURL.Text + "user/order";
            HttpResponseMessage response = await client.PostAsJsonAsync(requestUri, sendOrderRequest);

            TextBoxRequest.Text = "POST:" + requestUri + "\n\r";
            TextBoxRequest.Text += sendOrderRequest.ToString();

            await Task.Run(() => DisplayResponse(response));
        }

        protected async void ButtonCancelOrder_Click(object sender, EventArgs e)
        {
            var id = (TextBoxOrderIDCancel.Text == "" ? 0 : int.Parse(TextBoxOrderIDCancel.Text));

            string requestUri = TextBoxURL.Text + string.Format("user/order/{0}", id);

            HttpResponseMessage response = await client.DeleteAsync(requestUri);

            TextBoxRequest.Text = "DEL:" + requestUri;

            await Task.Run(() => DisplayResponse(response));
        }

        async void DisplayResponse(HttpResponseMessage response)
        {
            string responseBody = await response.Content.ReadAsStringAsync();

            TextBoxResponse.Text = "";
            if (!string.IsNullOrEmpty(responseBody))
            { 
                TextBoxResponse.Text = JsonConvert.DeserializeObject(responseBody).ToString();
                TextBoxResponse.Text += string.Format("\n\rResponse:\n\r{0}", "{" + response.ToString() + "}");
            }
            
        }
        
    }
}