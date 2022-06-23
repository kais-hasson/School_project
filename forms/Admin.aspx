<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="School_project.Controllers.Admin" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>admin_page</title>
</head>
<body>
    <form id="form1" runat="server">
        أهلا بك في صفحة تسجيل الدخول :
        <p>اسم المستخدم
            <asp:TextBox ID="user_name" runat="server" Text=""></asp:TextBox>
        </p>
        <p>كلمة المرور
            <asp:TextBox ID="password" runat="server" Text=""></asp:TextBox>
        </p>
        <p>
            <asp:Button ID="Login_Button" runat="server" Text="تسجيل الدخول" OnClick="Login_Button_Click" />
        </p>
       
    </form>
     </body>
</html>
