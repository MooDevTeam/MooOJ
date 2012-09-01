<%@ WebHandler Language="C#" Class="Special_Captcha" %>

using System;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using Moo.Utility;

public class Special_Captcha : IHttpHandler, IRequiresSessionState
{
    static Bitmap sampleBitmap = new Bitmap(1, 1);

    Color RandomColor()
    {
        return Color.FromArgb(Rand.RAND.Next(256), Rand.RAND.Next(256), Rand.RAND.Next(256));
    }

    Color MakeBright(Color color)
    {
        return Color.FromArgb(color.R / 2 + 127, color.G / 2 + 127, color.B / 2 + 127);
    }

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "image/png";
        string message = Encoding.Unicode.GetString(Converter.Decrypt(Convert.FromBase64String(context.Request["message"])));
        string[] splited = message.Split(',');
        string text = splited[0];
        string clientID = splited[1];
        if ((string)context.Session["MooCaptchaGetImage" + clientID] != "allowed")
        {
            using (Bitmap warning = new Bitmap(200, 100))
            {
                using (Graphics g = Graphics.FromImage(warning))
                {
                    g.Clear(Color.Transparent);
                    g.DrawString("请更换验证码", new Font(FontFamily.GenericSerif, 20), Brushes.Black, PointF.Empty);
                }
                warning.Save(context.Response.OutputStream, ImageFormat.Png);
                return;
            }
        }
        context.Session.Remove("MooCaptchaGetImage" + clientID);

        using (Bitmap allText = GenerateBitmap(text))
        {
            using (Bitmap final = new Bitmap(200, 100))
            {
                using (Graphics g = Graphics.FromImage(final))
                {
                    g.Clear(Color.Transparent);
                    g.DrawImage(allText, new Point[] { new Point(0, 0), new Point(final.Width - 1, 0), new Point(0, final.Height - 1) });
                    g.DrawBezier(new Pen(Color.Black, 7), new Point(Rand.RAND.Next(final.Width), Rand.RAND.Next(final.Height)), new Point(Rand.RAND.Next(final.Width), Rand.RAND.Next(final.Height)), new Point(Rand.RAND.Next(final.Width), Rand.RAND.Next(final.Height)), new Point(Rand.RAND.Next(final.Width), Rand.RAND.Next(final.Height)));
                    g.DrawBezier(new Pen(Color.Black, 7), new Point(Rand.RAND.Next(final.Width), Rand.RAND.Next(final.Height)), new Point(Rand.RAND.Next(final.Width), Rand.RAND.Next(final.Height)), new Point(Rand.RAND.Next(final.Width), Rand.RAND.Next(final.Height)), new Point(Rand.RAND.Next(final.Width), Rand.RAND.Next(final.Height)));
                }

                Transform1(final);
                Transform2(final);
                Transform3(final);
                //Transform4(final);
                final.Save(context.Response.OutputStream, ImageFormat.Png);
            }
        }
    }

    Bitmap GenerateBitmap(string text)
    {
        Bitmap[] single = new Bitmap[text.Length];
        for (int i = 0; i < text.Length; i++)
        {
            single[i] = GenerateSingelCharacter(text[i]);
        }
        Size size = new Size(single.Sum(b => b.Width), single.Max(b => b.Height));
        Bitmap result = new Bitmap(size.Width, size.Height);
        using (Graphics g = Graphics.FromImage(result))
        {
            int xOffset = 0;
            foreach (Bitmap b in single)
            {
                g.DrawImage(b, new Point(xOffset, Rand.RAND.Next(size.Height - b.Height)));
                xOffset += b.Width;
                b.Dispose();
            }
        }
        return result;
    }

    Bitmap GenerateSingelCharacter(char ch)
    {
        Font font = new Font(FontFamily.GenericSansSerif, Rand.RAND.Next(60 - 30) + 30);
        Size size;
        using (Graphics g = Graphics.FromImage(sampleBitmap))
        {
            size = g.MeasureString("" + ch, font).ToSize();
        }
        Bitmap result = new Bitmap(size.Width, size.Height);
        using (Graphics g = Graphics.FromImage(result))
        {
            g.Clear(Color.Transparent);
            g.DrawString("" + ch, font, Brushes.Black, 0, 0, StringFormat.GenericTypographic);
            int? minX = null, minY = null, maxX = null, maxY = null;
            for (int x = 0; x < result.Width; x++)
            {
                for (int y = 0; y < result.Height; y++)
                {
                    if (result.GetPixel(x, y).A > 250)
                    {
                        minX = minX == null ? x : Math.Min(x, (int)minX);
                        minY = minY == null ? y : Math.Min(y, (int)minY);
                        maxX = maxX == null ? x : Math.Max(x, (int)maxX);
                        maxY = maxY == null ? y : Math.Max(y, (int)maxY);
                    }
                }
            }
            minX -= 3;
            minY -= 3;
            maxX += 3;
            maxY += 3;

            maxX++;
            maxY++;

            Bitmap old = result;
            result = new Bitmap((int)(maxX - minX), (int)(maxY - minY));
            using (Graphics newG = Graphics.FromImage(result))
            {
                newG.Clear(Color.Transparent);
                newG.DrawImage(old, new Point(-(int)minX, -(int)minY));
            }
            old.Dispose();
        }
        using (Bitmap cloned = (Bitmap)result.Clone())
        {
            using (Graphics g = Graphics.FromImage(result))
            {
                g.Clear(Color.Transparent);
                Point ul = new Point(Rand.RAND.Next(result.Width / 4), Rand.RAND.Next(result.Height / 4));
                Point ur = new Point(result.Width - Rand.RAND.Next(result.Width / 4), Rand.RAND.Next(result.Height / 4));
                Point dl = new Point(Rand.RAND.Next(result.Width / 4), result.Height - Rand.RAND.Next(result.Height / 4));
                g.DrawImage(cloned, new Point[] { ul, ur, dl });
            }
        }
        return result;
    }
    /*
    Point RandomPoint()
    {
        return new Point(Rand.RAND.Next(size.Width), Rand.RAND.Next(size.Width));
    }
    */


    void Transform1(Bitmap bitmap)
    {
        for (int x = 0; x < bitmap.Width; x++)
        {
            for (int y = 0; y < bitmap.Height; y++)
            {
                if (bitmap.GetPixel(x, y).A > 250)
                {
                    bitmap.SetPixel(x, y, RandomColor());
                }
            }
        }
    }

    void Transform2(Bitmap bitmap)
    {
        using (Bitmap cloned = (Bitmap)bitmap.Clone())
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    int dx = Rand.RandomSign() * Rand.RAND.Next(3);
                    int dy = Rand.RandomSign() * Rand.RAND.Next(3);

                    int newX = x + dx, newY = y + dy;
                    newX = Math.Max(Math.Min(newX, bitmap.Width - 1), 0);
                    newY = Math.Max(Math.Min(newY, bitmap.Height - 1), 0);

                    bitmap.SetPixel(x, y, cloned.GetPixel(newX, newY));
                }
            }
        }
    }

    void Transform3(Bitmap bitmap)
    {
        for (int i = 0; i < 5000; i++)
        {
            bitmap.SetPixel(Rand.RAND.Next(bitmap.Width), Rand.RAND.Next(bitmap.Height), RandomColor());
        }
    }

    void Transform4(Bitmap bitmap)
    {
        using (Bitmap cloned = (Bitmap)bitmap.Clone())
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(MakeBright(RandomColor()));
                g.DrawImage(cloned, Point.Empty);
            }
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}