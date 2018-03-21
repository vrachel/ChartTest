using SciChart.Charting.Visuals;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Test
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
          

            SciChart.Charting.Visuals.SciChartSurface.SetRuntimeLicenseKey(@"<LicenseContract>
              <Customer>Israel Aerospace Industries</Customer>
              <OrderId>ABT180118-9841-52112</OrderId>
              <LicenseCount>1</LicenseCount>
              <IsTrialLicense>false</IsTrialLicense>
              <SupportExpires>04/18/2018 00:00:00</SupportExpires>
              <ProductCode>SC-WPF-2D-PRO</ProductCode>
              <KeyCode>lwAAAAEAAAAOxetHO5DTAXsAQ3VzdG9tZXI9SXNyYWVsIEFlcm9zcGFjZSBJbmR1c3RyaWVzO09yZGVySWQ9QUJUMTgwMTE4LTk4NDEtNTIxMTI7U3Vic2NyaXB0aW9uVmFsaWRUbz0xOC1BcHItMjAxODtQcm9kdWN0Q29kZT1TQy1XUEYtMkQtUFJPcLISmv5AzjFVDDc/SCVSsNo7DoM/QsoMkQDFp3YBVVkT53lEe9RcCroIjvT6do5U</KeyCode>
            </LicenseContract>");
        }

    }
}
