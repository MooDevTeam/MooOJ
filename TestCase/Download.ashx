<%@ WebHandler Language="C#" Class="TestCase_Download" %>

using System;
using System.Web;
using System.Linq;
using Moo.DB;
using Moo.Utility;
using Moo.Authorization;
public class TestCase_Download : IHttpHandler
{
    const int BLOCK_SIZE = 1024 * 400;
    TestCase testCase;
    TranditionalTestCase asTranditional;
    SpecialJudgedTestCase asSpecialJudged;
    HttpRequest Request;
    HttpResponse Response;

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    public void ProcessRequest(HttpContext context)
    {
        Request = context.Request;
        Response = context.Response;

        using (MooDB db = new MooDB())
        {
            if (Request["id"] != null)
            {
                int testCaseID = int.Parse(Request["id"]);
                testCase = (from t in db.TestCases
                            where t.ID == testCaseID
                            select t).SingleOrDefault<TestCase>();
            }
            if (testCase == null)
            {
                PageUtil.Redirect(Resources.Moo.FoundNothing, "~/");
                return;
            }

            if (testCase.Problem.TestCaseHidden)
            {
                if (!Permission.Check("testcase.hidden.read", false)) return;
            }
            else
            {
                if (!Permission.Check("testcase.read", true)) return;
            }

            if (testCase is TranditionalTestCase)
            {
                asTranditional = testCase as TranditionalTestCase;
                switch (Request["field"])
                {
                    case "Input":
                        Download(asTranditional.Input);
                        break;
                    case "Answer":
                        Download(asTranditional.Answer);
                        break;
                    default:
                        PageUtil.Redirect("未知的字段", "~/");
                        return;
                }
            }
            else if (testCase is SpecialJudgedTestCase)
            {
                asSpecialJudged = testCase as SpecialJudgedTestCase;
                switch (Request["field"])
                {
                    case "Input":
                        Download(asSpecialJudged.Input);
                        break;
                    case "Answer":
                        Download(asSpecialJudged.Answer);
                        break;
                    default:
                        PageUtil.Redirect("未知的字段", "~/");
                        return;
                }
            }
            else
            {
                PageUtil.Redirect("未知的测试数据类型", "~/");
                return;
            }
        }
    }

    void Download(byte[] toDownload)
    {
        int start = 0;
        int length = toDownload.Length;

        if (Request.Headers["Range"] != null)
        {
            start = int.Parse(Request.Headers["Range"].Replace("bytes=", "").Split('-')[0]);
            length -= start;

            Response.StatusCode = 206;
            Response.AddHeader("Content-Range", "bytes " + start + "-" + (toDownload.Length - 1) + "/" + toDownload.Length);
        }

        Response.AddHeader("Content-Length", length.ToString());
        Response.ContentType = "application/octet-stream";
        Response.AddHeader("Content-Disposition", "attachment; filename=TestCase" + testCase.ID + "." + Request["field"]);

        while (length > 0)
        {
            if (!Response.IsClientConnected) return;

            int toWrite = Math.Min(length, BLOCK_SIZE);
            Response.OutputStream.Write(toDownload, start, toWrite);
            start += toWrite;
            length -= toWrite;

            Response.Flush();
        }
    }

}