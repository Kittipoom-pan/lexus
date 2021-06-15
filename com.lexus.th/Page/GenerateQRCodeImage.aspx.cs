using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CenterModuleCode
{
    public partial class GenerateQRCodeImage : System.Web.UI.Page
    {
        //lib qrcode PM> Install-Package QRCoder
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["d"] != null)
            {
                string strData = Request.QueryString["d"];
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(strData, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20); 
                Bitmap bmpCrop = new Bitmap(qrCodeImage.Clone(new Rectangle(70, 70, 440, 440), qrCodeImage.PixelFormat));
                qrCodeImage.Dispose();

                MemoryStream MemStream = new MemoryStream();
                Response.ContentType = "image/jpeg";
                bmpCrop.Save(MemStream, ImageFormat.Jpeg);

                MemStream.WriteTo(Response.OutputStream);
            }
        }
    }
}