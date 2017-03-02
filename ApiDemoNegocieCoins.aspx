<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApiDemoNegocieCoins.aspx.cs" Inherits="TradeApi_WebDemo.WebForm1" Async="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

        <h1>NEGOCIECOINS - API - DEMO</h1>

        <div style="float:left; width:49%;">
            <fieldset>
                URL: <asp:Literal ID="TextBoxURL" runat="server"></asp:Literal><br />
                App ID: <asp:TextBox ID="TextBoxAppId" runat="server" Width="500px"></asp:TextBox><br />
                App Key: <asp:TextBox ID="TextBoxAppKey" runat="server" Width="500px"></asp:TextBox><br />
            </fieldset>

            <fieldset>
                <h3>GET "user/balance"</h3>
                <asp:Button ID="ButtonSaldo" runat="server" Text="Balance" OnClick="ButtonSaldo_Click" />
            </fieldset>

            <fieldset>
                <h3>POST "user/orders"</h3>

                Page: 
                <asp:TextBox ID="TextBoxPage" runat="server" Width="20px"></asp:TextBox>      

                PageSize:
                <asp:TextBox ID="TextBoxPageSize" runat="server" Width="20px"></asp:TextBox>      
                <br />
                <asp:RadioButton ID="RadioButtonMoedaBTC" runat="server" GroupName="moeda" Text="BTC"/>
                <asp:RadioButton ID="RadioButtonMoedaLTC" runat="server" GroupName="moeda" Text="LTC"/>
                <br />
                <asp:RadioButton ID="RadioButtonTipoComprar" runat="server" GroupName="tipo" Text="Comprar"/>
                <asp:RadioButton ID="RadioButtonTipoVender" runat="server" GroupName="tipo" Text="Vender"/>
                <br />
                <asp:RadioButton ID="RadioButtonStatusPendente" runat="server" GroupName="status" Text="Pendente"/>
                <asp:RadioButton ID="RadioButtonStatusParcialmente" runat="server" GroupName="status" Text="Parcialmente"/>
                <asp:RadioButton ID="RadioButtonStatusCancelado" runat="server" GroupName="status" Text="Cancelado"/>
                <asp:RadioButton ID="RadioButtonStatusExecutado" runat="server" GroupName="status" Text="Executado"/>

                <asp:Button ID="ButtonOrderList" runat="server" Text="Order List" OnClick="ButtonOrderList_Click" />
            </fieldset>

            <fieldset>
                <h3>GET "user/order/{orderId}"</h3>
                orderId: <asp:TextBox ID="TextBoxOrderID" runat="server" Width="80px"></asp:TextBox>        
                <asp:Button ID="ButtonOrderDetails" runat="server" Text="Order Details" OnClick="ButtonOrderDetails_Click" />
            </fieldset>

            <fieldset>
                <h3>POST "user/order"</h3>
                
                <asp:RadioButton ID="RadioButtonMoedaBTCSend" runat="server" GroupName="moeda2" Text="BTC"/>
                <asp:RadioButton ID="RadioButtonMoedaLTCSend" runat="server" GroupName="moeda2" Text="LTC"/>
                <br />
                <asp:RadioButton ID="RadioButtonTipoComprarSend" runat="server" GroupName="tipo2" Text="Comprar"/>
                <asp:RadioButton ID="RadioButtonTipoVenderSend" runat="server" GroupName="tipo2" Text="Vender"/>
                <br />
                Valor: <asp:TextBox ID="TextBoxValor" runat="server"></asp:TextBox>  
                <br /> 
                Volume: <asp:TextBox ID="TextBoxVolume" runat="server"></asp:TextBox>   
                <br />
                <asp:Button ID="ButtonSendOrder" runat="server" Text="Send Order" OnClick="ButtonSendOrder_Click" />
            </fieldset>

            <fieldset>
                <h3>DELETE "user/order/{orderId}"</h3>
                orderId: <asp:TextBox ID="TextBoxOrderIDCancel" runat="server" Width="80px"></asp:TextBox>        
                <asp:Button ID="ButtonCancelOrder" runat="server" Text="Cancel Order" OnClick="ButtonCancelOrder_Click" />
            </fieldset>

        </div>
        <div style="float:right; width:49%; height:700px;">

            <div style="height:350px;">
                Request:<br />
                <asp:TextBox ID="TextBoxRequest" runat="server" TextMode="MultiLine" Width="100%" Height="100%" ReadOnly="true"></asp:TextBox>
            </div>
            
            <div style="height:350px; margin-top:50px;">
                Response:<br />
                <asp:TextBox ID="TextBoxResponse" runat="server" TextMode="MultiLine" Width="100%" Height="100%" ReadOnly="true"></asp:TextBox>
            </div>

        </div>
    </form>
</body>
</html>
