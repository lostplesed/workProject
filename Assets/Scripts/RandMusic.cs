using System.Collections.Generic;

public class RandMusic
{
    public static RandMusic S = null;

    public List<string> choose;

    public RandMusic()
    {
        choose = new List<string>();
        S = this;
    }
}