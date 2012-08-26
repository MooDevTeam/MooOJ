using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_DateTime : System.Web.UI.UserControl
{
    public DateTime Value
    {
        get
        {
            return dateTimeCalendar.SelectedDate.AddHours(int.Parse(txtHour.Text)).AddMinutes(int.Parse(txtMinute.Text)).AddSeconds(int.Parse(txtSecond.Text));
        }
        set
        {
            dateTimeCalendar.SelectedDate = value.Date;
            txtHour.Text = value.Hour.ToString();
            txtMinute.Text = value.Minute.ToString();
            txtSecond.Text = value.Second.ToString();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void ValidateCalendarSelected(object source, ServerValidateEventArgs args)
    {
        args.IsValid = dateTimeCalendar.SelectedDates.Count > 0;
    }
}