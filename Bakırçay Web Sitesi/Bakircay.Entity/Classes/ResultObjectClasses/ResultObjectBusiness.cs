using Bakircay.Entity.Enums;


namespace Bakircay.Entity.Classes.ResultObjectClasses
{
  public class ResultObjectBusiness<T>
  {
    public ResultStatus ResultStatus { get; set; }
    public string Message { get; set; }
    public string Url { get; set; }
    public T ResultObject { get; set; }
    public ICollection<T> ResultObjects { get; set; }

  }
}
