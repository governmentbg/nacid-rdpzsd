namespace Logs.Interfaces
{
    public interface IActionWorkflowOperation
    {
        string SearchText { get; }
        string GetText { get; }
        string CreateText { get; }
        string UpdateText { get; }
        string DeleteText { get; }
        
        string ExcelText { get; }
    }
}
