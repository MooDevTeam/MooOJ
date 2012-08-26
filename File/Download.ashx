<%@ WebHandler Language="C#" Class="File_Download" %>

using System;
using System.Linq;
using System.Web;
using System.IO;
using Moo.DB;
using Moo.Utility;
using Moo.Authorization;
public class File_Download : IHttpHandler {
    static long BLOCK_SIZE = 1024 * 400;
    
    public void ProcessRequest (HttpContext context) {
        HttpRequest Request = context.Request;
        HttpResponse Response = context.Response;
        string path;
        int fileID;
        
        if (!Permission.Check("file.read", true)) return;
        using (MooDB db = new MooDB())
        {
            UploadedFile file=null;
            if (Request["id"] != null)
            {
                fileID = int.Parse(Request["id"]);
                file = (from f in db.UploadedFiles
                        where f.ID == fileID
                        select f).SingleOrDefault<UploadedFile>();
            }

            if (file == null)
            {
                PageUtil.Redirect(Resources.Moo.FoundNothing, "~/");
                return;
            }

            path = file.Path;
            fileID = file.ID;
        }

        Download(Request, Response, path,fileID);
    }

    void Download(HttpRequest Request, HttpResponse Response,string path,int fileID)
    {
        FileInfo fileInfo = new FileInfo(path);
        long start = 0;
        long length = fileInfo.Length;

        if (Request.Headers["Range"] != null)
        {
            start = int.Parse(Request.Headers["Range"].Replace("bytes=", "").Split('-')[0]);
            length -= start;

            Response.StatusCode = 206;
            Response.AddHeader("Content-Range", "bytes " + start + "-" + (fileInfo.Length - 1) + "/" + fileInfo.Length);
        }

        Response.AddHeader("Content-Length", length.ToString());
        Response.ContentType = "application/octet-stream";
        Response.AddHeader("Content-Disposition", "attachment; filename="+fileInfo.Name);

        using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            stream.Position = start;
            byte[] buf=new byte[BLOCK_SIZE];
            while (length > 0)
            {
                if (!Response.IsClientConnected) return;
                
                int currentRead = stream.Read(buf,0,buf.Length);
                Response.OutputStream.Write(buf, 0, currentRead);
                
                start += currentRead;
                length -= currentRead;

                Response.Flush();
            }
        }
    }
 
    public bool IsReusable {
        get {
            return true;
        }
    }

}