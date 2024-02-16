using System.Text;

namespace ScreenAimService;

public static class ExceptionExensions
{
    public static string ToMessageAndCompleteStacktrace(this Exception exception)
    {
        var e = exception;
        var s = new StringBuilder();
        while (e != null)
        {
            s.AppendLine();
            s.AppendLine("Exception type: " + e.GetType().FullName);
            s.AppendLine("Message: " + e.Message);
            s.AppendLine("Stacktrace: ");
            s.AppendLine(e.StackTrace);
            e = e.InnerException;
        }
        return s.ToString();
    }
}