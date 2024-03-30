namespace LMTS.Common.Enums;

public enum WorldObjectState
{

    /// <summary>
    /// indicates that the object should be rendered as a ghost and should not be synchronized/saved
    /// </summary>
    //PreviewGhost,
    /// <summary>
    /// indicates that the object should be rendered as an invalid object ghost and should not be synchronized/saved
    /// </summary>
    //PreviewInvalid,
    /// <summary>
    /// indicates that the object is placed on this client, but is not yet confirmed by other clients or the server, and should not be synchronized/saved
    /// </summary>
    //Preview,
    /// <summary>
    /// indicates that the object is placed and should be synchronized/saved
    /// </summary>
    Finalized
}