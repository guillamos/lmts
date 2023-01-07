using Godot;

namespace LMTS.Presentation.Utilities;

public static class DrawingUtilities
{
    public static int RenderQuad(SurfaceTool st, int currentIndex, Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, bool invert = false)
    {
        st.SetUv(new Vector2(0, 1));
        if (invert)
        {
            st.AddVertex(v4);
        }
        else
        {
            st.AddVertex(v1);
        }

        st.SetUv(new Vector2(0, 0));
        if (invert)
        {
            st.AddVertex(v3);
        }
        else
        {
            st.AddVertex(v2);
        }

        st.SetUv(new Vector2(1, 0));
        if (invert)
        {
            st.AddVertex(v2);
        }
        else
        {
            st.AddVertex(v3);
        }
        
        st.SetUv(new Vector2(1, 1));
        if (invert)
        {
            st.AddVertex(v1);
        }
        else
        {
            st.AddVertex(v4);
        }
        
        st.AddIndex(currentIndex + 0);
        st.AddIndex(currentIndex + 1);
        st.AddIndex(currentIndex + 2);

        st.AddIndex(currentIndex + 0);
        st.AddIndex(currentIndex + 2);
        st.AddIndex(currentIndex + 3);
        return currentIndex + 4;
    }
}