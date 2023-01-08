using System;
using Godot;
using LMTS.Common.Abstract;
using LMTS.Common.Constants;
using LMTS.Common.Models.World;

namespace LMTS.Common.Utilities;

public static class NodeUtilities
{
    public static void SetWorldObjectMeta(Node node, IWorldObject worldObject)
    {
        if (worldObject is WorldNavigationJunction j)
        {
            node.SetMeta(MetadataConstants.MetaTypeKey, MetadataConstants.MetaTypeNavigationJunction);
        }
        else if (worldObject is WorldNavigationPath p)
        {
            node.SetMeta(MetadataConstants.MetaTypeKey, MetadataConstants.MetaTypeNavigationPath);
        }
        else
        {
            throw new ArgumentOutOfRangeException();
        }

        node.SetMeta(MetadataConstants.MetaTypeIdKey, worldObject.Identifier.ToString());
    }
}