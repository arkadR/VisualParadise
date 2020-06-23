namespace Assets.Scripts.Common
{
  public static class Constants
  {
    public const float c_defaultMovementSpeed = 12f;
    public const float c_defaultMouseSensitivity = 100f;
    public static readonly string PhysicalNodeTag = "PHYSICAL_NODE_TAG";
    public static readonly string PhysicalEdgeTag = "PHYSICAL_EDGE_TAG";
    public static readonly string CustomLineEndingTag = "CUSTOM_LINE_ENDING";
    public static readonly string GraphFilePathKey = "graphFilePath";
    public static readonly string GraphFolder = "data/graphs/";

    public static readonly string PlayerMovementModeKey = "PlayerMovementMode";
    public static readonly string MouseSensitivityKey = "MouseSensitivityKey";
    public static readonly string MovementSpeedKey = "MovementSpeedKey";

    public static readonly string GameScene = "GameScene";
    public static readonly string MainMenuScene = "MainMenu";

    public static readonly string ColliderGameObjectName = "Collider";
  }
}
