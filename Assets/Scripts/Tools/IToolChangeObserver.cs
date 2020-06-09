namespace Assets.Scripts.Tools
{
  public interface IToolChangeObserver
  {
    void OnToolsChanged(ITool newTool);
  }
}
