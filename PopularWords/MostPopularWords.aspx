<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MostPopularWords.aspx.cs" Inherits="PopularWords.MostPopularWords" %>
<link href="style.css" rel="stylesheet" />

<!-- UI for the user -->
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>Popular Words</title>
    </head>
    <body>
        <form id="form1" runat="server">
        <div class="chosenUrl">
            <br />
            <h2>
                Please insert a url in the textbox below:
            </h2>
            <h4>
                <strong>
                    <u>URL:</u>
                </strong>
                <asp:TextBox ID="urlInput" placeholder="Insert URL" Width="900px" runat="server"></asp:TextBox>
            </h4>
            <br />
            <asp:Button CssClass="enterButton" runat="server" Text="ENTER" onclick="ProcessContentFromUrl" /> 
            <asp:Button CssClass="resetButton" runat="server" Text="CLEAR" onclick="ClearScreen" /> 
        </div>
        <div class="resultsBlock" runat="server">
            <br /><br /><br />
            <h3>
                The 10 most popular words:
            </h3>
            <div class="top10" id="top10" runat="server">
            </div>
         </div>
        </form>
    </body>
</html>
