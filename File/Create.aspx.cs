using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Moo.DB;
using Moo.Authorization;
using Moo.Utility;
public partial class File_Create : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Page.DataBind();
        }
    }
    protected void validateFileLength_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = fileUpload.PostedFile.ContentLength <= 10 * 1024 * 1024;
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        if (!Permission.Check("file.create", false)) return;

        string fileName = Resources.Moo.File_UploadPath + Path.GetRandomFileName() + "." + fileUpload.FileName.Split('.').Last();
        fileUpload.SaveAs(fileName);
        int fileID;
        using (MooDB db = new MooDB())
        {
            UploadedFile file = new UploadedFile()
            {
                Name = txtName.Text,
                Path = fileName
            };
            db.UploadedFiles.AddObject(file);
            db.SaveChanges();
            fileID = file.ID;
        }

        PageUtil.Redirect("操作成功", "~/File/?id=" + fileID);
    }
}