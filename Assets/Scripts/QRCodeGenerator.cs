using UnityEngine;
using UnityEngine.UI;

using System.Drawing;
using System.IO;
using ZXing.QrCode;
using ZXing;
using ZXing;
using ZXing.QrCode;


public class QRCodeGenerator : MonoBehaviour
{
    private WebCamTexture camTexture;
    private Rect screenRect;
    public RawImage qrCODE;
    public static QRCodeGenerator _Instance;

    private void Awake()
    {
        _Instance = this;
    }
    private static Color32[] Encode(string textForEncoding, int width, int height)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }

    public Texture2D generateQR(string text)
    {
        var encoded = new Texture2D(512, 512);
        var color32 = Encode(text, encoded.width, encoded.height);
        encoded.SetPixels32(color32);
        encoded.Apply();
        return encoded;
    }

    public void GenerateQR(string address)
    {
        Texture2D myQR = generateQR(address);
        qrCODE.texture = myQR;
    }
    public void Start()
    {
       
    }
}
