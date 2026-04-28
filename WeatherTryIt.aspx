<%@ Page Title="Weather TryIt" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WeatherTryIt.aspx.cs" Inherits="Assignment6.WeatherTryIt" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <section class="info-card tryit-shell">
        <h2>Weather Service TryIt</h2>
        <p>
            Enter a ZIP code to validate it and view a five-day forecast.
        </p>

        <div class="form-group">
            <label for="<%= txtWeatherZip.ClientID %>">ZIP Code</label>
            <asp:TextBox ID="txtWeatherZip" runat="server" CssClass="form-control" placeholder="Enter a ZIP code" />
        </div>

        <asp:Button ID="btnGetForecast" runat="server" CssClass="btn btn-primary" Text="Get 5-Day Forecast" OnClick="btnGetForecast_Click" />

        <div class="result-panel">
            <h3>Search Status</h3>
            <p><strong>Normalized ZIP:</strong> <asp:Label ID="lblWeatherNormalizedZip" runat="server" Text="" /></p>
            <p><strong>Status:</strong> <asp:Label ID="lblWeatherStatus" runat="server" Text="" /></p>
            <p><strong>Last ZIP:</strong> <asp:Label ID="lblLastZipCookie" runat="server" Text="" /></p>
        </div>

        <div class="result-panel">
            <h3>Forecast Results</h3>
            <asp:BulletedList ID="bltForecastResults" runat="server" BulletStyle="Disc" />
        </div>
    </section>
</asp:Content>
